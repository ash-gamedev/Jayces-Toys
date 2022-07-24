using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGame
{
    EnumGameState GameState { get; }
    void OnPrepareGame();
    void OnPrepareLevel();
    void OnPlayLevel();
    void OnLevelComplete();
    bool IsLevelComplete();
}
