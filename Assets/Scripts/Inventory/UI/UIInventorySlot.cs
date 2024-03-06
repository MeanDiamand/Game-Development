using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Windows;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text quantityTxt;

    public event Action<UIInventorySlot> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
            OnRightMouseBtnClick;

    private bool empty = true;

    public static UIInventorySlot InstantiateEmpty(RectTransform contentPanel, UIInventorySlot itemPrefab)
    {
        UIInventorySlot slot = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        slot.transform.SetParent(contentPanel);
        return slot;
    }

    public void Awake()
    {
        ResetData();
    }
    public void ResetData()
    {
        itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        quantityTxt.text = quantity.ToString();
        empty = false;
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        Debug.Log("Clicked");
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

}
