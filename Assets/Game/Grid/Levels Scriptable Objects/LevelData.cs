using MyBox;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    [Space(10)]
    public int targetGoal;
    public int moveLimit;

    [SerializeField] public int[] starsGoals = new int[2];
    [SerializeField, ReadOnly] public int starsAchieved;
   
    public BubbleData[] bubblesData;
    public ObstacleData[] obstacles = new ObstacleData[] { };
}

[System.Serializable]
public struct BubbleData
{
    public BubbleType bubbleType;
    public int bubbleNumber;
    public Vector2Int position;
    public float radius;
}

public enum BubbleType
{
    Player =0,
    Positive =1,
    Negative =2,
    Swap =3
}

[System.Serializable]
public struct ObstacleData
{
    public Vector2Int position;
    public Vector2Int targetPosition;
    public float speed,radius;
    public int time,value;
    public ObstacleType obstacleType;
    public Direction direction;
}

public enum ObstacleType
{
    Static =0,
    Moving =1,
    ChainReactionBubble =2,
    Swap =3,
    TimerBubble =4,
    Bubblewind =5,
    GravityWell = 6,
    Mirror = 7,
    ColorSwitch = 8,
}