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
        spriteName = sprite[0].name;
    }

    [OnDeserialized]
    internal void OnDeserializedWearableItem(StreamingContext context)
    {
        sprite = SkinChanger.FindSpriteSheetByFullName(spriteName);
    }

    public override Sprite[] GetSprite()
    {
        return sprite;
    }
}
