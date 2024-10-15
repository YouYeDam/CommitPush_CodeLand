using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddShopItem : MonoBehaviour
{
    [SerializeField] List<Item> ShopItems; // 상점에 등록할 아이템 리스트
    private ShopSlot[] ShopSlots;

    public void SetContent() {
        GameObject ShopContent = GameObject.Find("ShopContent");
        ShopSlots = ShopContent.GetComponentsInChildren<ShopSlot>();
    }
    public void AddEachItem() { // 상점 아이템 등록
        ClearSlot();
        foreach (Item ShopItem in ShopItems) { // 해당하는 상점의 아이템 다시 등록
            AddItemOnShop(ShopItem);
        }
    }
    void ClearSlot() { // 상점 아이템 슬롯 전부 비우기
        for (int i = 0; i < ShopSlots.Length; i++)
        {
            if (ShopSlots[i].Item != null)
            {
                ShopSlots[i].ClearSlot();
            }
        }
    }
    void AddItemOnShop(Item Item) // 개별 아이템 등록
    {
        for (int i = 0; i < ShopSlots.Length; i++)
        {
            if (ShopSlots[i].Item == null)
            {
                ShopSlots[i].AddItem(Item);
                return;
            }
        }
    }
}

