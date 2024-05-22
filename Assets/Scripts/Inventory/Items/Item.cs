using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int ID => GetInstanceID();

    [field: SerializeField]
    [JsonProperty]
    public string Name { get; set; }

    [field: SerializeField]
    [JsonProperty]
    public float Weight { get; set; }

    [field: SerializeField]
    [JsonProperty]
    public bool IsStackable { get; set; }

    [field: SerializeField]
    [JsonProperty]
    public int MaxStackSize { get; set; }

    [field: SerializeField]
    [field: TextArea]
    [JsonProperty]
    public string Description { get; set; }


    [JsonProperty]
    private string itemIconName;
    [field: SerializeField]
    [JsonIgnore]
    public Sprite ItemIcon { get; set; }

    [OnSerializing]
    internal void OnSerializingItem(StreamingContext context)
    {
        itemIconName = ItemIcon.name;
    }
    [OnDeserialized]
    internal void OnDeserializedItem(StreamingContext context)
    {
        Debug.Log($"OnDeserializedItem {Name}");
        ItemIcon = SkinChanger.FullNameToSprite(itemIconName);
    }
    public override string ToString()
    {
        return Name;
    }
    public virtual Damage dealDamage()
    {
        return null;
    }
    public virtual int Use()
    {
        Debug.LogError("Use() not implemented in item");
        throw new Exception("Use() not implemented in item");
    }
    public virtual string Type()
    {
        return "Item";
    }
    public virtual SpritesContainer GetSprite()
    {
        return null;
    }

    public virtual float GetDefenceAmount()
    {
        throw new Exception("GetDefenceAmount() not implemented in item");
    }
}
