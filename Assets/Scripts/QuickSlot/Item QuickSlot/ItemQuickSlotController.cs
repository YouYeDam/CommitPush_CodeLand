using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemQuickSlotController : MonoBehaviour
{
    [SerializeField] ItemQuickSlot[] QuickSlots;  // 퀵슬롯들

    void Update()
    {
        TryInputNumber();
    }

    void TryInputNumber() { // 숫자 키(1, 2, 3, 4)를 눌러 해당하는 퀵슬롯 실행
        if (Input.GetKeyDown(KeyCode.Alpha1)) { // 0번 슬롯 실행
            Execute(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { // 1번 슬롯 실행
            Execute(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { // 2번 슬롯 실행
            Execute(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { // 3번 슬롯 실행
            Execute(3);
        }
    }

    void Execute(int SlotIndex) { // 퀵슬롯에 등록된 아이템 사용
        QuickSlots[SlotIndex].UseItem();
    }
}
