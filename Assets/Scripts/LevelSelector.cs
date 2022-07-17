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
}
