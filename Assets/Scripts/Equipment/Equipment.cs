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

    void Start() {
        HelmetSlot.InitialEquip();
        HelmetSlot.ConnectUIManager();

        BodySlot.InitialEquip();
        BodySlot.ConnectUIManager();

        PantsSlot.InitialEquip();
        PantsSlot.ConnectUIManager();

        WeaponSlot.InitialEquip();
        WeaponSlot.ConnectUIManager();

        GloveSlot.InitialEquip();
        GloveSlot.ConnectUIManager();

        ShoesSlot.InitialEquip();
        ShoesSlot.ConnectUIManager();

        PetSlot.InitialEquip();
        PetSlot.ConnectUIManager();

        TrinketSlot.InitialEquip();
        TrinketSlot.ConnectUIManager();
    }
}
