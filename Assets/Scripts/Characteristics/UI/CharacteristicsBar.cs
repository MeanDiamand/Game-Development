using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacteristicsBar : MonoBehaviour
{
    [SerializeField]
    private Image ProgressImage;
    [SerializeField]
    private TMP_Text valueTxt;
    public int ID { get; set; }

    public event Action<CharacteristicsBar> ValueIncreased, ValueDecreased;


    public void Start()
    {
        
    }
    public void Decrease()
    {
        ValueDecreased?.Invoke(this);
    }
    public void Increase()
    {
        ValueIncreased?.Invoke(this);
    }
    public void UpdateData(int value, int maximum = 10, int minimum = 0)
    {
        if (value < minimum || value > maximum)
        {
            Debug.LogWarning($"Invalid progress value, expected between {minimum} and {maximum}. Got {value}. Clamping.");
            value = Mathf.Clamp(value, minimum, maximum);
        }

        ProgressImage.fillAmount = (float)(value - minimum) / (maximum - minimum);
        valueTxt.text = value.ToString();
    }

}
