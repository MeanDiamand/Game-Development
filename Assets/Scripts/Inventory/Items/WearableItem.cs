using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

    [SerializeField]
    [JsonProperty]
    private string overlayedSpritesString;

    private HashSet<int> overlayedSpritesConverter()
    {
        if (overlayedSpritesString == null) return null;

        HashSet<int> overlayedSpritesIds = new HashSet<int>();

        string[] rangeArray = overlayedSpritesString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string range in rangeArray)
        {
            // Split the range by dash to get the start and end values
            string[] bounds = range.Split('-');
            int start = int.Parse(bounds[0].Trim());
            int end = int.Parse(bounds[1].Trim());

            Debug.Log($"overlayedSpritesConverter() {range} {start} {end}");

            // Add all values in the range to the dictionary
            for (int i = start; i <= end; i++)
            {
                overlayedSpritesIds.Add(i);
            }
        }

        return overlayedSpritesIds;
    }

    [OnSerializing]
    internal void OnSerializingWearableItem(StreamingContext context)
    {
        if (sprite != null && sprite.Length > 0)
        {
            spriteName = sprite[0].name; // Accessing the first element of 'sprite' array
        }
        else
        {
            Debug.LogWarning("Sprite array is null or empty in OnSerializingWearableItem");
        }
    }

    [OnDeserialized]
    internal void OnDeserializedWearableItem(StreamingContext context)
    {
        sprite = SkinChanger.FindSpriteSheetByFullName(spriteName);
        if (sprite == null || sprite.Length < 1) { Debug.Log("WearableItem Sprite {spriteName} Not Found"); }
    }

    public override SpritesContainer GetSprite()
    {
        return new SpritesContainer(sprite, overlayedSpritesConverter());
    }
}
