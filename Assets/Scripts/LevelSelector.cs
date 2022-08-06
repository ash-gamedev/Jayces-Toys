using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LoadLevelSelectorScene()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void LoadPotatoeHeadScene()
    {
        SceneManager.LoadScene("PotatoeHead");
    }

    public void LoadEtchSketchScene()
    {
        SceneManager.LoadScene("EtchSketch");
    }

    public void LoadSpellingLetterBlocksScene()
    {
        SceneManager.LoadScene("SpellingLetterBlocks");
    }

    public void LoadMatchingCardGameScene()
    {
        SceneManager.LoadScene("MatchingCardGame");
    }

    public void ReLoadLevel()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex);
    }
}
