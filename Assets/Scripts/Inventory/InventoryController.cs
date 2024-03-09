using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using static Inventory;

public class InventoryController : MonoBehaviour
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Show(); 
                foreach (var item in inventoryModel.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.index,
                            item.item.ItemIcon,
                            item.quantity);
                }
            }
            else
                inventoryUI.Hide();
        }
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
        {
            inventoryUI.UpdateData(slot.index, slot.item.ItemIcon,
                slot.quantity);
        }
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
        InventorySlot slot = inventoryModel.GetItemAt(itemIndex);
        if (slot.IsEmpty)
            return;
        inventoryUI.CreateDraggedItem(slot.item.ItemIcon, slot.quantity);
    }
    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        inventoryModel.SwapItems(itemIndex_1, itemIndex_2);
    }
    private void HandleDescriptionRequest(int itemIndex)
    {
        Debug.Log("HandleDescriptionRequest");
        InventorySlot slot = inventoryModel.GetItemAt(itemIndex);
        Item item = slot.item;
        inventoryUI.UpdateDescription(item.Description);
    }
}
