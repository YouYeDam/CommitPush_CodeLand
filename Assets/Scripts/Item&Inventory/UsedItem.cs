using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedItem : MonoBehaviour
{
    [SerializeField] string UsedType; // 소비 유형(Food, Drink, Tonic 등)
    [SerializeField] string TonicType; // 강장제 유형(ATK, DEF 등)
    [SerializeField] int EffectValue; // 영향값
    [SerializeField] int EffectDuration; // 기간

    public PlayerStatus PlayerStatus;

    public void EffectItem() {
        switch (UsedType) {
            case "Food":
                IncreaseHP();
                break;
            case "Drink":
                IncreaseMP();
                break;
            case "Tonic":
                IncreaseStatus();
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

    public void IncreaseStatus() {
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

    public void EndIncreaseStatus() {
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

}

