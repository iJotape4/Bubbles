using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BubbleBase : MonoBehaviour, IBubble 
{
    [Header("Bubble Base Parameters")]
    public PhysicsMaterial2D PhysicsMaterial;
    public int Value;
    public Sprite itemSprite;
    public string ItemTag;
    public float force = 2f;
    public bool shouldPushOthers = false;
    
    internal Rigidbody2D rb;
    internal bool isDragging = false;
    internal Game gameScript;
    internal bool isTriggered = false;
    internal bool canMerge = false;

    public abstract void Init(int value);
   
}
