using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour, IGame
{
    public EnumGameState GameState
    {
        get { return gameState; }
    }
    private EnumGameState gameState;

    [HideInInspector] public int levelsCompleted = 0;

    public virtual void OnPrepareGame()
    {
        gameState = EnumGameState.GamePrepare;
    }

    public virtual void OnPrepareLevel()
    {
        gameState = EnumGameState.LevelPrepare;
    }

    public virtual void OnPlayLevel()
    {
        gameState = EnumGameState.LevelInPlay;
    }

    public virtual void OnLevelComplete()
    {
        gameState = EnumGameState.LevelCompleted;

        AudioManager.instance.PlaySoundEffect(EnumSoundName.Victory);

        // increment level complete
        levelsCompleted += 1;

        StartCoroutine(WaitAndPrepareNextLevel());
    }

    public virtual void OnShowHint()
    {

    }

    public virtual IEnumerator WaitAndPrepareNextLevel()
    {
        yield return new WaitForSeconds(1.5f);

        // check if 3 levels were completed to end game
        if (levelsCompleted == 3)
            OnGameComplete();
        else
            OnPrepareLevel();
    }

    public virtual void OnGameComplete()
    {
        gameState = EnumGameState.GameCompleted;
    }

    public virtual bool IsLevelComplete()
    {
        return false;
    }
}