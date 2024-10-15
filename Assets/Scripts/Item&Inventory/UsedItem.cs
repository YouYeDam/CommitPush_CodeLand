using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedItem : MonoBehaviour
{
    [SerializeField] string UsedType; // 소비 유형(음식, 음료, 강장제 등)
    [SerializeField] string TonicType; // 강장제 유형(ATK, DEF 등)
    [SerializeField] int EffectValue; // 영향값
    [SerializeField] int EffectDuration; // 기간

    public PlayerStatus PlayerStatus;
    public void EffectItem() { // 소비 아이템 사용시 효과
        switch (UsedType) {
            case "음식":
                IncreaseHP();
                break;
            case "음료":
                IncreaseMP();
                break;
            case "강장제":
                //IncreaseStatus();
                break;
            default:
                break;
        }
    }

    public void IncreaseHP() {
        PlayerStatus.PlayerCurrentHP += EffectValue;
        if (PlayerStatus.PlayerCurrentHP > PlayerStatus.PlayerMaxHP) {
            PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP;
        }
    }

    public void IncreaseMP() {
        PlayerStatus.PlayerCurrentMP += EffectValue;
        if (PlayerStatus.PlayerCurrentMP > PlayerStatus.PlayerMaxMP) {
            PlayerStatus.PlayerCurrentMP = PlayerStatus.PlayerMaxMP;
        }
    }

    /* 강장제 미구현
    public void IncreaseStatus() { // 강장제에만 해당
        switch (TonicType) {
            case "ATK":
                PlayerStatus.PlayerATK += EffectValue;
                break;
            case "DEF":
                PlayerStatus.PlayerDEF += EffectValue;
                break;
            default:
                break;
        }
        Invoke("EndIncreaseStatus", EffectDuration);
    }

    public void EndIncreaseStatus() { // 강장제 지속시간 끝나면 스탯 복구
        switch (TonicType) {
            case "ATK":
                PlayerStatus.PlayerATK -= EffectValue;
                break;
            case "DEF":
                PlayerStatus.PlayerDEF -= EffectValue;
                break;
            default:
                break;
        }
    }
    */
}

