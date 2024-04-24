using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : WearableItem
{
    [SerializeField]
    private float defenceAmount;

    public override float GetDefenceAmount()
    {
        return defenceAmount;
    }
}
