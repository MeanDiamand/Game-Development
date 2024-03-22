using UnityEngine;

public interface IUIController
{
    public bool Trigger();
}

public class UIController : MonoBehaviour
{
    [SerializeField]
    private InventoryController inventoryController;
    [SerializeField]
    private CharacteristicsController characteristicsController;
    [SerializeField]
    private QuickAccessBar quickAccessBar;

    private bool inventoryOpened;
    private bool characteristicsOpened;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !characteristicsOpened)
        {
            inventoryOpened = inventoryController.Trigger();
            ProcessQAB(inventoryOpened);
        }
        if (Input.GetKeyDown(KeyCode.B) && !inventoryOpened)
        {
            characteristicsOpened = characteristicsController.Trigger();
            ProcessQAB(characteristicsOpened);
        }
        if (Input.GetKeyDown(KeyCode.E) && !inventoryOpened && !characteristicsOpened)
        {
            Debug.Log("E Pressed");
            quickAccessBar.UseE();
        }
        if (Input.GetKeyDown(KeyCode.Q) && !inventoryOpened && !characteristicsOpened)
            quickAccessBar.UseQ();
    }

    private void ProcessQAB(bool var)
    {
        if (var)
            quickAccessBar.Hide();
        else
            quickAccessBar.Show();
    }

    private void Start()
    {
        if (inventoryController == null)
            Debug.LogError("inventoryController is null in Controller");
        if (characteristicsController == null)
            Debug.LogError("characteristicsController is null in Controller");
        if (quickAccessBar == null)
            Debug.LogError("quickAccessBar is null in Controller");
    }
}
