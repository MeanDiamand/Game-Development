using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventory inventoryUI;
    int inventorySize = 10;
    private void Start()
    {
        inventoryUI.Initialize(inventorySize);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryUI.isActiveAndEnabled) 
                inventoryUI.Show();
            else
                inventoryUI.Hide();
        }
    }
}
