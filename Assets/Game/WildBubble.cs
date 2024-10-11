using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;

public class WildBubble : BubbleBase
{
    [SerializeField] protected TextMeshProUGUI txValue;
    public override void Init(int value)
    {
        Value = value;
        name = value.ToString();
        txValue.text =  value.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.TryGetComponent(out PlayerBubble playerBubble))
        {
            var newValue = playerBubble.Value + this.Value;
            playerBubble.Init(newValue);
            EventManager.Dispatch(InGameEvents.NewScore, newValue);
            EventManager.Dispatch(InGameEvents.UpdateBubbleScales);
            EventManager.Dispatch(SongsEvents.PlaySFX, songs.BubbleMerge);
            this.DestroyBubble();
        }
        if (collision.TryGetComponent(out StaticBubble staticBubble) && staticBubble.isColisionable)
        {      
           GetComponent<Rigidbody2D>().velocity=Vector2.zero;
            this.DestroyBubble();
        }
    }
    public void DestroyBubble()
    {
        GetComponent<CircleCollider2D>().enabled = false;

        transform.DOScale(Vector3.zero, 0.8f).OnComplete(() => Destroy(gameObject));
    }
}
