using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public GameObject transitionPanel;
    Animator transitionPanelAnimator;

    private void Start()
    {
        transitionPanel = GameObject.FindGameObjectWithTag("TransitionPanel");
        transitionPanelAnimator = transitionPanel.GetComponent<Animator>();

        StartCoroutine(FadeInAndDisablePanel());
    }

    public void LoadLevelSelectorScene()
    {
        LoadScene("LevelSelection");
    }

    public void LoadPotatoeHeadScene()
    {
        LoadScene("PotatoeHead");
    }

    public void LoadEtchSketchScene()
    {
        LoadScene("EtchSketch");
    }

    public void LoadSpellingLetterBlocksScene()
    {
        LoadScene("SpellingLetterBlocks");
    }

    public void LoadMatchingCardGameScene()
    {
        LoadScene("MatchingCardGame");
    }

    public void ReLoadLevel()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    IEnumerator FadeInAndDisablePanel()
    {
        // fade in playes automatically, wait for it to finish
        yield return new WaitForSeconds(0.45f);

        transitionPanel.SetActive(false);
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // first, pause all sound effect
        AudioManager.instance?.PauseAllSoundEffects();

        // play "click" sound effect when switching levels
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.LevelButtonClick);

        // fade out
        transitionPanel.SetActive(true);
        transitionPanelAnimator.Play("FadeOut");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
