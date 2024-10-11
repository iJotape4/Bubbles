using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    // Generate a level based on the level number (can influence difficulty)
    public Level GenerateLevel(int levelNumber)
    {
        //Level newLevel = new Level(levelNumber, CalculateMovesLimit(levelNumber), CalculateObjectiveScore(levelNumber));

        //// Generate item positions and types based on some logic
        //for (int i = 0; i < gridHeight; i++)
        //{
        //    for (int j = 0; j < gridWidth; j++)
        //    {
        //        // Example logic to decide whether to place an item in each grid cell
        //        if (Random.value > 0.5f) // 50% chance to spawn an item
        //        {
        //            Vector2Int position = new Vector2Int(j, i);
        //            ItemType itemType = ChooseRandomItemType();
        //            newLevel.AddItemPosition(position, itemType);
        //        }
        //    }
        //}

        //return newLevel;
        return null;
    }

   
}
