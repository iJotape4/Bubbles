using DG.Tweening;
using UnityEngine;


public class ColorSwitchBubble : MonoBehaviour, Obstacles
{
    [SerializeField] private Vector2 Position;
    [SerializeField] CircleCollider2D circleCollider;

    protected DragView dragView;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out NumberBubble numberBubble))
        {
            //switch color and number
            numberBubble.Init(-numberBubble.Value);
            // Disable drag
            numberBubble.dragView.DisableDrag();
           
            //destroy bubble
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => Destroy(gameObject));
            circleCollider.enabled = false;
        }
        
    }

    public void setData(ObstacleData data)
    {
    }
}
