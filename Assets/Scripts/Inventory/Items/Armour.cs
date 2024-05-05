using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Armour : WearableItem
{
    [SerializeField]
    [JsonProperty]
    private float defenceAmount;

    public override float GetDefenceAmount()
    {
        return defenceAmount;
    }
}
