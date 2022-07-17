using System.Collections;
using UnityEngine;

public class PotatoeHeadGame : MonoBehaviour
{
    DragController dragController;
    Coroutine startGame;

    // Use this for initialization
    void Start()
    {
        dragController = FindObjectOfType<DragController>();
    }

    private void Update()
    {
        if (dragController?.AllTargetsReached() == true && startGame == null)
        {
            startGame = StartCoroutine(WaitAndStartNextGame());
        }
    }

    IEnumerator WaitAndStartNextGame()
    {
        AudioManager.instance.PlaySoundEffect(EnumSoundName.Victory);
        yield return new WaitForSeconds(3f);
        startGame = null;
    }
}