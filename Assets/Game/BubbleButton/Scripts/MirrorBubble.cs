using Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class MirrorBubble : MonoBehaviour, Obstacles
{
    [SerializeField] private Collider2D circleCollider;
    [SerializeField] private Collider2D rigidCollider;

    private void Start()
    {
        StartCoroutine(EnableMirrorBubble());

        rigidCollider.enabled = false;

        circleCollider.enabled = true;
    }

    IEnumerator EnableMirrorBubble()
    {
        yield return new WaitForSeconds(0.5f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out NumberBubble numberBubble))
        {
            //Lose drag on bubble and end movement
            numberBubble.dragView.DisableDrag();
            EventManager.Dispatch(InGameEvents.MovementFinished);

            //Bounce physics
            Vector2 direction = -(transform.position - collision.transform.position);
            float forceMagnitude = 1.3f;
            Vector2 force = new Vector2(direction.normalized.x, direction.normalized.y) * forceMagnitude;
            numberBubble.rb.AddForce(force, ForceMode2D.Impulse);

            //Bounce of mirror bubble effect
            gameObject.transform.DOScale(0.05f, 0.2f).SetEase(Ease.OutCubic);
            gameObject.transform.DOScale(0.4f, 0.2f).SetEase(Ease.OutCubic);
        }
    }

    public void setData(ObstacleData data)
    {
    }
}