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

    private bool isActive = true;

    public void Update()
    {
        if (!isActive) return;
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
        {
            quickAccessBar.Hide();
            Time.timeScale = 0;
        }
        else
        {
            quickAccessBar.Show();
            Time.timeScale = 1;
        }
    }

    private void Start()
    {
        if (inventoryController == null)
            Debug.LogError("inventoryController is null in Controller");
        if (characteristicsController == null)
            Debug.LogError("characteristicsController is null in Controller");
        if (quickAccessBar == null)
            Debug.LogError("quickAccessBar is null in Controller");
        inventoryController.OnInventoryUpdated += quickAccessBar.Upd;
    }
    public void Hide()
    {
        isActive = false;
        quickAccessBar.Hide();
    }
    public void Show()
    {
        isActive = true;
        quickAccessBar.Show();
    }
    public bool IsInputBlocked()
    {
        return inventoryOpened || characteristicsOpened;
    }
}
