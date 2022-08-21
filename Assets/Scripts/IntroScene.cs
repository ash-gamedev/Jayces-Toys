using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class IntroScene : MonoBehaviour
{
    [SerializeField] float fadeTime = 1f;
    [SerializeField] List<GameObject> colourBlocks = new List<GameObject>();

    private void Start()
    {
        AudioManager.instance?.StopMusicTrack();
        StartCoroutine(ItemsAnimation());
    }

    IEnumerator ItemsAnimation()
    {
        foreach (var colourBlock in colourBlocks)
        {
            colourBlock.transform.localScale = Vector3.zero;
        }

        foreach (var colourBlock in colourBlocks)
        {
            colourBlock.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.PopSound);
            yield return new WaitForSeconds(0.25f);
        }

        AudioManager.instance?.PlaySoundEffect(EnumSoundName.IntroSoundEffect);

        yield return new WaitForSeconds(0.5f);

        AudioManager.instance?.PlayMusicTrack(EnumSoundName.MainTheme);

        yield return new WaitForSeconds(2f);

        FindObjectOfType<LevelSelector>().LoadLevelSelectorScene(false);
    }
}