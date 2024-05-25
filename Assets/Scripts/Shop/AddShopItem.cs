using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddShopItem : MonoBehaviour
{
    [SerializeField] List<Item> ShopItems; // 상점에 등록할 아이템 리스트
    private ShopSlot[] ShopSlots;  // 슬롯들 배열

    public void SetContent() {
        GameObject ShopContent = GameObject.Find("ShopContent");
        ShopSlots = ShopContent.GetComponentsInChildren<ShopSlot>();
    }
    public void AddEachItem() {
        ClearSlot();
        foreach (Item ShopItem in ShopItems) {
            AddItemOnShop(ShopItem);
        }
    }
    void ClearSlot() {
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

