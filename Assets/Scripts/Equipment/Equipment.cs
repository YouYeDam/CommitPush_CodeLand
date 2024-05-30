using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipmentSlot HelmetSlot;
    public EquipmentSlot BodySlot;
    public EquipmentSlot PantsSlot;
    public EquipmentSlot WeaponSlot;
    public EquipmentSlot GloveSlot;
    public EquipmentSlot ShoesSlot;
    public EquipmentSlot PetSlot;
    public EquipmentSlot TrinketSlot;
    private PlayerStatus playerStatus;

    void Start()
    { // 새로하기 할 때만 실행되면 됨
        // playerStatus = FindObjectOfType<PlayerStatus>();
        // if (!playerStatus.IsLoaded)
        // {
            // Debug.Log("log331: Equipment init");
            HelmetSlot.InitialEquip();
            BodySlot.InitialEquip();
            PantsSlot.InitialEquip();
            WeaponSlot.InitialEquip();
            GloveSlot.InitialEquip();
            ShoesSlot.InitialEquip();
            PetSlot.InitialEquip();
            TrinketSlot.InitialEquip();
        // }
        // else{
            // Debug.Log("log332: Equipment init passed");}
    }
}
