﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpellingLetterBlockGame : Game
{
    // serialize fields
    [SerializeField] Transform placeholderLetterBlocksGroup;
    [SerializeField] Transform letterBlocksGroup;
    [SerializeField] List<GameObject> letterBlockPrefabs;
    [SerializeField] List<GameObject> placeholderLetterBlockPrefabs;

    // public fields
    public List<string> words;
    public string currentWord;

    System.Random random;

    DragController dragController;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();
        dragController = FindObjectOfType<DragController>();

        words = new List<string>
        {
            "Count", "Start", "Light", "Dog", "Cat", "Drive", "Frog"
        };

        OnPrepareLevel();
    }

    public override void OnPrepareLevel()
    {
        base.OnPrepareLevel();

        // set up
        SelectNextWord();
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

    #region Game functions

    public void SelectNextWord()
    {
        // get random word
        int index = random.Next(words.Count);
        currentWord = words[index];

        // remove word from list (so it doesn't get selected again)
        words.Remove(currentWord);

        // instantiate letter blocks for word
        InstantiateLetterBlocks();
    }

    private void InstantiateLetterBlocks()
    {
        // delete old letters
        foreach (Transform child in letterBlocksGroup.transform)
            GameObject.Destroy(child.gameObject);
        foreach (Transform child in placeholderLetterBlocksGroup.transform)
            GameObject.Destroy(child.gameObject);

        // get x positions
        List<int> xPositions = GetXPositionIndexes();
        List<int> xPositionsShuffled = Shuffle(xPositions);

        // create letter block for each letter
        for (int i = 0; i < currentWord.Length; i++)
        {
            // instantiate letter block
            int xPositionShuffled = xPositionsShuffled[i];
            GameObject letterBlockPrefab = letterBlockPrefabs[i];
            GameObject letterBlockInstance = Instantiate(letterBlockPrefab, letterBlocksGroup);
            letterBlockInstance.transform.position += new Vector3(xPositionShuffled, 0, 0);

            // set letter
            string letter = currentWord[i].ToString();
            LetterBlock letterBlock = letterBlockInstance.GetComponent<LetterBlock>();
            letterBlock.SetLetter(letter);
            letterBlock.GetComponent<Draggable>().StartPosition = letterBlock.transform.position;

            // instantiate letter block placeholder
            int xPosition = xPositions[i];
            GameObject placeholderLetterBlockPrefab = placeholderLetterBlockPrefabs[i];
            GameObject placeholderLetterBlockInstance = Instantiate(placeholderLetterBlockPrefab, placeholderLetterBlocksGroup);
            placeholderLetterBlockInstance.transform.position += new Vector3(xPosition, 0, 0);

            // set letter
            placeholderLetterBlockInstance.GetComponent<LetterBlock>().SetLetter(letter);
            
            // set drag target
            letterBlockInstance.GetComponent<Draggable>().DragTarget = placeholderLetterBlockInstance.GetComponent<DragTarget>();
        }

        StartCoroutine(StartingBlockAnimation());
    }
    #endregion


    #region Helper functions

    private List<int> GetXPositionIndexes()
    {
        List<int> xIndexes = new List<int>();

        int xPosition = 0;
        if (currentWord.Length == 3)
            xPosition = -3;
        else if (currentWord.Length == 4)
            xPosition = -4;
        else
            xPosition = -6;

        xIndexes.Add(xPosition);

        for(int i = 0; i < currentWord.Length - 1; i++)
        {
            xPosition += 3;
            xIndexes.Add(xPosition);
        }

        return xIndexes;
    }

    private List<int> Shuffle(List<int> list)
    {
        List<int> listCopy = new List<int>(list);
        int n = listCopy.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            int value = listCopy[k];
            listCopy[k] = listCopy[n];
            listCopy[n] = value;
        }

        return listCopy;
    }

    IEnumerator StartingBlockAnimation()
    {
        yield return new WaitForSeconds(1f);

        List<DraggableAnimation> draggableAnimations = FindObjectsOfType<DraggableAnimation>().ToList();
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