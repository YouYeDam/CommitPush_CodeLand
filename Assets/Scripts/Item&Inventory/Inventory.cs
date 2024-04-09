using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject Bag;  // Slot들의 부모인 Bag

    private Slot[] Slots;  // 슬롯들 배열

    void Start()
    {
        Slots = Bag.GetComponentsInChildren<Slot>();
    }
    
    public void AcquireItem(Item Item, int Count = 1) // 아이템 획득
    {
        if(Item.ItemType.Equipment != Item.Type)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item != null)
                {
                    if (Slots[i].Item.ItemName == Item.ItemName && Slots[i].ItemCount <= 99) // 아이템 이름이 같으면 갯수 더해주기
                    {
                        Slots[i].SetSlotCount(Count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Item == null)
            {
                Slots[i].AddItem(Item, Count);
                return;
            }
        }
    }
}