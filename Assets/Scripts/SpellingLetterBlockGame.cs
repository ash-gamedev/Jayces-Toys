using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellingLetterBlockGame : MonoBehaviour
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

    #region Start, Update, etc.

    // Use this for initialization
    void Start()
    {
        random = new System.Random();

        words = new List<string>
        {
            "Count"
        };

        SelectNextWord();
    }

    #endregion

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
            letterBlockInstance.GetComponent<LetterBlock>().SetLetter(letter);

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
    }

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

    #endregion
}