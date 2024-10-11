using System.IO;
using UnityEngine;

public class JsonReader:MonoBehaviour
{
    private const string nameSaveData= "SaveDataUser.json"; 
    public static void SaveStarsAchivement(int level,int data)
    {
        SaveStars stars = loadData();
        if (stars.starsAchivement[level] < data)
        {
            stars.addData(level, data);
            SaveData(stars);
        }
       
    }
    public static void SaveData(SaveStars stars)
    {
        string infoJson = JsonUtility.ToJson(stars);

        string path = Path.Combine(Application.persistentDataPath, nameSaveData);
        Debug.Log(path);
        SaveJsonInPath(path, infoJson);
    }
    public static SaveStars loadData()
    {
          
        string path = Path.Combine(Application.persistentDataPath, nameSaveData);

        if (!Directory.Exists(Application.persistentDataPath))
            Directory.CreateDirectory(Application.persistentDataPath);

        if (!File.Exists(path))
        {
            GameData gameData = Resources.Load<GameData>("Levels/GameData");
            SaveStars stars = new SaveStars(gameData.levels.Length);
            string infoJson = JsonUtility.ToJson(stars);
            File.WriteAllText(path, infoJson);
        }
        string infoStarsAchivement = File.ReadAllText(path);
        SaveStars saveStars = JsonUtility.FromJson<SaveStars>(infoStarsAchivement);
        return saveStars;
    }
    //Method to read the json file from path without resources
    public static string LoadJSONAsFile(string path)
    {
        return File.ReadAllText(path);
    }

    //convert the string Json to a T object
    public static T LoadJsondata<T>(string path) where T : new()
    {
        string json = LoadJSONAsFile(path);
        T jsonData = new T();
        JsonUtility.FromJsonOverwrite(json, jsonData);
        return  jsonData;

    }
    
    //Convert a Object to JSON
    public static string SaveLevelData<T>(T levelData)
    {
        return JsonUtility.ToJson(levelData,true);
    }

    //Save Json In a path
    public static void SaveJsonInPath(string path, string json)
    {
        if (!File.Exists(path)){ File.Create(path);   }
        
        File.WriteAllText(path, json);
    }
}