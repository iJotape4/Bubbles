using CandyCoded.HapticFeedback;
using Events;
using UnityEngine;

public class PlayerBubble : NumberBubble
{    public override void Init(int value)
    {
        Value = value;
        textComponent.text = Value.ToString();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (!canMerge) return;
        if (collision.gameObject.CompareTag(_ItemTag)) 
        {
            //Set the drag to false  in order to obligate the player to raise the finger before dragging again
            dragView.DisableDrag();
            NumberBubble bubbleOther = collision.gameObject.GetComponent<NumberBubble>();
            if (bubbleOther.Value < 0) { EventManager.Dispatch(InGameEvents.MovementFinished); 
                EventManager.Dispatch(SongsEvents.PlaySFX, songs.NegativeMerge);
            }
            else
            {
                EventManager.Dispatch(SongsEvents.PlaySFX, songs.BubbleMerge);
            }
            var newValue = bubbleOther.Value + this.Value;
            Init(newValue);
            this.dragView.wasMergedInThisMovement = true;
            EventManager.Dispatch(InGameEvents.NewScore, newValue);
            bubbleOther.DestroyBubble();
            HapticFeedback.LightFeedback();
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
}