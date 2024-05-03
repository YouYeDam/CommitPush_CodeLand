using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item Item; // 획득한 아이템
    public int ItemCount; // 획득한 아이템의 개수
    public Image ItemImage;  // 아이템의 이미지

    [SerializeField] TMP_Text TextCount;
    [SerializeField] GameObject CountImage;

    void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }
    public void AddItem(Item Item, int Count = 1) { //인벤토리에 새로운 아이템 슬롯 추가
        this.Item = Item;
        ItemCount = Count;
        ItemImage.sprite = this.Item.ItemImage;

        if(this.Item.Type != Item.ItemType.Equipment)
        {
            CountImage.SetActive(true);
            TextCount.text = ItemCount.ToString();
        }
        else
        {
            TextCount.text = "0";
            CountImage.SetActive(false);
        }
        SetColor(1);
    }

    
    public void SetSlotCount(int Count) {// 해당 슬롯의 아이템 갯수 업데이트
        ItemCount += Count;
        TextCount.text = ItemCount.ToString();

        if (ItemCount <= 0)
            ClearSlot();
    }

    void ClearSlot() { // 해당 슬롯 하나 삭제
        Item = null;
        ItemCount = 0;
        ItemImage.sprite = null;
        SetColor(0);

        TextCount.text = "0";
        CountImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Item != null)
        {
            ItemDrag.Instance.DragSlot = this;
            ItemDrag.Instance.DragSetImage(ItemImage);
            ItemDrag.Instance.MyRectTransform.anchoredPosition += eventData.delta/ ItemDrag.Instance.UIManager.scaleFactor;
            SetColor(0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item != null) 
        {
            ItemDrag.Instance.MyRectTransform.anchoredPosition += eventData.delta/ ItemDrag.Instance.UIManager.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemDrag.Instance.SetColor(0);
        ItemDrag.Instance.DragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDrag.Instance.DragSlot != null) 
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Item TempItem = Item;
        int TempItemCount = ItemCount;

        AddItem(ItemDrag.Instance.DragSlot.Item, ItemDrag.Instance.DragSlot.ItemCount);

        if (TempItem != null) 
        {
            ItemDrag.Instance.DragSlot.AddItem(TempItem, TempItemCount);
        }
        else 
        {
            ItemDrag.Instance.DragSlot.ClearSlot();
        }
    }
}