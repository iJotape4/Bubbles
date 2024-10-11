using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Events;
using CandyCoded.HapticFeedback;

public class NumberBubble : BubbleBase 
{ 
    public static string _ItemTag = "NumberBubble";
    [Header("Sprites info")]
    [SerializeField] BubbleSpritesScriptableObject sprites;

    [Header("Collider Parameters")]
    [SerializeField] CircleCollider2D circleCollider;
    const float positiveColliderRadius = 2.56f;

    [Header("Negative Bubble Parameters")]
    [SerializeField] GameObject magneticVFX;
    const float negativeColliderRadius = 3.5f;

    [SerializeField] protected TextMeshProUGUI textComponent;
    //protected DragView dragView;
    public DragView dragView;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dragView = GetComponent<DragView>();
    }
    protected void Start()
    {
        dragView.onBeginDrag += () => canMerge = true;
        dragView.onBeginDrag += () => isDragging = true;
        dragView.onEndDrag += () => canMerge = false;
        dragView.onEndDrag += () => isDragging = false;
    }

    public override void Init(int value)
    {
        ItemTag = NumberBubble._ItemTag;
        Value = value;
        name = value.ToString();

        if (Value > 0)
        {
            itemSprite = sprites.PositiveBubbleSprite;
            SetColliderSize(positiveColliderRadius, false);
        }
        else if (Value < 0)
        {
            itemSprite = sprites.NegativeBubbleSprite;
            SetColliderSize(negativeColliderRadius, true);
        }
        else
        {
            itemSprite = sprites.NeutralBubbleSprite;
            SetColliderSize(positiveColliderRadius, false);
        }

        GetComponent<SpriteRenderer>().sprite = itemSprite;
        textComponent.text = Value.ToString();
    }

    private void SetColliderSize(float radius, bool VFXActive)
    {
        circleCollider.radius = radius;
        magneticVFX.SetActive(VFXActive);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out MovingBubble movingBubble))
        {
            canMerge = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MovingBubble>())
        {
            canMerge = true;
        }
    }

    //Merge conditions for Gravity well
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GravityWell>())
        {
            canMerge = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GravityWell>())
        {
            canMerge = false;
            rb.velocity = Vector2.zero;
        }
        if (collision.gameObject.GetComponent<MirrorBubble>())
        {
            dragView.onEndDrag += () => canMerge = true;
            canMerge = true;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
         if (!canMerge) return;

        if (collision.gameObject.CompareTag(_ItemTag))
        {
            BubbleBase otherBubble =  collision.gameObject.GetComponent<BubbleBase>();
            var newValue = otherBubble.Value + this.Value;

            if (collision.gameObject.TryGetComponent(out PlayerBubble playerBubble))
            {
                playerBubble.dragView.DisableDrag();
                playerBubble.Init(newValue);
                playerBubble.dragView.wasMergedInThisMovement = true;
                EventManager.Dispatch(InGameEvents.NewScore, newValue);
                EventManager.Dispatch(SongsEvents.PlaySFX, songs.BubbleMerge);
                this.DestroyBubble();
            }
            else if (collision.gameObject.TryGetComponent(out NumberBubble numberBubble))
            {
                //Set the drag to false  in order to obligate the player to raise the finger before dragging again
                this.dragView.DisableDrag();
                if (this.Value < 0 || otherBubble.Value < 0) { 
                    EventManager.Dispatch(InGameEvents.MovementFinished);
                    EventManager.Dispatch(SongsEvents.PlaySFX, songs.NegativeMerge);
                }
                else
                {
                    EventManager.Dispatch(SongsEvents.PlaySFX, songs.BubbleMerge);
                }
                this.Init(newValue);
                this.dragView.wasMergedInThisMovement = true;
              
                numberBubble.DestroyBubble();
                //The other bubbles can merge between them. But it will cost two moves
              
                EventManager.Dispatch(InGameEvents.MovementFinished);
            }
           
        }
        else if (collision.gameObject.TryGetComponent(out StaticBubble staticBubble))
        {
            //Set the drag to false  in order to obligate the player to raise the finger before dragging again
            this.dragView.DisableDrag();
            this.dragView.wasMergedInThisMovement = true;
            //The other bubbles can merge between them. But it will cost two moves
            EventManager.Dispatch(InGameEvents.MovementFinished);
            EventManager.Dispatch(SongsEvents.PlaySFX, songs.BlockedObstacle);
            HapticFeedback.LightFeedback();
        }

    }
    public void DestroyBubble() { 
        circleCollider.enabled = false;
        EventManager.Dispatch(InGameEvents.BubbleMerged, this);
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
    }
    // Update is called once per frame

    //void Update()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);
    //        Debug.Log(touch.phase + " --- "  + isDragging);
    //        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
    //        touchPosition.z = 0; // Ensure the touch position is in the same plane as the sprite

    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            // Check if the touch begins over the sprite
    //            var collider = GetComponent<Collider2D>();

    //            if (collider == Physics2D.OverlapPoint(touchPosition))
    //            {

    //                isDragging = true;
    //                canMerge = true;

    //                offset = transform.position - touchPosition;
    //            }

    //        }
    //        else if (touch.phase == TouchPhase.Moved && isDragging)
    //        {
    //            Vector3 newPosition = touchPosition + offset;
    //            // Move the Rigidbody2D to the touch position
    //            rb.MovePosition(touchPosition);
    //        }
    //        else if (touch.phase == TouchPhase.Ended)
    //        {
    //            // Stop dragging
    //            isDragging = false;
    //            canMerge = false;


    //        }
    //    }

    //}
    /*
    public void SpawnNewBubbles(int newValue, Vector2 midpoint)
    {
        List<LevelItem> items = new List<LevelItem>();
        LevelItem newBubble = new LevelItem();
        newBubble.ItemValue = newValue;
        newBubble.LevelItemType = ItemType.NumberBubble;
        newBubble.InitialPosition = midpoint;
        newBubble.InitialForce = new Vector2(1, 1);
        items.Add(newBubble);

        for (int i = 0; i < 3; i++)
        {
            LevelItem bubbleNegative = new LevelItem();
            bubbleNegative.LevelItemType = ItemType.NumberBubble;
            bubbleNegative.ItemValue = Random.Range(1, 10);
            bubbleNegative.InitialForce = new Vector2(1, 1);

            items.Add(bubbleNegative);
        }

        for (int i = 0; i < 3; i++)
        {
            LevelItem bubblePositive = new LevelItem();
            bubblePositive.LevelItemType = ItemType.NumberBubble;
            bubblePositive.ItemValue = Random.Range(-10, -1);
            bubblePositive.InitialForce = new Vector2(1, 1);

            items.Add(bubblePositive);
        }

        for (int i = 0; i < 2; i++)
        {
            LevelItem bubbleSpecial = new LevelItem();

            bubbleSpecial.ItemValue = Random.Range(1, 3);
            bubbleSpecial.InitialForce = new Vector2(1, 1);
            bubbleSpecial.LevelItemType = ItemType.SpecialBubble;

            items.Add(bubbleSpecial);
        }
        gameScript.CreateItems(items);
    }

    void PushOthers()
    {
        // Define the center of the area (e.g., the position of the GameObject this script is attached to)
        Vector2 center = transform.position;
        float diameter = transform.localScale.x; // Or .size.y, they should be the same for a perfect circle

        // Find all colliders within the specified radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, diameter * 0.5f);

        // Apply a force to each object within the area
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Calculate direction from the center of the effect to the object
                Vector2 direction = rb.transform.position - transform.position;
                direction.Normalize();

                // Apply force to the object
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }

    public void PushOthersInCircle(Vector2 center)
    {
        // Define the center of the area (e.g., the position of the GameObject this script is attached to)

        float diameter = transform.localScale.x;
        // Find all colliders within the specified radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, diameter * 0.5f);

        // Apply a force to each object within the area
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Calculate direction from the center of the effect to the object
                Vector2 direction = rb.transform.position - transform.position;
                direction.Normalize();

                // Apply force to the object
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }
    */

}