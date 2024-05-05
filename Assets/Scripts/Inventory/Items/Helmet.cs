using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Helmet : Armour
{
    public override string Type()
    {
        return "Helmet";
    }
}
