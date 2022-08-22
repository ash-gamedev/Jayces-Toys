using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // serilizable fields
    [SerializeField] GameObject levelFinishedPanel;
    [SerializeField] GameObject backButton;

    // the current game
    Game game;

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<Game>();
        game.OnPrepareGame();
    }

    // Update is called once per frame
    void Update()
    {
        // check if game in play
        if (game.GameState == EnumGameState.LevelInPlay)
            if (game.IsLevelComplete())
                game.OnLevelComplete();
            else
                game.OnPlayLevel();

        if (game.GameState == EnumGameState.GameCompleted)
            ShowGameCompleteMenu();

    }

    void ShowGameCompleteMenu()
    {
        if(levelFinishedPanel.active == false)
        {
            StartCoroutine(ShowGameCompleteMenuAndReturnToMainMenu());
        }
    }

    IEnumerator ShowGameCompleteMenuAndReturnToMainMenu()
    {
        // show menu
        levelFinishedPanel.SetActive(true);

        // hide back button
        backButton.SetActive(false);

        // lower volume
        AudioManager.instance?.FadeMusicVolume(0.3f, EnumSoundName.MainTheme);

        yield return new WaitForSeconds(0.5f);

        AudioManager.instance?.PlaySoundEffect(EnumSoundName.ApplauseSound);

        yield return new WaitForSeconds(2f);

        AudioManager.instance?.PlaySoundEffect(EnumSoundName.YaySound);

        yield return new WaitForSeconds(2f);

        AudioManager.instance?.FadeMusicVolume(1f, EnumSoundName.MainTheme);

        yield return new WaitForSeconds(4f);

        FindObjectOfType<LevelSelector>().LoadLevelSelectorScene(false);
    }
}