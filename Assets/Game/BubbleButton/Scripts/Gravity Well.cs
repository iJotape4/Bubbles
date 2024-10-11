using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GravityWell : MonoBehaviour, Obstacles
{
    [SerializeField] float attractionForce = 30f,duration,timeToActive;
    CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;
    private bool isActive = false;
    [SerializeField] GameObject areaEffect;
    [SerializeField] GameObject alert;

    Vector2 position;
    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Active());
        StartCoroutine(rotate());

        alert.transform.localScale = Vector3.one * 0.9f;

        // Expand and collapse animation
        alert.transform.DOScale(2f, 1f)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D rbOther))
        {

            //Here we get the vec from gravity source to the bubble to be pulled
            Vector2 direction = transform.position - collision.transform.position;
            float distance = direction.magnitude;
            float radius = circleCollider.radius;

            // To give a realistic effect, the force is inversely proportional to the square of the distance.
            if (distance < radius && distance > radius/13)
            {
                float forceMagnitude = 3.7f * attractionForce/(distance * distance);
                Vector2 force = direction.normalized * forceMagnitude;
                rbOther.AddForce(force);
            }
            if(distance < radius / 13)
            {
                //However, when the bubble is too close and the distance is less than 1, the force would be too huge.
                float forceMagnitude = 1.1f * attractionForce;
                Vector2 force = direction.normalized * forceMagnitude;
                rbOther.AddForce(force);
            }
        }
    }

    IEnumerator Active()
    {
        yield return new WaitForSeconds(0.2f);      
        Alert();

        yield return new WaitForSeconds(timeToActive);
        Estate();
        alert.SetActive(false);

        //this moment to activate the collider is to prevent gravity well to rush on its operation.
        circleCollider.enabled = true;
        alert.transform.DOScale(Vector3.zero, 0.5f);

        transform.DOScale(transform.localScale, 0f);
        transform.position = position;
        transform.DOScale(transform.localScale, 0.1f);
        yield return new WaitForSeconds(duration);
        Estate();
        StartCoroutine(Active());
    }
    IEnumerator rotate()
    {
        Vector3 angles = areaEffect.transform.eulerAngles;
        Vector3 targetAngle = angles + Vector3.forward * 45;
        areaEffect.transform.DORotate(targetAngle, 5f, RotateMode.Fast);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(rotate());
    }

    private void Estate()
    {
        if (isActive)
        {
            circleCollider.enabled = false;
            spriteRenderer.enabled = false;
            areaEffect.SetActive(false);
            isActive=!isActive;
            
        }
        else
        {
            circleCollider.enabled = true;
            spriteRenderer.enabled = true;
            areaEffect.SetActive(true);
            isActive = !isActive;
        }
    }

    private void Alert()
    {
        // The structure of this random gen is to avoid the settings button.
        float randY = Random.Range(-4.4f, 3f);
        float randX;
        if(randY < -3.5f)
        {
            randX = Random.Range(-2f, 1f);
        }
        else
        {
            randX = Random.Range(-2, 2);
        }
        position = new Vector2(randX, randY);
        transform.DOMove( new Vector3(position.x, position.y, 0), 1f);
        alert.SetActive(true);
    }

    public void setData(ObstacleData data)
    {
    }
}
