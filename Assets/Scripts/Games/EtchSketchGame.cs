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

        // play level
        OnPlayLevel();
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
        return lineIndex == currentShape?.Points.Count;
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
        Transform closestPoint = currentShape.Points.Where(x => Mathf.Abs(Vector3.Distance(x.position, draggingPoint.transform.position)) < 1).FirstOrDefault();

        if(closestPoint != null && currentShape.AreNeighbours(closestPoint, startPoint.transform) && !currentShape.IsLine(closestPoint, startPoint.transform))
        {
            // sound effect
            AudioManager.instance.PlaySoundEffect(EnumSoundName.DraggableDrop);

            // snap to point
            draggingPoint.transform.position = closestPoint.transform.position;

            // add line
            Tuple<Transform, Transform> line = new Tuple<Transform, Transform>(startPoint.transform, closestPoint);
            currentShape.LinesDrawn.Add(line);

            lineIndex++;

            if (IsLevelComplete())
            {
                currentShape.ShowShape(true);
            }
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
        // remove previous shape 
        if(currentShape != null)
            Destroy(currentShape.gameObject);

        // remove previous lines
        lineIndex = 0;
        if (lines != null)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                lines[i].RemoveLine();
                Destroy(lines[0].gameObject);
            }
        }
        
        // get random shape
        int index = random.Next(shapes.Count);
        currentShape = Instantiate(shapes[index]).GetComponent<Shape>();

        // add points (+ add start point to end (to complete the shapes))
        currentShapeDottedLinePoints.Clear();
        currentShapeDottedLinePoints.AddRange(currentShape.Points);
        currentShapeDottedLinePoints.Add(currentShapeDottedLinePoints[0]);

        // remove from list (so it doesn't get selected again)
        shapes.RemoveAt(index);

        // instantiate dotted line for shape
        InstantiateDottedLine();

        // set up line controllers for lines
        InstantiateLineControllers();
    }

    public void InstantiateDottedLine()
    {
        dottedLine.SetUpLine(currentShapeDottedLinePoints);
    }

    public void InstantiateLineControllers()
    {
        lines = new List<LineController>();
        foreach(Transform point in currentShape.Points)
        {
            GameObject lineInstance = Instantiate(LinePrefab);
            lines.Add(lineInstance.GetComponent<LineController>());
        }
    }
}