using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static CharacteristicsUI;
using static Inventory;

public class CharacteristicsController : MonoBehaviour
{
    [SerializeField]
    private CharacteristicsUI characteristicsUI;
    [SerializeField]
    private PlayerCharacteristics characteristicsModel;
    [SerializeField]
    private PlayerCharacteristics currentCharacteristics;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!characteristicsUI.isActiveAndEnabled)
            {
                Time.timeScale = 0;
                characteristicsUI.Show();

                characteristicsModel.SetData(currentCharacteristics);
                characteristicsUI.UpdateData(currentCharacteristics, characteristicsModel);
            }
            else
            {
                Time.timeScale = 1;
                characteristicsUI.Hide();
            }
        }
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
