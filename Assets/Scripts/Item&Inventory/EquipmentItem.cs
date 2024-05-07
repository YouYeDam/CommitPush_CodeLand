using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField] string EquipmentType; // 소비 유형(투구, 갑옷, 무기, 장갑, 신발, 펫, 장신구 등)
    [SerializeField] int[] EffectValue; // 영향값
    public PlayerStatus PlayerStatus;

    public void EffectItem() {
        switch (EquipmentType) {
            case "투구":
                break;
            case "갑옷":
                break;
            case "무기":
                break;
            case "장갑":
                break;
            case "신발":
                break;
            case "펫":
                break;
            case "장신구":
                break;
            default:
                break;
        }
    }
}
