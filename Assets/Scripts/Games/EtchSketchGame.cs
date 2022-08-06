using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EtchSketchGame : Game
{
    [SerializeField] private List<Transform> shapeTransforms; 
    [SerializeField] private LineController dottedLine;

    private List<Transform> currentShape;
    System.Random random;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();

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
    }

    public override bool IsLevelComplete()
    {
        return false;
    }
    #endregion

    public void SelectNextShape()
    {
        // get random shape
        int index = random.Next(shapeTransforms.Count);
        currentShape = shapeTransforms[index].GetComponentsInChildren<Transform>().ToList();

        // remove first point (center/parent)
        currentShape.RemoveAt(0);

        // add start point to end (to complete the shapes)
        currentShape.Add(currentShape[0]);

        // remove from list (so it doesn't get selected again)
        shapeTransforms.RemoveAt(index);

        // instantiate letter blocks for word
        InstantiateDottedLine();
    }

    public void InstantiateDottedLine()
    {
        dottedLine.SetUpLine(currentShape);
    }
}