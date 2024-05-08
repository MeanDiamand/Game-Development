using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Inventory;

public class InventoryController : MonoBehaviour, IUIController
{
    [SerializeField]
    private UIInventory inventoryUI;
    [SerializeField]
    private Inventory inventoryModel;
    public event Action OnInventoryUpdated;

    private void Awake()
    {
        InitializeInventoryModel();
    }

    private void Start()
    {
        InitializeUI();     
    }

    private void OnDestroy()
    {
        inventoryModel.OnInventoryUpdated -= UpdateInventoryUI;
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
    }
    private void InitializeInventoryModel()
    {
        inventoryModel.Initialize();
        inventoryModel.OnInventoryUpdated += UpdateInventoryUI;
    }
    private void UpdateInventoryUI(List<InventorySlot> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var slot in inventoryState)
            inventoryUI.UpdateData(slot);
        OnInventoryUpdated?.Invoke();
    }

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
        string itemType = item.Type();
        switch (itemIndex_2)
        {
            case 0:
                if (!itemType.Equals("Helmet"))
                    return;
                break;
            case 1:
                if (!itemType.Equals("Chestplate"))
                    return;
                break;
            case 2:
                if (!itemType.Equals("Leggins"))
                    return;
                break;
            case 3:
                if (!itemType.Equals("Boots"))
                    return;
                break;
            case 4:
                if (!itemType.Equals("Weapon"))
                    return;
                break;
            case 5:
                if (!itemType.Equals("Shield"))
                    return;
                break;
            case 6:
                if (!itemType.Equals("Potion"))
                    return;
                break;
            default:
                break;
        }
        inventoryModel.SwapItems(itemIndex_1, itemIndex_2);
    }
    private void HandleDescriptionRequest(int itemIndex)
    {
        InventorySlot slot = inventoryModel.GetSlotAt(itemIndex);
        Item item = slot.item;
        if (item == null)
            return;
        inventoryUI.UpdateDescription(item.Description);
    }
}
