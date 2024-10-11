using UnityEngine;

[CreateAssetMenu(fileName = "NewGameLevelsData", menuName = "GameLevels Data")]
public class GameData :ScriptableObject
{
    public LevelData[] levels;
}