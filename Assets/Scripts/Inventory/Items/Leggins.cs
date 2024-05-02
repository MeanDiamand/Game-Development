using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Leggins : Armour
{
    public override string Type()
    {
        return "Leggins";
    }
}