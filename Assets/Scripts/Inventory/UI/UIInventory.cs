using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private MouseFollower mouseFollower;

    List<UIInventorySlot> slots = new List<UIInventorySlot>();

    [SerializeField]
    UIInventorySlot helmetSlot;
    [SerializeField]
    UIInventorySlot chestplateSlot;
    [SerializeField]
    UIInventorySlot legginsSlot;
    [SerializeField]
    UIInventorySlot bootsSlot;

    [SerializeField] 
    UIInventorySlot weaponSlot;
    List<UIInventorySlot> quickAccessSlots = new List<UIInventorySlot>();

    [SerializeField]
    private Sprite def;

    public event Action<int> OnDescriptionRequested,
                OnItemActionRequested,
                OnStartDragging;

    public event Action<int, int> OnSwapItems;

    private int currentlyDraggedItemIndex = -1;

    public void Initialize(int inventorySize)
    {
        slots.Add(helmetSlot);
        SlotAddListeners(helmetSlot);
        slots.Add(chestplateSlot);
        SlotAddListeners(chestplateSlot);
        slots.Add(legginsSlot);
        SlotAddListeners(legginsSlot);
        slots.Add(bootsSlot);
        SlotAddListeners(bootsSlot);
        slots.Add(weaponSlot);
        SlotAddListeners(weaponSlot);

        for (int i = 0; i < inventorySize; i++) 
        {
            UIInventorySlot slot = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            slot.transform.SetParent(contentPanel, false);
            slots.Add(slot);
            SlotAddListeners(slot);
        }
        chestplateSlot.SetData(def, 1);

        Hide();
    }
    private void SlotAddListeners(UIInventorySlot slot)
    {
        slot.OnItemClicked += HandleItemSelection;
        slot.OnItemBeginDrag += HandleBeginDrag;
        slot.OnItemDroppedOn += HadleDropOn;
        slot.OnItemEndDrag += HandleEndDrag;
    }

    private void Awake()
    {
        Hide();
        mouseFollower.Toggle(false);
    }

    public void UpdateData(int itemIndex,
            Sprite itemImage, int itemQuantity)
    {
        if (slots.Count > itemIndex)
        {
            slots[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    internal void ResetAllItems()
    {
        foreach (var item in slots)
        {
            item.ResetData();
            item.Deselect();
        }
    }
    private void DeselectAllItems()
    {
        foreach (var item in slots)
        {
            item.Deselect();
        }
    }
    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(UIInventorySlot inventoryItemUI)
    {
        Debug.Log("HandleItemSelection");
        DeselectAllItems();
        inventoryItemUI.Select();
    }
    private void HandleBeginDrag(UIInventorySlot inventoryItemUI)
    {
        int index = slots.IndexOf(inventoryItemUI);
        if (index == -1)
            return;

        currentlyDraggedItemIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }
    private void HadleDropOn(UIInventorySlot inventoryItemUI)
    {
        int index = slots.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        HandleItemSelection(inventoryItemUI);
    }
    private void HandleEndDrag(UIInventorySlot inventoryItemUI)
    {
        Debug.Log("HandleEndDrag");
        mouseFollower.Toggle(false);
        ResetDraggedItem();
    }
    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }
}
