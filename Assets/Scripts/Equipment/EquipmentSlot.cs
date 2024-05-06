using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item Item; // 장비 아이템
    public Image ItemImage;  // 아이템의 이미지
    ItemToolTip ItemToolTip;
    private int ClickCount = 0;
    private float LastClickTime = 0f;
    private const float DoubleClickTime = 0.2f; // 더블 클릭 간격

    void Start() {
        GameObject ToolTipObject = GameObject.Find("EquipmentToolTip");
        if (ToolTipObject != null) {
        ItemToolTip = ToolTipObject.GetComponent<ItemToolTip>();
        }
    }
    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    void ClearSlot() { // 해당 슬롯 하나 삭제
        Item = null;
        ItemImage.sprite = null;
        SetColor(0);
    }

    public void OnPointerClick(PointerEventData eventData) {
        ClickCount++;
        if (ClickCount == 1)
        {
            LastClickTime = Time.time;
        }
        else if (ClickCount == 2 && Time.time - LastClickTime < DoubleClickTime)
        {
            OnDoubleClick(); // 더블 클릭 발생
            ClickCount = 0; // 클릭 카운트 초기화
        }
        else if (Time.time - LastClickTime > DoubleClickTime)
        {
            ClickCount = 1; // 시간 초과로 다시 1부터 카운트
            LastClickTime = Time.time;
        }
    }

    void OnDoubleClick() // 슬롯 더블클릭
    {
        ClearSlot();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Item != null) {
            ItemToolTip.ShowToolTip(Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        ItemToolTip.HideToolTip();
    }
}
