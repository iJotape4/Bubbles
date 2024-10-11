using Events;
using TMPro;
using UnityEngine;

public class GoalText : MonoBehaviour
{
    /// <summary>
    /// Main text to show the score
    /// </summary>
    [SerializeField] private TextMeshProUGUI text;
    /// <summary>
    /// Shadow text to give a better look
    /// </summary>
    [SerializeField] private TextMeshProUGUI shadowText;
    float levelGoalScore;
    private void Awake()
    {
        EventManager.AddListener<int>(LevelInitializationEvents.LevelGoal, SetGoalText);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(LevelInitializationEvents.LevelGoal, SetGoalText);
    }

    /// <summary>
    /// Set the Goal Text. Is called at level start
    /// </summary>
    /// <param name="eventData"></param>
    private void SetGoalText(int eventData)
    {
        levelGoalScore = eventData;
        text.text = levelGoalScore.ToString();
        shadowText.text = levelGoalScore.ToString();
    }
}