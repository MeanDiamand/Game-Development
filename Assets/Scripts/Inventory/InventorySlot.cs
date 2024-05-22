using Newtonsoft.Json;
using System;

[Serializable]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public struct InventorySlot
{
    [JsonProperty]
    public int index;
    [JsonProperty]
    public int quantity;
    [JsonProperty]
    public Item item;
    public bool IsEmpty => item == null;

    public InventorySlot ChangeQuantity(int newQuantity)
    {
        return new InventorySlot
        {
            index = this.index,
            quantity = newQuantity,
            item = this.item
        };
    }
    /// <summary>
    /// Shorthand for writing InventorySlot(null, 0)
    /// </summary>
    public static InventorySlot GetEmpty(int i)
        => new InventorySlot
        {
            item = null,
            quantity = 0,
            index = i
        };

    public override string ToString()
    {
        if (item == null)
            return "none";
        return item.ToString() + " " + index;
    }
}
