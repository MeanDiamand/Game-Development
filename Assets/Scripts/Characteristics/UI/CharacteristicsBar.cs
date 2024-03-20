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
    private Image FixedProgressImage;
    [SerializeField]
    private TMP_Text valueTxt;
    public int ID { get; set; }

    public event Action<CharacteristicsBar> ValueIncreased, ValueDecreased;

    public void Decrease()
    {
        ValueDecreased?.Invoke(this);
    }
    public void Increase()
    {
        ValueIncreased?.Invoke(this);
    }
    public void UpdateData(int fixedProgress, int progress, int maximum = 10, int minimum = 0)
    {
        if (progress < minimum || progress > maximum)
        {
            Debug.LogWarning($"Invalid progress value, expected between {minimum} and {maximum}. Got {progress}. Clamping.");
            progress = Mathf.Clamp(progress, minimum, maximum);
        }

        ProgressImage.fillAmount = (float)(progress - minimum) / (maximum - minimum);
        FixedProgressImage.fillAmount = (float)(fixedProgress - minimum) / (maximum - minimum);
        valueTxt.text = progress.ToString();
    }

}
