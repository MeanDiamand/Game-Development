using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private Inventory inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemPUS item = collision.GetComponent<ItemPUS>();
        if (item != null)
        {
            //int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            inventoryData.AddItem(item.InventoryItem, item.Quantity);
            //if (reminder == 0)
            //item.DestroyItem();
            //else
            //item.Quantity = reminder;
            item.DestroyItem();
        }
    }
}