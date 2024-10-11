using UnityEngine;

public class LevelSelectorManager : SingletonBase<LevelSelectorManager>
{
    public LevelData currentLevel;
    public LevelData[] levels;

    protected override void Awake()
    {
        base.Awake();
        GameData gameData = Resources.Load<GameData>("Levels/GameData");
        levels = gameData.levels;
       
    }

    /// <summary>
    /// Method to find a level in the levels array giving a levelData
    /// </summary>
    /// <param name="levelData"></param>
    public LevelData FindLevel(LevelData levelData)
    {
       foreach(LevelData level in levels)
       {
           if(level == levelData)
           {
                return level;

           }
       }
       return null;
    }
}