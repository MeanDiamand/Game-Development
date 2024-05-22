using Newtonsoft.Json;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class SceneSave
{
    [JsonProperty]
    public bool[] alive;

    [JsonProperty]
    public float[] heath;

    [JsonProperty]
    public int[] indixes;

    [JsonProperty]
    public float[] xCords;

    [JsonProperty]
    public float[] yCords;

    [JsonProperty]
    public bool savePlayerPos;
    [JsonProperty]
    public float playerX;
    [JsonProperty]
    public float playerY;
}