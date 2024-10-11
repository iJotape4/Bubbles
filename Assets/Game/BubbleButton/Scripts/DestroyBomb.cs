using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DestroyBomb : MonoBehaviour
{
    [SerializeField] float radiusExplosion;
    [SerializeField] float speedExplosion;
    CircleCollider2D colliderExplode;
    bool OnExplode;
    private void Start()
    {
        colliderExplode = GetComponent<CircleCollider2D>();
    }
    public void startDestroyBomb(float radius)
    {
        EventManager.Dispatch(SongsEvents.PlaySFX, songs.BombExplode);
        radiusExplosion = radius;
        OnExplode = true;
    }
    private void Update()
    {
        if (OnExplode)
        {
            colliderExplode.radius = Mathf.MoveTowards(colliderExplode.radius, radiusExplosion, speedExplosion * Time.deltaTime);
            transform.GetChild(0).localScale = new Vector2((colliderExplode.radius * 2), colliderExplode.radius * 2);
        }
        if (colliderExplode.radius >= radiusExplosion) Destroy(gameObject, 1f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NumberBubble numberBubble) && !collision.GetComponent<PlayerBubble>())
        {
            EventManager.Dispatch(SongsEvents.PlaySFX, songs.breakBubble);
            numberBubble.DestroyBubble();
        }
    }
}
