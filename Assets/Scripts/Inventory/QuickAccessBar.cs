using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAccessBar : MonoBehaviour
{
    [SerializeField]
    private List<UIInventorySlot> slots;
    [SerializeField]
    private Inventory inventory;
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);

        Upd();
    }

    public void Upd()
    {
        for (int i = 0; i < 2; i++)
        {
            Inventory.InventorySlot slot = inventory.GetSlotAt(5 + i);
            if (slot.IsEmpty)
                slots[i].ResetData();
            else
                slots[i].SetData(slot.item.ItemIcon, slot.quantity);
        }
    }

    public void UseQ()
    {
        inventory.UsePotion(0);
    }

    public void UseE()
    {
        inventory.UsePotion(1);
    }
}
