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
    List<int> indexes;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();
        indexes = new List<int> { 0, 1, 2 };
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

    public override void OnShowHint()
    {
        base.OnShowHint();
        dragController.Hint();
    }
    #endregion

    #region Game Helper Functions

    public void SelectPotatoeHeadParts()
    {
        // get index for potatoe head
        int randomIndex = random.Next(indexes.Count);
        int index = indexes[randomIndex];

        // remove from list (to not get selected again)
        indexes.Remove(index);

        // select new sprite & reset the positions
        foreach (PotatoeHeadPart potatoeHeadPart in potatoeHeadParts)
        {
            potatoeHeadPart.SetSpriteByIndex(index);
        }

        StartCoroutine(StartingPotatoeHeadAnimation());
    }

    IEnumerator StartingPotatoeHeadAnimation()
    {
        yield return new WaitForSeconds(1f);

        AudioManager.instance?.PlaySoundEffect(EnumSoundName.JigglingParts);

        foreach (DraggableAnimation draggableAnimation in draggableAnimations)
        {
            draggableAnimation.StartAnimation();
            yield return new WaitForSeconds(0.1f);
        }

        AudioManager.instance?.PlaySoundEffect(EnumSoundName.DraggableSwish);

        // wait for movement to finish
        yield return new WaitForSeconds(draggableAnimations[0].movementTime);

        // play level
        OnPlayLevel();
    }
    #endregion
}