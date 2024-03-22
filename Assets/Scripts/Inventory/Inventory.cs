using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Progress;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<InventorySlot> slots;
    private Weapon weapon;
    [field: SerializeField]
    public int Size { get; private set; } = 10;
    public event Action<List<InventorySlot>> OnInventoryUpdated;

    public void Initialize()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < Size + 7; i++)
        {
            slots.Add(InventorySlot.GetEmpty(i));
        }
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
    }
    public InventorySlot GetSlotAt(int itemIndex)
    {
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
        Item item = slots[5 + id].item;
        if (item == null) return;   
        item.Use();
    }


    [Serializable]
    public struct InventorySlot
    {
        public int index;
        public int quantity;
        public Item item;
        public bool IsEmpty => item == null;

        public InventorySlot ChangeQuantity(int newQuantity)
        {
            return new InventorySlot
            {
                item = this.item,
                quantity = newQuantity
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
