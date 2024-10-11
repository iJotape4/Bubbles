using Events;
using UnityEngine;

public class LevelFlowManager : MonoBehaviour
{
    public int currentLevel { get; private set; }
    public int levelGoal { get; private set; }
    public int currentScore { get; private set; }
    public int maxMovements { get; private set; }
    public float currentMovements { get; private set; }

    private void Awake()
    {
        EventManager.AddListener(LevelFlowEvents.LevelEnded, OnLevelEnded);
        EventManager.AddListener(LevelFlowEvents.LevelFailed, OnLevelfailed);
        EventManager.AddListener(LevelFlowEvents.LevelCompleted, OnLevelCompleted);
        EventManager.AddListener<int>(InGameEvents.NewScore, CheckGoalReached);
        EventManager.AddListener<float>(InGameEvents.GetMovements, updateMovements);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(LevelFlowEvents.LevelEnded, OnLevelEnded);
        EventManager.RemoveListener(LevelFlowEvents.LevelFailed, OnLevelfailed);
        EventManager.RemoveListener(LevelFlowEvents.LevelCompleted, OnLevelCompleted);
        EventManager.RemoveListener<int>(InGameEvents.NewScore, CheckGoalReached);
        EventManager.RemoveListener<float>(InGameEvents.GetMovements, updateMovements);
    }

    /// <summary>
    /// Check if the goal was reached when level ends
    /// </summary>
    private void OnLevelEnded()
    {
       if(currentScore >= levelGoal)
       {
            EventManager.Dispatch(LevelFlowEvents.LevelCompleted);        
       }
        else       
            EventManager.Dispatch(LevelFlowEvents.LevelFailed);      
    }

    /// <summary>
    /// Check if the goal is reached everyTime a new score is set
    /// </summary>
    /// <param name="eventData"></param>
    private void CheckGoalReached(int eventData)
    {
        currentScore = eventData;
        if (currentScore >= levelGoal)
            EventManager.Dispatch(LevelFlowEvents.LevelEnded);
    }
    /// <summary>
    /// Set Max Movements when level is initialized
    /// </summary>
    /// <param name="_maxMovements"></param>
    public void SetMaxMovements(int _maxMovements)
    {
        maxMovements = _maxMovements;
        EventManager.Dispatch(LevelInitializationEvents.LevelMaxMovements, _maxMovements);
    }
    /// <summary>
    /// Set Level goal when level is initialized
    /// </summary>
    /// <param name="_levelGoal"></param>
    public void SetLevelGoal(int _level,int _levelGoal)
    {
        currentLevel = _level;
        Debug.Log("levelCurrent: "+currentLevel);
        levelGoal = _levelGoal;
        EventManager.Dispatch(LevelInitializationEvents.LevelGoal, _levelGoal);
    }

    /// <summary>
    /// Sends the final score when the level is failed
    /// </summary>
    private void OnLevelfailed()
    {
        EventManager.Dispatch(LevelFlowEvents.GameOverLoose, currentScore);
    }

    private void OnLevelCompleted()
    {
        int starAchieved = CheckStars();
        JsonReader.SaveStarsAchivement(currentLevel - 1, starAchieved);
        int[] results = { currentScore, starAchieved };
        EventManager.Dispatch(LevelFlowEvents.GameOverWon,results);
    }
    private void updateMovements(float movements)
    {
        currentMovements = movements;
    }
    /// <summary>
    /// Check if the currentMovementsS reached the stars goals
    /// </summary>
    public int CheckStars()
    {
        LevelData levelData = LevelSelectorManager.Instance.currentLevel;
        int starAchieved=0;
        if (currentMovements >= levelData.starsGoals[1])
            starAchieved= 3;
        else if (currentMovements >= levelData.starsGoals[0])
            starAchieved = 2;
        else
            starAchieved = 1;
        return starAchieved;
    
      
    }
}