using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using static Inventory;

public class InventoryController : MonoBehaviour, IUIController
{
    [SerializeField]
    private UIInventory inventoryUI;
    [SerializeField]
    private Inventory inventoryModel;
    public List<InventorySlot> initialItems = new List<InventorySlot>();
    private void Start()
    {
        InitializeUI();
        InitializeInventoryModel();
    }
    public bool Trigger()
    {
        if (!inventoryUI.isActiveAndEnabled)
        {
            inventoryUI.Show();
            foreach (var slot in inventoryModel.GetCurrentInventoryState())
            {
                inventoryUI.UpdateData(slot);
            }
            return true;
        }
        inventoryUI.Hide();
        return false;
    }
    private void InitializeUI()
    {
        inventoryUI.Initialize(inventoryModel.Size);
        inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        inventoryUI.OnSwapItems += HandleSwapItems;
        inventoryUI.OnStartDragging += HandleDragging;
        // inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }
    private void InitializeInventoryModel()
    {
        inventoryModel.Initialize();
        inventoryModel.OnInventoryUpdated += UpdateInventoryUI;
        foreach (InventorySlot slot in initialItems)
        {
            if (slot.IsEmpty)
                continue;
            inventoryModel.AddItem(slot);
        }
    }
    private void UpdateInventoryUI(List<InventorySlot> inventoryState)
    {
        Debug.Log("UpdateInventoryUI");
        inventoryUI.ResetAllItems();
        foreach (var slot in inventoryState)
            inventoryUI.UpdateData(slot);
    }
    /*
    private void HandleItemActionRequest(int itemIndex)
    {
        InventorySlot slot = inventoryModel.GetItemAt(itemIndex);
        if (slot.IsEmpty)
            return;

        IItemAction itemAction = slot.item as IItemAction;
        if (itemAction != null)
        {

            inventoryUI.ShowItemAction(itemIndex);
            inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
        }

        IDestroyableItem destroyableItem = slot.item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryUI.AddAction("Drop", () => DropItem(itemIndex, slot.quantity));
        }

    }
    */
    /*
    private void DropItem(int itemIndex, int quantity)
    {
        inventoryModel.RemoveItem(itemIndex, quantity);
        inventoryUI.ResetSelection();
    }
    */
    /*
    public void PerformAction(int itemIndex)
    {
        InventorySlot slot = slot.GetItemAt(itemIndex);
        if (slot.IsEmpty)
            return;

        IDestroyableItem destroyableItem = slot.item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryModel.RemoveItem(itemIndex, 1);
        }

        IItemAction itemAction = slot.item as IItemAction;
        if (itemAction != null)
        {
            itemAction.PerformAction(gameObject, slot.itemState);
            if (inventoryModel.GetItemAt(itemIndex).IsEmpty)
                inventoryUI.ResetSelection();
        }
    }
    */
    private void HandleDragging(int itemIndex)
    {
        InventorySlot slot = inventoryModel.GetSlotAt(itemIndex);
        if (slot.IsEmpty)
            return;
        inventoryUI.CreateDraggedItem(slot.item.ItemIcon, slot.quantity);
    }
    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        Item item = inventoryModel.GetSlotAt(itemIndex_1).item;
        if (item == null)
            return;
        switch (itemIndex_2)
        {
            case 0:
                if (!item.Type().Equals("Helmet"))
                    return;
                break;
            case 1:
                if (!item.Type().Equals("Chestplate"))
                    return;
                break;
            case 2:
                if (!item.Type().Equals("Leggins"))
                    return;
                break;
            case 3:
                if (!item.Type().Equals("Boots"))
                    return;
                break;
            case 4:
                if (!item.Type().Equals("Weapon"))
                    return;
                break;
            case 5:
                if (!item.Type().Equals("Potion"))
                    return;
                break;
            case 6:
                if (!item.Type().Equals("Potion"))
                    return;
                break;
            default:
                break;
        }
        inventoryModel.SwapItems(itemIndex_1, itemIndex_2);
    }
    private void HandleDescriptionRequest(int itemIndex)
    {
        Debug.Log("HandleDescriptionRequest");
        InventorySlot slot = inventoryModel.GetSlotAt(itemIndex);
        Item item = slot.item;
        if (item == null)
            return;
        inventoryUI.UpdateDescription(item.Description);
    }
}
