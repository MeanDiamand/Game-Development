using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Weapon : WearableItem
{
    [field: SerializeField]
    [JsonProperty]
    public Damage damage { get; private set; }
    
    public override Damage dealDamage()
    {
        Debug.Log("Weapon");
        return damage;
    }

    public override string Type()
    {
        return "Weapon";
    }
}