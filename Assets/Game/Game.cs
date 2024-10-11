using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Vector2 initialForce = new Vector2(100, 0);
    [SerializeField] GameObject numberBubblePrefab;
    [SerializeField] GameObject emptyBubblePrefab;
    [SerializeField] GameObject specialBubblePrefab;

    void Start()
    {
        CreateLevel();
    }

    void ResetLevel()
    {
        var allNumberItems = GameObject.FindObjectsOfType<BubbleBase>();

        foreach (var item in allNumberItems)
        {
            Destroy(item.gameObject);
        }
    }

    public void CreateLevel()
    {
        ResetLevel();
        Level level1 = new Level();
        level1.LevelNumber = 1;
        level1.MergeLimit = 5;
        level1.ObjectiveScore = 10;
        level1.Items = new List<LevelItem>();

        for (int i = 0; i < 4; i++)
        {
            LevelItem bubble1 = new LevelItem();

            bubble1.ItemValue = Random.Range(1, 4);
            bubble1.LevelItemType = ItemType.NumberBubble;
            //bubble1.InitialPosition = new Vector2(-1.6856f, 3.5156f);
            bubble1.InitialForce = new Vector2(1, 1);
            level1.Items.Add(bubble1);
        }

        for (int i = 0; i < 10; i++)
        {
            LevelItem bubbleNegative = new LevelItem();
            bubbleNegative.LevelItemType = ItemType.NumberBubble;
            bubbleNegative.ItemValue = Random.Range(-10, -3);
            bubbleNegative.InitialForce = new Vector2(1, 1);
            bubbleNegative.LevelItemType = ItemType.NumberBubble;


            level1.Items.Add(bubbleNegative);
        }

        //Not sure how the empty bubbles are going to be used
        //for (int i = 0; i < 800; i++)
        //{
        //    LevelItem bubbleEmpty = new LevelItem();

        //    bubbleEmpty.ItemValue = 2;
        //    bubbleEmpty.LevelItemType = ItemType.EmptyBubble;


        //    level1.Items.Add(bubbleEmpty);
        //}

        CreateItems(level1.Items);
    }

    public void CreateItems(List<LevelItem> items)
    {
        foreach (var item in items)
        {
            Vector2 screenPosition;
            Vector3 worldPosition;
            if (item.InitialPosition == null)
            {
                //TODO: this can be improved using a collider's bounds
                screenPosition = new Vector2(Random.Range(-1.75f, 1.85f), Random.Range(-4.25f, 3.5f));
                //worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
            }
            else
            {
                screenPosition = item.InitialPosition.Value;
                worldPosition = new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane);
            }

            Vector2 size = new Vector2(1, 1);
            Vector3 bubblePosition = new Vector3(screenPosition.x, screenPosition.y, 0);

            BubblesManager.Instance.CreateBubble(bubblePosition, item.InitialForce, item.ItemValue, item.LevelItemType);

        }
        BubblesManager.Instance.SetBubblesScales();
    }
}