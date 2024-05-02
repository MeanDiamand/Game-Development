using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Boots : Armour
{
    public override string Type()
    {
        return "Boots";
    }
}
