using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicsUI : MonoBehaviour
{
    [SerializeField]
    private List<CharacteristicsBar> characteristicsBarList;
    [SerializeField]
    private TMP_Text valueAvailablePoints;
    [SerializeField]
    private TMP_Text TMPLevel;
    [SerializeField]
    private Image ExperienceProgressImage;


    public event Action<ValueAndID> OnCharacteristicChanged;
    public event Action OnChangesApplied;

    public void Initialize(int numberOfBars)
    {
        for (int i = 0; i < numberOfBars; i++)
        {
            characteristicsBarList[i].ID = i;
            characteristicsBarList[i].ValueIncreased += IncreaseCharacteristic;
            characteristicsBarList[i].ValueDecreased += DecreaseCharacteristic;
        }
    }

    public void UpdateData(PlayerCharacteristics fixedCharacteristics, PlayerCharacteristics characteristics)
    {
        characteristicsBarList[0].UpdateData(fixedCharacteristics.Strength, characteristics.Strength);
        characteristicsBarList[1].UpdateData(fixedCharacteristics.Endurance, characteristics.Endurance);
        characteristicsBarList[2].UpdateData(fixedCharacteristics.Intelligence, characteristics.Intelligence);
        characteristicsBarList[3].UpdateData(fixedCharacteristics.Agility, characteristics.Agility);
        characteristicsBarList[4].UpdateData(fixedCharacteristics.Luck, characteristics.Luck);

        valueAvailablePoints.text = characteristics.AvailablePoints.ToString();

        TMPLevel.text = characteristics.Level.ToString();
        ExperienceProgressImage.fillAmount = (float)(characteristics.Experience) / (characteristics.MaxExp());
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void IncreaseCharacteristic(CharacteristicsBar bar)
    {
        Debug.Log("ID: " + bar.ID);
        OnCharacteristicChanged?.Invoke(new ValueAndID() { ID = bar.ID, Value = 1});
    }

    public void DecreaseCharacteristic(CharacteristicsBar bar)
    {
        OnCharacteristicChanged?.Invoke(new ValueAndID() { ID = bar.ID, Value = -1 });
    }

    public struct ValueAndID
    {
        public int ID;
        public int Value;
    }

    public void Apply()
    {
        OnChangesApplied?.Invoke();
    }
}
