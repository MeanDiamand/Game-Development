using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventory inventoryUI;
    [SerializeField]
    private Inventory inventoryModel;
    private void Start()
    {
        //inventoryUI.Initialize(inventoryModel.Size);
        //inventoryModel.Initialize();
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
}
