using DG.Tweening;
using Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubblesManager : SingletonBase<BubblesManager> 
{
    [SerializeField] private GameObject numberBubblePrefab, playerBubblePrefab, bombBubblePrefab, emptyBubblePrefab, staticBubblePrefab,
        movingBubblePrefab, explodeBubblePrefab, wildsObjectPrefab, swapBubblPrefab, timmerBubble, gravityWellPrefab, mirrorBubblePrefab, colorSwitchBubblePrefab;//obstacles and bubbles
    [SerializeField] private GameObject WarningPrefab;
    [SerializeField] private List<BubbleBase> bubbles = new List<BubbleBase>();
    [SerializeField] Transform[] directions = new Transform[4];
    [Header("Scale Bound Values")]
    //float minPositiveCircleScale = 0.07f; // The scale for the smallest number
    //float maxPositiveCircleScale = 0.2f; // The scale for the largest number
    //float minNegativeCircleScale = 0.07f; // The scale for the smallest number
    //float maxNegativeCircleScale = 0.2f; // The scale for the largest number
    float minPositiveCircleScale = 0.13f; // The scale for the smallest number
    float maxPositiveCircleScale = 0.2f; // The scale for the largest number
    float minNegativeCircleScale = 0.13f; // The scale for the smallest number
    float maxNegativeCircleScale = 0.2f; // The scale for the largest number

    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(InGameEvents.UpdateBubbleScales, SetBubblesScales);
        EventManager.AddListener<NumberBubble>(InGameEvents.BubbleMerged, RemoveDestroyedBubbleFromList);
    }
    public Vector2 getPosDirection(Direction direction) 
    {
        return directions[(int)direction].position; 
    }
    private void OnDestroy()
    {
        EventManager.RemoveListener(InGameEvents.UpdateBubbleScales, SetBubblesScales); 
        EventManager.RemoveListener<NumberBubble>(InGameEvents.BubbleMerged, RemoveDestroyedBubbleFromList);
    }
    public void ActiveWarning(Direction direction,float duration)
    {
        Transform Parent = directions[(int)direction].GetChild(0);
        GameObject warning = Instantiate(this.WarningPrefab,Parent);
        Destroy(warning,duration);
    }
    
    public void CreateBubble(Vector2 position, Vector2 force, int value, ItemType type)
    {
        GameObject prefabToInstantiate  = type switch
        {
            ItemType.NumberBubble => numberBubblePrefab,
            ItemType.EmptyBubble => emptyBubblePrefab,
            ItemType.SpecialBubble => bombBubblePrefab,
        };
        GameObject newBubble = Instantiate(prefabToInstantiate, position, Quaternion.identity);
        BubbleBase newBubbleComponent = newBubble.GetComponent<BubbleBase>();
        newBubbleComponent.Init(value);
        bubbles.Add(newBubbleComponent);
        //SortValues();
    }
    public void CreateBubble(BubbleData bubbleData, Vector3 position)
    {
        GameObject prefabToInstantiate = bubbleData.bubbleType switch
        {
            BubbleType.Player => playerBubblePrefab,
            BubbleType.Positive => numberBubblePrefab,
            BubbleType.Negative => numberBubblePrefab,
            //  BubbleType.Swap => swapBubblPrefab,      
            _ => numberBubblePrefab
        };

        GameObject newBubble = Instantiate(prefabToInstantiate, position, Quaternion.identity, this.transform);
        BubbleBase newBubbleComponent = newBubble.GetComponent<BubbleBase>();
        newBubbleComponent.Init(bubbleData.bubbleNumber);
        bubbles.Add(newBubbleComponent);
    }
    public void CreateObstacle(ObstacleData ObstacleData, Vector3 position)
    {
        GameObject prefabToInstantiate = ObstacleData.obstacleType switch
        {
            ObstacleType.Static=>staticBubblePrefab,
            ObstacleType.Moving => movingBubblePrefab,
            ObstacleType.ChainReactionBubble => explodeBubblePrefab,
            ObstacleType.Bubblewind => wildsObjectPrefab,
            ObstacleType.Swap => swapBubblPrefab,
            ObstacleType.TimerBubble=>timmerBubble,
            ObstacleType.GravityWell => gravityWellPrefab,
            ObstacleType.Mirror => mirrorBubblePrefab,
            ObstacleType.ColorSwitch => colorSwitchBubblePrefab,
            _ => numberBubblePrefab
        };
        GameObject newBubble = Instantiate(prefabToInstantiate, position, Quaternion.identity, this.transform);
            newBubble.GetComponent<Obstacles>().setData(ObstacleData);
    }

    public void RemoveDestroyedBubbleFromList(BubbleBase bubble)
    {
        bubbles.Remove(bubble);
        //When the only bubble in the playfield is the player's one, dispatch the level ended event
        if(bubbles.Count <=1)
            EventManager.Dispatch(LevelFlowEvents.LevelEnded);

        SetBubblesScales();
    }
    public void SetBubblesScales()
    {
        List<float> bubblesValues = new List<float>();
        foreach (var item in bubbles)    
            bubblesValues.Add(item.Value);            
        
        //First sort the values to find negative and positive numbers
        float[] sortedNumbers =  bubblesValues.ToArray();
        Array.Sort(sortedNumbers);

        int splitIndex = Array.FindIndex(sortedNumbers, x => x >= 0);

        //This means there is no positive numbers in the playfield. So the game ends
        if (splitIndex < 0)
        {
            EventManager.Dispatch(LevelFlowEvents.LevelEnded);
            return;
        }
        // Negative numbers
        float[] negativeNumbers = new float[splitIndex];    
            Array.Copy(sortedNumbers, negativeNumbers, splitIndex);

        // Positive or zero numbers
        float[] positiveNumbers = new float[sortedNumbers.Length - splitIndex];
        if(positiveNumbers!=null)
            Array.Copy(sortedNumbers, splitIndex, positiveNumbers, 0, sortedNumbers.Length - splitIndex);

        //If there is only one positive bubble in the playfield, check if it is the player's one.
        //If thats the case, dispatch the level ended event
        if(positiveNumbers.Length <= 1)
        {
            if (CheckIfBubbleIsPlayer(GetBubbleComponentByValue(positiveNumbers[0])))
            {
                EventManager.Dispatch(LevelFlowEvents.LevelEnded);
                return;
            }
        }
        //If by any chance, there is no negative or positive numbers, set them to 0, in order to avoid errors
        if (negativeNumbers.Length==0)
        negativeNumbers = new float[]{0};

        if (positiveNumbers.Length == 0)
        positiveNumbers = new float[] {0};

        //Then set scales according to the current bound values
        SetScales(positiveNumbers.Min(), positiveNumbers.Max(), negativeNumbers.Max(),negativeNumbers.Min());
    }

    private void SetScales(float minPositiveNumber, float maxPositiveNumber, float minNegativeNumber, float maxNegativeNumber)
    {
        foreach (var item in bubbles)
        {
            float scale = 0.0f;
            if (item.Value > 0)
            {
                if (item.Value == maxPositiveNumber) scale = maxPositiveCircleScale;
                else if (item.Value == minPositiveNumber) scale = minPositiveCircleScale;
                else scale = minPositiveCircleScale + (float)(item.Value - minPositiveNumber) / (maxPositiveNumber - minPositiveNumber) * (maxPositiveCircleScale - minPositiveCircleScale);
                //  scale = minPositiveCircleScale + (float)(item.Value - minPositiveNumber) / (maxPositiveNumber - minPositiveNumber) * (maxPositiveCircleScale - minPositiveCircleScale);
                // if(float.IsNaN(scale) || float.IsInfinity(scale))         
                //   scale = maxPositiveCircleScale;  
            }
            else if (item.Value == 0)
            {
                scale = (maxPositiveCircleScale + minPositiveCircleScale) / 2;
            }
            else
            {
                if (item.Value == maxNegativeNumber) scale = maxNegativeCircleScale;
                else if (item.Value == minNegativeNumber) scale = minNegativeCircleScale;
                else scale = minNegativeCircleScale + (float)(item.Value - minNegativeNumber) / (maxNegativeNumber - minNegativeNumber) * (maxNegativeCircleScale - minNegativeCircleScale);
                // scale = minNegativeCircleScale + (float)(item.Value - minNegativeNumber) / (maxNegativeNumber - minNegativeNumber) * (maxNegativeCircleScale - minNegativeCircleScale);
                /*  if (float.IsNaN(scale) || float.IsInfinity(scale))
                      scale = maxNegativeCircleScale;*/
            }
            item.transform.DOScale(new Vector3(scale, scale, 1), 0.5f);
        }
    }
    private BubbleBase GetBubbleComponentByValue(float value)
    {
        foreach (var item in bubbles)
        {
            if (item.Value == value)
                return item;
        }
        return null;
    }
    private bool CheckIfBubbleIsPlayer(BubbleBase bubble)
    {
        if (bubble.GetComponent<PlayerBubble>() != null)
            return true;
        else
            return false;
    }
}