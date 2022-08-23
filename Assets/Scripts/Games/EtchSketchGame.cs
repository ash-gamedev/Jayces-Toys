using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class EtchSketchGame : Game
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private List<GameObject> shapes;
    [SerializeField] private LineController dottedLine;
    [SerializeField] private GameObject LinePrefab;
    [SerializeField] private List<Color> colors;

    List<LineController> lines;
    LineController line => lines[lineIndex];

    private Shape currentShape;
    private List<Transform> currentShapeDottedLinePoints;
    System.Random random;

    // fields for drawing lines 
    private Vector3 worldPosition;
    bool isDragActive = false;
    Point startPoint;
    Point draggingPoint;
    int lineIndex = 0;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();
        currentShapeDottedLinePoints = new List<Transform>();

        OnPrepareLevel();
    }

    public override void OnPrepareLevel()
    {
        base.OnPrepareLevel();

        // select shape
        SelectNextShape();
    }

    public override void OnPlayLevel()
    {
        base.OnPlayLevel();

        if (Touchscreen.current?.primaryTouch?.press?.isPressed != null)
        {
            if (!Touchscreen.current.primaryTouch.press.isPressed)
            {
                if (isDragActive)
                    Drop();
                return;
            }

            // get touch position
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // convert to world space
            worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            // check if drag is already active
            if (isDragActive)
            {
                Drag();
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                if (hit.collider != null)
                {
                    Point draggable = hit.transform.gameObject.GetComponent<Point>();
                    if (draggable != null && currentShape?.IsPointFullyConnected(draggable.transform) == false)
                    {
                        startPoint = draggable;
                        draggingPoint = Instantiate(pointPrefab, currentShape.transform).GetComponent<Point>();
                        InitDrag();
                    }
                }
            }
        }
    }

    public override bool IsLevelComplete()
    {
        // count how many lines have been drawn
        return currentShape.LinesDrawn.Count() == currentShape?.Points.Count;
    }
    #endregion

    #region Game functions
    void InitDrag()
    {
        AudioManager.instance.PlaySoundEffect(EnumSoundName.DraggablePickUp);
        UpdateDragStatus(true);
    }

    void Drag()
    {
        draggingPoint.transform.position = new Vector3(worldPosition.x, worldPosition.y, draggingPoint.transform.position.z);
        line.SetUpLine(new List<Transform>(){ startPoint.transform, draggingPoint.transform });
    }

    void Drop()
    {
        // Get closes point
        Transform closestPoint = currentShape.TransformPoints.Where(x => Mathf.Abs(Vector3.Distance(x.position, draggingPoint.transform.position)) < 1.5).FirstOrDefault();

        if(closestPoint != null && currentShape.AreNeighbours(closestPoint, startPoint.transform) && !currentShape.IsLine(closestPoint, startPoint.transform))
        {
            // sound effect
            AudioManager.instance.PlaySoundEffect(EnumSoundName.DraggableDrop);

            // snap to point
            draggingPoint.transform.position = closestPoint.transform.position;

            // add line
            Tuple<Vector3, Vector3> line = new Tuple<Vector3, Vector3>(startPoint.transform.position, closestPoint.position);
            currentShape.LinesDrawn.Add(line);

            lineIndex++;
        }
        else
        {
            // sound effect
            AudioManager.instance.PlaySoundEffect(EnumSoundName.DraggableSwish);

            // remove line
            line.RemoveLine();

            // destroy current drag
            Destroy(draggingPoint.gameObject);
        }
        
        UpdateDragStatus(false);
    }

    void UpdateDragStatus(bool isDragging)
    {
        isDragActive = isDragging;
    }
    #endregion

    public void SelectNextShape()
    {
        ClearPreviousShapeLines();
        lineIndex = 0;

        // remove previous shape 
        if (currentShape != null)
            Destroy(currentShape.gameObject);
        
        // get random shape
        int index = random.Next(shapes.Count);
        currentShape = Instantiate(shapes[index]).GetComponent<Shape>();

        // add points (+ add start point to end (to complete the shapes))
        currentShapeDottedLinePoints.AddRange(currentShape.TransformPoints);
        currentShapeDottedLinePoints.Add(currentShapeDottedLinePoints[0]);

        // remove from list (so it doesn't get selected again)
        shapes.RemoveAt(index);

        // set up line controllers for lines
        InstantiateLineControllers();

        StartCoroutine(InstantiatePointsAndLines());
    }

    private IEnumerator InstantiatePointsAndLines()
    {
        foreach (var point in currentShape.TransformPoints)
        {
            point.transform.localScale = Vector3.zero;
        }

        yield return new WaitForSeconds(1f);

        foreach (var point in currentShape.TransformPoints)
        {
            yield return new WaitForSeconds(0.35f);
            point.transform.DOScale(1f, 1f).SetEase(Ease.OutBounce);
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.PopSound);
        }

        yield return new WaitForSeconds(0.35f);

        yield return InstantiateDottedLine();

        // play level
        OnPlayLevel();
    }

    public void ClearPreviousShapeLines()
    {
        dottedLine?.RemoveLine();
        currentShapeDottedLinePoints?.Clear();

        // remove lines & points
        if (lines != null)
        {
            // remove lines
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                lines[i].RemoveLine();
                Destroy(lines[i].gameObject);
            }

            // remove transforms
            for (int i = currentShape.TransformPoints.Count - 1; i >= 0; i--)
            {
                Destroy(currentShape.TransformPoints[i].gameObject);
            }
        }
    }

    public void FadeShapeLines()
    {
        float fadeOut = 0.5f;

        // remove lines & points
        if (lines != null)
        {
            // remove lines
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                lines[i].lr.material.DOFade(0f, fadeOut);
            }

            // remove transforms
            for (int i = currentShape.TransformPoints.Count - 1; i >= 0; i--)
            {
                currentShape.TransformPoints[i].GetComponent<SpriteRenderer>().material.DOFade(0f, fadeOut);
            }
        }

        dottedLine?.RemoveLine();
    }

    public IEnumerator InstantiateDottedLine()
    {
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.Drawing);

        foreach (Transform transform in currentShapeDottedLinePoints)
        {
            dottedLine.AddLine(transform);
            yield return new WaitForSeconds(0.35f);
        }

        AudioManager.instance?.StopSoundEffect(EnumSoundName.Drawing);
    }

    public void InstantiateLineControllers()
    {
        lines = new List<LineController>();
        foreach(Transform point in currentShape.TransformPoints)
        { 
            GameObject lineInstance = Instantiate(LinePrefab);
            lines.Add(lineInstance.GetComponent<LineController>());
        }
    }

    public override IEnumerator WaitAndPrepareNextLevel()
    {
        FadeShapeLines();
        currentShape.ShowShape(true, PickRandomColor());

        yield return new WaitForSeconds(3.5f);

        // check if 3 levels were completed to end game
        if (levelsCompleted == 3)
            OnGameComplete();
        else
            OnPrepareLevel();
    }

    public Color PickRandomColor()
    {
        // get random shape
        int index = random.Next(colors.Count);
        Color color = colors[index];

        // remove from list (so it doesn't get selected again)
        colors.RemoveAt(index);

        return color;
    }
}