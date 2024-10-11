using Events;
using System;
using TMPro;
using UnityEngine;

public class CurrentScoreText : MonoBehaviour
{
    /// <summary>
    /// Main text to show the score
    /// </summary>
    [SerializeField] private TextMeshProUGUI currentScoreText;
    /// <summary>
    /// Shadow text to give a better look
    /// </summary>
    [SerializeField] private TextMeshProUGUI shadowText;


    /// <summary>
    /// Subscribes to the event to update the score
    /// </summary>
    private void Awake()
    {
        EventManager.AddListener<int>(InGameEvents.NewScore, UpdateScore);
    }

    /// <summary>
    /// Unsubscribes from the event. Don't delete this method
    /// </summary>
    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(InGameEvents.NewScore, UpdateScore);
    }
    private void Start() =>  UpdateScore(0);   
    /// <summary>
    /// Updates the score text
    //</summary>
    /// <param name="eventData"></param>
    public void UpdateScore(int eventData)
    {
        string newScore = eventData.ToString();
        currentScoreText.text = newScore;
        shadowText.text = newScore;
    }
}