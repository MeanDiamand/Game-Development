using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Chestplate : Armour
{
    public override string Type()
    {
        return "Chestplate";
    }
}
