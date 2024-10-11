using UnityEngine;
using CandyCoded.HapticFeedback;

public class SpecialBubble : BubbleBase
{
    public float radius = 0;// Adjust this to set the radius within which objects will be destroyed
    void OnMouseDown()
    {
        DestroyObjectsAround();
    }
    void DestroyObjectsAround()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            // Add any specific conditions here, for example, checking tags
            Destroy(hitCollider.gameObject);
        }
        HapticFeedback.HeavyFeedback();
    }
    // Start is called before the first frame update
    public override void Init(int value)
    {
        radius = Random.Range(0.2f, 1.0f);
        ItemTag = "SpecialBubble";
        Value = value;
        PhysicsMaterial = new PhysicsMaterial2D("Bounce2d");
        itemSprite = Resources.Load<Sprite>("bubble3_4");
        name = value.ToString();
    }
}