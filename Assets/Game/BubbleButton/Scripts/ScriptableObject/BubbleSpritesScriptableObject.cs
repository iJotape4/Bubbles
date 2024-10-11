using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BubbleSpritesScriptableObject", order = 1)]
public class BubbleSpritesScriptableObject : ScriptableObject
{
    [SerializeField] public Sprite NegativeBubbleSprite;
    [SerializeField] public Sprite PositiveBubbleSprite;
    [SerializeField] public Sprite NeutralBubbleSprite;
    [SerializeField] public Sprite SwapBubbleSprite;
}