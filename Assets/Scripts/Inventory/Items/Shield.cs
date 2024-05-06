using Newtonsoft.Json;
using System;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Shield : WearableItem
{
    public override string Type()
    {
        return "Shield";
    }

    // IF Q is pressed and shield is in Q slot, than this method is called
    public override int Use()
    {
        Debug.Log("Shield used");
        PlayerEvents.GetInstance().ShieldUse(true);

        return 0; // not to decrease quantity
    }
}
