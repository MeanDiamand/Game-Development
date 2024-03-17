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
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!characteristicsUI.isActiveAndEnabled)
            {
                Time.timeScale = 0;
                characteristicsUI.Show();
                characteristicsUI.UpdateData(characteristicsModel);
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
        characteristicsUI.OnCharacteristicChanged += characteristicsModel.ChangeCharacteristicBy;
        characteristicsModel.OnUpdated += characteristicsUI.UpdateData;
    }
}
