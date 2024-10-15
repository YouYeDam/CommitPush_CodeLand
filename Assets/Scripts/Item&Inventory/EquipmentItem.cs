using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipmentItem : MonoBehaviour
{
    [SerializeField] public string EquipmentType; // 장비 유형(머리, 상의, 하의, 무기, 장갑, 신발, 펫, 장신구)
    [SerializeField] public int RequireLevel = 1; //필요 착용 레벨
    [SerializeField] public string EquipmentItemGrade = "일반"; // 기본 장비 등급
    [SerializeField] int HPValue;
    [SerializeField] int MPValue;
    [SerializeField] int ATKValue;
    [SerializeField] int DEFValue;
    [SerializeField] float APValue;
    [SerializeField] float CritValue;
    public PlayerStatus PlayerStatus;
    public Equipment Equipment;
    public bool IsSpecialEquipment = false;
    
    public void EquipItem(Item NewItem) { // 아이템 착용 기능
        Equipment = FindObjectOfType<Equipment>();
        switch (EquipmentType) { // 각 타입별로 알맞은 부위에 수행
            case "머리":
                Equipment.HelmetSlot.CheckNull(NewItem);
                break;
            case "상의":
                Equipment.BodySlot.CheckNull(NewItem);
                break;
            case "하의":
                Equipment.PantsSlot.CheckNull(NewItem);
                break;
            case "무기":
                Equipment.WeaponSlot.CheckNull(NewItem);
                break;
            case "장갑":
                Equipment.GloveSlot.CheckNull(NewItem);
                break;
            case "신발":
                Equipment.ShoesSlot.CheckNull(NewItem);
                break;
            case "펫":
                Equipment.PetSlot.CheckNull(NewItem);
                break;
            case "장신구":
                Equipment.TrinketSlot.CheckNull(NewItem);
                break;
            default:
                break;
        }
    }

    public void IncreaseStat() { // 장비 착용 시 장비 스탯만큼 캐릭터 스탯 상승
        if (HPValue != 0) {
            IncreaseHP();
        }
        if (MPValue != 0) {
            IncreaseMP();
        }
        if (ATKValue != 0) {
            IncreaseATK();
        }
        if (DEFValue != 0) {
            IncreaseDEF();
        }
        if (APValue != 0) {
            IncreaseAP();
        }
        if (CritValue != 0) {
            IncreaseCrit();
        }
    }

    public void IncreaseHP() {
        PlayerStatus.PlayerMaxHP += HPValue;
    }

    public void IncreaseMP() {
        PlayerStatus.PlayerMaxMP += MPValue;
    }

    public void IncreaseATK() {
        PlayerStatus.PlayerATK += ATKValue;
    }

    public void IncreaseDEF() {
        PlayerStatus.PlayerDEF += DEFValue;
    }

    public void IncreaseAP() {
        PlayerStatus.PlayerAP += APValue;
    }

    public void IncreaseCrit() {
        PlayerStatus.PlayerCrit += CritValue;
    }

    public void DecreaseStat() { // 장비 탈착 시 스탯 원상태 복구
        if (HPValue != 0) {
            DecreaseHP();
        }
        if (MPValue != 0) {
            DecreaseMP();
        }
        if (ATKValue != 0) {
            DecreaseATK();
        }
        if (DEFValue != 0) {
            DecreaseDEF();
        }
        if (APValue != 0) {
            DecreaseAP();
        }
        if (CritValue != 0) {
            DecreaseCrit();
        }
    }

    public void DecreaseHP() {
        PlayerStatus.PlayerMaxHP -= HPValue;
        if (PlayerStatus.PlayerCurrentHP > PlayerStatus.PlayerMaxHP) {
            PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP;
        }
    }

    public void DecreaseMP() {
        PlayerStatus.PlayerMaxMP -= MPValue;
        if (PlayerStatus.PlayerCurrentMP > PlayerStatus.PlayerMaxMP) {
            PlayerStatus.PlayerCurrentMP = PlayerStatus.PlayerMaxMP;
        }
    }

    public void DecreaseATK() {
        PlayerStatus.PlayerATK -= ATKValue;
    }

    public void DecreaseDEF() {
        PlayerStatus.PlayerDEF -= DEFValue;
    }

    public void DecreaseAP() {
        PlayerStatus.PlayerAP -= APValue;
    }

    public void DecreaseCrit() {
        PlayerStatus.PlayerCrit -= CritValue;
    }
}
