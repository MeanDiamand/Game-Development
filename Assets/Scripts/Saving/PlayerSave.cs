using Newtonsoft.Json;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class PlayerSave
{
    [JsonProperty]
    public Inventory inventory;
    [JsonProperty]
    public PlayerCharacteristics characteristics;
    [JsonProperty]
    public float posX;
    [JsonProperty]
    public float posY;
    [JsonProperty]
    public float health;
}
