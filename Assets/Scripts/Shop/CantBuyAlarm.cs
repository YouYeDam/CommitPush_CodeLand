using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantBuyAlarm : MonoBehaviour
{
    [SerializeField] GameObject CantBuyAlarmBase;

    public void OpenCantBuyAlarm() { // 아이템 구매 불가 알람 띄우기
        CantBuyAlarmBase.SetActive(true);
    }

    public void OK() { // 확인 시 알람 제거
        CantBuyAlarmBase.SetActive(false);
    }
}
