using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearableItem : Item
{
    [SerializeField]
    protected Sprite[] sprite;

    public override Sprite[] GetSprite()
    {
        return sprite;
    }
}
