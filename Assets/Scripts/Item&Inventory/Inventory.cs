using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject Content;  // Slot들의 부모인 Content

    private Slot[] Slots;  // 슬롯들 배열

    void Start()
    {
        Slots = Content.GetComponentsInChildren<Slot>();
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
                        int TotalItemCount = Slots[i].ItemCount + Count;
                        if (TotalItemCount <= 99)
                        {
                            Slots[i].SetSlotCount(Count);
                            return;
                        }
                        else
                        {
                            int RemainingCount = TotalItemCount - 99;
                            Slots[i].SetSlotCount(99 - Slots[i].ItemCount);
                            Count = RemainingCount;
                        }
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