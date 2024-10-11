
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombBubble : StaticBubble, Obstacles
{
    [SerializeField] float radiusExplosion;
    [SerializeField] float speedExplosion;
    [SerializeField] TextMeshProUGUI txValue;
    CircleCollider2D colliderExplode;
    bool OnExplode;
    private void Start()
    {
        colliderExplode = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        if (OnExplode) { colliderExplode.radius = Mathf.MoveTowards(colliderExplode.radius, radiusExplosion, speedExplosion * Time.deltaTime);
            transform.GetChild(0).localScale = new Vector2((colliderExplode.radius * 2), colliderExplode.radius * 2);
        }
        if (colliderExplode.radius >= radiusExplosion) Destroy(gameObject, 1f);
    }
    void OnMouseDown()
    {
        activeReaction();
    }
    private void activeReaction()
    {
        EventManager.Dispatch(SongsEvents.PlaySFX, songs.BombExplode);
        OnExplode = true;
        colliderExplode.isTrigger = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out NumberBubble numberBubble)&& !collision.GetComponent<PlayerBubble>())
        {     
            numberBubble.Init(this.Value);
        }
        if (collision.TryGetComponent(out BombBubble bombBubble))
        {
            bombBubble.activeReaction();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out NumberBubble numberBubble))
        {
            if (!numberBubble.isDragging) { return; }
            activeReaction();
        }
    }
    public override void setData(ObstacleData data)
    {
        radiusExplosion = data.radius;
        Init(data.value);
        txValue.text = data.value.ToString();
    }

}
