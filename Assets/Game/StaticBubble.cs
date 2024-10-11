using MyBox;

public class StaticBubble : BubbleBase, Obstacles
{
    public bool isColisionable;
    public override void Init(int value)
    {
        this.Value = value;
    }
    public virtual void setData(ObstacleData data){ }
}
