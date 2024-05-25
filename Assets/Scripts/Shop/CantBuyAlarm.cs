using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantBuyAlarm : MonoBehaviour
{
    [SerializeField] GameObject CantBuyAlarmBase;

    public void OpenCantBuyAlarm()
    {
        CantBuyAlarmBase.SetActive(true);
    }

    public void OK()
    {
        CantBuyAlarmBase.SetActive(false);
    }
}
