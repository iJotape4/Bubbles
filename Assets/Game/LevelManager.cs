using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> predefinedLevels; // List of hand-crafted levels
    public LevelGenerator levelGenerator; // Component responsible for generating levels dynamically

    public void LoadLevel(int levelNumber)
    {
        // Load a predefined level based on levelNumber
        var hasLevel = predefinedLevels.Exists(x => x.LevelNumber == levelNumber);

        if (hasLevel)
        {
            Level levelToLoad = predefinedLevels.Find(x => x.LevelNumber == levelNumber);

        }
        else
        {
            LoadDynamicallyGeneratedLevel(levelNumber);
        }
    }


    private void LoadDynamicallyGeneratedLevel(int levelNumber)
    {
        // Use levelGenerator to create a new level dynamically
        Level newLevel = levelGenerator.GenerateLevel(levelNumber);
        // Implement the logic to set up the game board based on newLevel details
    }
}
