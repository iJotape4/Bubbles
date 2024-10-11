using Events;
using MyBox;
using UnityEngine;

[RequireComponent(typeof(BubblesManager))]
[RequireComponent(typeof(LevelFlowManager))]
public class BubblesGrid : MonoBehaviour
{    
    [SerializeField] public LevelData levelData { get; private set; }
    [Header("Grid Settings")]
    [SerializeField, ReadOnly] private int width = 5;
    [SerializeField, ReadOnly] private int height = 10;
    [SerializeField, ReadOnly] private int YPadding = 150, XPadding = 200;
    [SerializeField, ReadOnly] public float cellSize;
    private Grid grid;

    BubblesManager bubblesManager;
    LevelFlowManager levelParameters;    

    private void Awake()
    {
        bubblesManager = GetComponent<BubblesManager>();
        levelParameters = GetComponent<LevelFlowManager>();
    }
   public void Start()
   {
        levelData = LevelSelectorManager.Instance.currentLevel;
        float screenWidth = Screen.width;
        cellSize = (screenWidth - XPadding) / width;
        grid = new Grid(width, height, cellSize, new Vector3(transform.position.x + XPadding, transform.position.y + YPadding, 0));
        SetUpBubbles();
        SetUpLevelParameters();     
   }

    private void SetUpBubbles()
    {
        if(levelData == null)
        {
            Debug.LogError("LevelData is null. Are you opening the level from level selector?");
            return;
        }
        foreach (BubbleData bubble in levelData.bubblesData)     
            bubblesManager.CreateBubble(bubble, grid.GetWorldPosition(bubble.position.x, bubble.position.y));
        foreach (ObstacleData obstacle in levelData.obstacles)
            bubblesManager.CreateObstacle(obstacle, grid.GetWorldPosition(obstacle.position.x, obstacle.position.y));

        EventManager.Dispatch(InGameEvents.UpdateBubbleScales);
    }
    private void SetUpLevelParameters()
    {
        levelParameters.SetMaxMovements(levelData.moveLimit);
        levelParameters.SetLevelGoal(levelData.levelNumber,levelData.targetGoal);
    }
}