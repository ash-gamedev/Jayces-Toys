using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
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
    }
}