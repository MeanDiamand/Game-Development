using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class Inventory : ScriptableObject
{
    [SerializeField]
    [JsonProperty]
    private List<InventorySlot> slots;

    [JsonProperty]
    private Weapon weapon;
    
    [field: SerializeField]
    [JsonProperty]
    public int Size { get; private set; }
    public event Action<List<InventorySlot>> OnInventoryUpdated;

    public void Clone(Inventory inventoryToClone)
    {
        if (inventoryToClone == null)
        {
            Debug.LogError("Trying to copy null inventory");
            return;
        }
        slots = new List<InventorySlot>();
        foreach (var slot in inventoryToClone.slots)
        {
            slots.Add(new InventorySlot
            {
                index = slot.index,
                quantity = slot.quantity,
                item = slot.item != null ? Instantiate(slot.item) : null // Deep clone items if not null
            });
        }

        for (int i = 0; i < 7; i++)
            PlayerEvents.GetInstance().ArmourChanged((slots[i].item != null) ? slots[i].item.GetSprite() : null, i);

        PlayerEvents.GetInstance().WeaponChanged((slots[4].item != null) ? slots[4].item.GetSprite() : null);

        weapon = inventoryToClone.weapon != null ? Instantiate(inventoryToClone.weapon) : null;
        Size = inventoryToClone.Size;
    }

    public void Initialize()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < Size + 7; i++)
        {
            slots.Add(InventorySlot.GetEmpty(i));
        }

        Size = 10;

        //for (int i = 0; i < 7; i++)
        //    PlayerEvents.GetInstance().ArmourChanged(null, i);
        //PlayerEvents.GetInstance().WeaponChanged(null);
    }

    public Weapon.Damage GetDamage()
    {
        if (slots[4].IsEmpty)
            return Weapon.Damage.HandDamage();
        Debug.Log(slots[4].item.Name);
        return slots[4].item.dealDamage();
    }

    public void AddItem(Item item, int quantity)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i] = new InventorySlot
                {
                    index = i,
                    item = item,
                    quantity = quantity
                };
                return;
            }
        }
    }
    public void AddItemToMain(Item item, int quantity)
    {
        for (int i = 7; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i] = new InventorySlot
                {
                    index = i,
                    item = item,
                    quantity = quantity
                };
                return;
            }
        }
    }
    public void AddItem(InventorySlot slot)
    {
        AddItem(slot.item, slot.quantity);
    }

    private void PutSlotInSlot(InventorySlot slot, int index)
    {
        slot.index = index;
        slots[index] = slot;

        SpritesContainer sprites = null;
        if (!slot.IsEmpty)
            sprites = slot.item.GetSprite();
        if (index >= 0 && index <= 3)
            PlayerEvents.GetInstance().ArmourChanged(sprites, index);
        if (index == 4)
            PlayerEvents.GetInstance().WeaponChanged(sprites);
        if (index >= 5 && index <= 6)
            PlayerEvents.GetInstance().ArmourChanged(sprites, index);
    }
    public InventorySlot GetSlotAt(int itemIndex)
    {
        if (itemIndex >= slots.Count || itemIndex < 0)
        {
            Debug.LogWarning("Index out of range: " + itemIndex + " | " + slots.Count);
            return new InventorySlot();
        }
        return slots[itemIndex];
    }
    public void SwapItems(int itemIndex_1, int itemIndex_2)
    {
        if ((itemIndex_1 >= Size + 7) || (itemIndex_2>= Size + 7)) return;
        InventorySlot slot_1 = slots[itemIndex_1];
        //slots[itemIndex_1] = slots[itemIndex_2];
        PutSlotInSlot(slots[itemIndex_2], itemIndex_1);
        //slots[itemIndex_2] = slot_1;
        PutSlotInSlot(slot_1, itemIndex_2);
        NotifyInventoryUpdated();
    }
    private void NotifyInventoryUpdated()
    {
        weapon = (Weapon)slots[4].item;
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }

    public List<InventorySlot> GetCurrentInventoryState()
    {
        List<InventorySlot> activeSlots = new List<InventorySlot>();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
                continue;
            activeSlots.Add(slots[i]);
        }
        return activeSlots;
    }

    public void UsePotion(int id)
    {
        if (id > 1 || id < 0) return;

        InventorySlot slot = slots[5 + id];

        Item item = slot.item;
        if (slot.IsEmpty) return;
        if (slot.quantity <= 0) return;

        slots[5 + id] = slots[5 + id].ChangeQuantity(slot.quantity + item.Use()); // Use Item. Decrease quantity of it if needed
        if (slots[5 + id].quantity <= 0)
            slots[5 + id] = InventorySlot.GetEmpty(slots[5 + id].index);
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }


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
}
