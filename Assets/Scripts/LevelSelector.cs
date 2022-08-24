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

    public void LoadLevelSelectorScene(bool playSoundEffect = true)
    {
        LoadScene("LevelSelection", playSoundEffect);
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

    public void LoadScene(string sceneName, bool playSoundEffect = true)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName, playSoundEffect));
    }

    IEnumerator FadeInAndDisablePanel()
    {
        // fade in playes automatically, wait for it to finish
        yield return new WaitForSeconds(0.45f);

        transitionPanel.SetActive(false);
    }

    IEnumerator FadeOutAndLoadScene(string sceneName, bool playSoundEffect = true)
    {
        // first, pause all sound effect
        AudioManager.instance?.StopAllSoundEffects();

        // play "click" sound effect when switching levels
        if (playSoundEffect)
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.LevelButtonClick);

        // fade out
        transitionPanel.SetActive(true);
        transitionPanelAnimator.Play("FadeOut");

        yield return new WaitForSeconds(1f);

        // stop all sound effects (if any)
        AudioManager.instance?.StopAllSoundEffects();

        SceneManager.LoadScene(sceneName);
    }
}
