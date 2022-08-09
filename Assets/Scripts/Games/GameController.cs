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
        StartCoroutine(WaitForSceneTransitionAndStartGame());
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

    IEnumerator WaitForSceneTransitionAndStartGame()
    {
        LevelSelector levelSelector = FindObjectOfType<LevelSelector>();
        yield return new WaitUntil(() => levelSelector.transitionPanel.activeSelf == false);

        game.OnPrepareGame();
    }

    void ShowGameCompleteMenu()
    {
        if(levelFinishedPanel.active == false)
        {
            // show menu
            levelFinishedPanel.SetActive(true);

            // hide back button
            backButton.SetActive(false);
        }
    }
}