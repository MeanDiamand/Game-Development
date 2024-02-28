using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    List<UIInventorySlot> inventorySlots = new List<UIInventorySlot>();

    public void Initialize(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++) 
        {
            UIInventorySlot slot = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            slot.transform.SetParent(contentPanel);
            inventorySlots.Add(slot);
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
}
