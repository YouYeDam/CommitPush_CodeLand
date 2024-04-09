using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour
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
}