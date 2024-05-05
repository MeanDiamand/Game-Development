using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class WearableItem : Item
{
    [JsonProperty]
    private string spriteName;

    [SerializeField]
    [JsonIgnore]
    protected Sprite[] sprite;

    [OnSerializing]
    internal void OnSerializingWearableItem(StreamingContext context)
    {
        if (sprite != null && sprite.Length > 0)
        {
            spriteName = sprite[0].name; // Accessing the first element of 'sprite' array
        }
        else
        {
            // Handle the case where 'sprite' array is null or empty
            Debug.LogWarning("Sprite array is null or empty in OnSerializingWearableItem");
        }
    }

    [OnDeserialized]
    internal void OnDeserializedWearableItem(StreamingContext context)
    {
        sprite = SkinChanger.FindSpriteSheetByFullName(spriteName);
        if (sprite == null || sprite.Length < 1) { Debug.Log("WearableItem Sprite {spriteName} Not Found"); }
    }

    public override Sprite[] GetSprite()
    {
        return sprite;
    }
}
