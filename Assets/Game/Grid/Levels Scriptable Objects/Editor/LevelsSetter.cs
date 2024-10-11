#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LevelsSetter
{
    static GameData gameData;
    static string JSONpath = "Assets/Game/Grid/Levels Scriptable Objects/Resources/Levels/final_bubbles_50_levels_with_obstacles.json";

    [MenuItem("Tools/Bubbles/Set Levels",false, 1)]
    public static void SetLevels()
    {
        gameData = Resources.Load<GameData>("Levels/GameData");
        GameData jsonData = JsonReader.LoadJsondata<GameData>(JSONpath);
       
        if (jsonData != null )
        {
            gameData.levels = new LevelData[jsonData.levels.Length];
           for(int i = 0; i < jsonData.levels.Length; i++)
           {
                gameData.levels[i] = jsonData.levels[i];
           }
            EditorUtility.SetDirty(gameData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Levels Setted successfully");
        }
    }
}
#endif