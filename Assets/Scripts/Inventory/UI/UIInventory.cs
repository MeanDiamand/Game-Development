using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private UIInventorySlot itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;
    
    List<UIInventorySlot> mainInventorySlots = new List<UIInventorySlot>();

    [SerializeField]
    UIInventorySlot helmetSlot;
    UIInventorySlot chestplateSlot;
    UIInventorySlot legginsSlot;
    UIInventorySlot bootsSlot;

    UIInventorySlot weaponSlot;
    List<UIInventorySlot> quickAccessSlots = new List<UIInventorySlot>();

    [SerializeField]
    private Sprite def;

    public void Initialize(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++) 
        {
            UIInventorySlot slot = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            slot.transform.SetParent(contentPanel);
            mainInventorySlots.Add(slot);
        }
        
        //for (int i = 0; i < 4; i++)
        //{
            //UIInventorySlot slot = UIInventorySlot.InstantiateEmpty(contentPanel, itemPrefab);
            //quickAccessSlots.Add(slot);
        //}

        helmetSlot.SetData(def, 1);
    }

    public void UpdateData(int itemIndex,
            Sprite itemImage, int itemQuantity)
    {
        if (mainInventorySlots.Count > itemIndex)
        {
            mainInventorySlots[itemIndex].SetData(itemImage, itemQuantity);
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
