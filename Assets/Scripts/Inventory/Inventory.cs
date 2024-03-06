using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<InventorySlot> slots;
    [field: SerializeField]
    public int Size { get; private set; } = 10;

    public void Initialize()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < Size + 5; i++)
        {
            slots.Add(InventorySlot.GetEmpty(i));
        }
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
            }
        }
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
