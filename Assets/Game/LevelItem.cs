using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelItem
{
    public ItemType LevelItemType;
    public Vector2? InitialPosition;
    public Vector2 InitialForce;
    public int ItemValue;
    public PhysicsMaterial2D PhysicsMaterial;
    public string ItemTag;
}
