using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
public class SwapBubble : StaticBubble
{
    [SerializeField] protected TextMeshProUGUI txValue;
    const float positiveColliderRadius = 2.56f;
    const float negativeColliderRadius = 3.5f;
    protected DragView dragView;
    public override void Init(int value)
    {
        Value = value;
        name = value.ToString();
        txValue.text = value.ToString();
        if (Value > 0)
        {
            SetColliderSize(positiveColliderRadius);
        }
        else if (Value < 0)
        {
            SetColliderSize(negativeColliderRadius);
        }
    }
    public override void setData(ObstacleData data)
    {
        Init(data.value);
    }
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
    private void SetColliderSize(float radius)
    {
        GetComponent<CircleCollider2D>().radius = radius;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.TryGetComponent(out NumberBubble numberBubble))
        {
           
            if (!(numberBubble.isDragging||isDragging)) { return; }
            numberBubble.Init(Value);
            this.dragView.DisableDrag();
            this.dragView.wasMergedInThisMovement = true;
            EventManager.Dispatch(InGameEvents.UpdateBubbleScales);
            EventManager.Dispatch(InGameEvents.MovementFinished);
            EventManager.Dispatch(SongsEvents.PlaySFX, songs.BubbleMerge);
            if (collision.gameObject.GetComponent<PlayerBubble>())EventManager.Dispatch(InGameEvents.NewScore, Value);
            DestroyBubble();
        }
    }
    public void DestroyBubble()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
    }
}
