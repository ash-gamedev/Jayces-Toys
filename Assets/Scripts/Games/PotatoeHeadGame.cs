using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PotatoeHeadGame : Game
{
    List<PotatoeHeadPart> potatoeHeadParts;
    List<DraggableAnimation> draggableAnimations;
    DragController dragController;

    System.Random random;
    int index = 0;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();
        dragController = FindObjectOfType<DragController>();
        potatoeHeadParts = FindObjectsOfType<PotatoeHeadPart>().ToList();
        draggableAnimations = FindObjectsOfType<DraggableAnimation>().ToList();

        OnPrepareLevel();
    }

    public override void OnPrepareLevel()
    {
        base.OnPrepareLevel();

        SelectPotatoeHeadParts();
    }

    public override void OnPlayLevel()
    {
        base.OnPlayLevel();

        // drag controller controls the gameplay for this level
        dragController.OnUpdate();
    }

    public override bool IsLevelComplete()
    {
        return dragController?.AllTargetsReached() == true;
    }
    #endregion

    #region Game Helper Functions

    public void SelectPotatoeHeadParts()
    {
        // select new sprite & reset the positions
        foreach(PotatoeHeadPart potatoeHeadPart in potatoeHeadParts)
        {
            potatoeHeadPart.SetSpriteByIndex(index);
        }

        StartCoroutine(StartingPotatoeHeadAnimation());

        index++;
    }

    IEnumerator StartingPotatoeHeadAnimation()
    {
        yield return new WaitForSeconds(1f);

        foreach (DraggableAnimation draggableAnimation in draggableAnimations)
        {
            draggableAnimation.StartAnimation();
            yield return new WaitForSeconds(0.1f);
        }

        // wait for movement to finish
        yield return new WaitForSeconds(draggableAnimations[0].movementTime);

        // play level
        OnPlayLevel();
    }
    #endregion
}