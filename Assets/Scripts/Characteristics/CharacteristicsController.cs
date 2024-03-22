using UnityEngine;
using static CharacteristicsUI;
using static Inventory;

public class CharacteristicsController : MonoBehaviour, IUIController
{
    [SerializeField]
    private CharacteristicsUI characteristicsUI;
    [SerializeField]
    private PlayerCharacteristics characteristicsModel;
    [SerializeField]
    private PlayerCharacteristics currentCharacteristics;

    public bool Trigger()
    {
        if (!characteristicsUI.isActiveAndEnabled)
        {
            Time.timeScale = 0;
            characteristicsUI.Show();

            characteristicsModel.SetData(currentCharacteristics);
            characteristicsUI.UpdateData(currentCharacteristics, characteristicsModel);
            return true;
        }
        Time.timeScale = 1;
        characteristicsUI.Hide();
        return false;
    }

    private void Start()
    {
        characteristicsUI.Initialize(5);

        characteristicsModel = PlayerCharacteristics.CreateFrom(currentCharacteristics);

        characteristicsUI.OnCharacteristicChanged += ChangeValue;
        characteristicsUI.OnChangesApplied += ChagesApplied;
        characteristicsModel.OnUpdated += UpdateUI;
    }

    private void UpdateUI(PlayerCharacteristics characteristics)
    {
        characteristicsUI.UpdateData(currentCharacteristics, characteristics);
    }

    private void ChangeValue(ValueAndID vaid)
    {
        characteristicsModel.ChangeCharacteristicBy(vaid, currentCharacteristics.GetCharacteristic(vaid.ID));
    }

    private void ChagesApplied()
    {
        currentCharacteristics.SetData(characteristicsModel);
        characteristicsUI.UpdateData(currentCharacteristics,characteristicsModel);
    }
}
