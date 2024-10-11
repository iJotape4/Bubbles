using UnityEngine;

public class EmptyBubble : BubbleBase
{

    // Start is called before the first frame update
    public override void Init(int value)
    {
        ItemTag = "EmptyBubble";
        Value = value;
        PhysicsMaterial = new PhysicsMaterial2D("Bounce2d");
        itemSprite = Resources.Load<Sprite>("bubble3_16");
        name = value.ToString();
    }
}
