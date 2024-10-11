public enum InGameEvents
{
    UpdateBubbleScales,
    BubbleMerged,
    NewScore,
    BombExploded,
    MovementFinished,
    GetMovements,
}

public enum LevelFlowEvents
{
    LevelStarted,
    LevelPaused,
    LevelResumed,
    LevelEnded,
    LevelLoaded,
    LevelCompleted,
    LevelFailed,
    GameOverLoose,
    GameOverWon,
    levelSongState,
}
public enum SongsEvents
{
    PlaySFX,
    LoopSong,
    StopSong,
    PlayMusic,
    EnableMusic,
    StopSounds,
    EnableSounds,
}
public enum LevelInitializationEvents
{
    LevelMaxMovements,
    LevelGoal,
}