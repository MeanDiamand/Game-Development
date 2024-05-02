using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Potion : Item
{
    [SerializeField]
    [JsonProperty]
    private int healing;
    public override string Type()
    {
        return "Potion";
    }
    public override int Use()
    {
        Debug.Log("Healing");
        PlayerEvents.GetInstance().Heal(healing);
        return -1;
    }
}
