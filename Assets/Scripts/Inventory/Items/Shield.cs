using System;
using UnityEngine;

[CreateAssetMenu]
public class Shield : WearableItem
{
    public override string Type()
    {
        return "Shield";
    }

    // IF Q/E is pressed and shield is in QA slots, than this method is called
    public override int Use()
    {
        Debug.Log("Shield used");
        PlayerEvents.GetInstance().ShieldUse(true);

        return 0; // not to decrease quantity
    }
}
