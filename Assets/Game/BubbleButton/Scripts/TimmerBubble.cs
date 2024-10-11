using DG.Tweening;
using Events;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimmerBubble : StaticBubble,Obstacles
{
    [SerializeField] protected TextMeshProUGUI txValue;
    [SerializeField] GameObject explodeBomb;
    protected int time;
    float DestroyRadius;
    public override void setData(ObstacleData data)
    {
        Init(data.value);
        time = data.time;
        txValue.text = time.ToString();
        DestroyRadius = data.radius;
        StartCoroutine(timmer(time));
    }
    IEnumerator timmer(int time)
    {
        yield return (new WaitForSeconds(1f));
        time--;
        txValue.text = time.ToString();
        if (time > 0) StartCoroutine(timmer(time));
        else Change();
    }
    void Change()
    {
        int change = Random.Range(0,2);
        BubblesManager bubblesManager = FindObjectOfType<BubblesManager>();
        if (change == 0)
        {
            BubbleData bubbleData= new BubbleData();
            bubbleData.bubbleType = BubbleType.Positive;
            bubbleData.bubbleNumber = Value;
            bubbleData.radius = 1;
            bubblesManager.CreateBubble(bubbleData,transform.position);
        }
        else
        {
           GameObject bomb= Instantiate(explodeBomb,transform.position,transform.rotation);
            bomb.GetComponent<DestroyBomb>().startDestroyBomb(DestroyRadius);
        }
        EventManager.Dispatch(InGameEvents.UpdateBubbleScales);
        EventManager.Dispatch(SongsEvents.PlaySFX, songs.Spawn);
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
    }
}
