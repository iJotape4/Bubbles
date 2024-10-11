using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovementsCounter : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text; 
    float maxMovements, currentMovements =0;

    private void Awake()
    {
        EventManager.AddListener<int>(LevelInitializationEvents.LevelMaxMovements, SetMaxMovements);
        EventManager.AddListener(InGameEvents.MovementFinished, UpdateMovements);
        EventManager.AddListener(InGameEvents.BubbleMerged, UpdateMovements);
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener<int>(LevelInitializationEvents.LevelMaxMovements, SetMaxMovements);
        EventManager.RemoveListener(InGameEvents.MovementFinished, UpdateMovements);
        EventManager.RemoveListener(InGameEvents.BubbleMerged, UpdateMovements);
    }

    /// <summary>
    /// Update the movements counter text and sliders
    /// Dispatch an event when there are no available movements
    /// </summary>
    private void UpdateMovements()
    {
        currentMovements++;
        slider.value = 1 -( currentMovements/maxMovements);
        text.text= (maxMovements - currentMovements).ToString();
        EventManager.Dispatch(InGameEvents.GetMovements, maxMovements-currentMovements);
        if(currentMovements >= maxMovements)
        {
            EventManager.Dispatch(LevelFlowEvents.LevelEnded);
            text.text = 0.ToString();
        }
    }

    /// <summary>
    /// Set the Max Movments counter. Is called at level start
    /// </summary>
    /// <param name="eventData"></param>
    private void SetMaxMovements(int eventData)
    {
       maxMovements = eventData;
       text.text = maxMovements.ToString();
    }
}