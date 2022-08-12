using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// used to name/identify the various sounds such as music tracks or sound effects
public enum EnumSoundName
{
    MainTheme,
    SoundVolumeChange,
    IntroSoundEffect,
    DraggablePickUp,
    DraggableDrop,
    Drawing,
    Victory,
    DraggableSwish,
    CardFlip,
    CardDeal,
    CardMatch,
    CardMismatch,
    JigglingParts,
    LevelButtonClick,
    PopSound
}

public enum EnumGameState
{
    GamePrepare,
    LevelPrepare,
    LevelInPlay,
    LevelCompleted,
    GameCompleted
}
