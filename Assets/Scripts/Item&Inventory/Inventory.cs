using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject Content; // Slot들의 부모인 Content

    private Slot[] Slots; // 슬롯들 배열

    void Start()
    {
        Slots = Content.GetComponentsInChildren<Slot>();
    }
    
    public void AcquireItem(Item Item, int Count = 1) { // 아이템 획득
        if(Item.ItemType.Used == Item.Type || Item.ItemType.ETC == Item.Type) { // 소비 혹은 기타 아이템일 경우에만 개수 스택
            for (int i = 0; i < Slots.Length; i++) {
                if (Slots[i].Item != null) {
                    if (Slots[i].Item.ItemName == Item.ItemName && Slots[i].ItemCount <= 99) { // 아이템 이름이 같으면 개수 더해주기
                        int TotalItemCount = Slots[i].ItemCount + Count;

                        if (TotalItemCount <= 99) {
                            Slots[i].SetSlotCount(Count);
                            return;
                        }
                        else { // 더한 아이템 개수가 99가 넘는다면 99개까지 채우고 나머지로 다시 다른 슬롯에 할당
                            int RemainingCount = TotalItemCount - 99;
                            Slots[i].SetSlotCount(99 - Slots[i].ItemCount);
                            Count = RemainingCount;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < Slots.Length; i++) { // 소비 혹은 기타 아이템이 아니라면 스택 없이 슬롯에 넣기
            if (Slots[i].Item == null)
            {
                Slots[i].AddItem(Item, Count);
                return;
            }
        }
    }

    public void RemoveItem(string itemName, int Count) { // 아이템을 제거하는 기능 (퀘스트 완료 시 아이템 차감하는 용도)
        foreach (var slot in Slots) {
            if (slot.Item != null && slot.Item.ItemName == itemName) {
                if (slot.ItemCount >= Count) {
                    slot.SetSlotCount(-Count); // 기존 수량에서 count만큼 감소
                    return;
                }
                else { // 해당 슬롯의 수량이 차감해야 할 수량보다 적다면 모두 없애고, 나머지로 다시 검사
                    int RemainingCount = Count - slot.ItemCount;
                    slot.SetSlotCount(-slot.ItemCount); // 슬롯을 0으로 설정
                    Count = RemainingCount;
                }
            }
        }
    }

    public int GetItemAmount(string itemName) { // 특정 아이템의 수량을 반환하는 기능
        int Amount = 0;
        foreach (var slot in Slots)
        {
            if (slot.Item != null && slot.Item.ItemName == itemName)
            {
                Amount += slot.ItemCount;
            }
        }
        return Amount;
    }
}