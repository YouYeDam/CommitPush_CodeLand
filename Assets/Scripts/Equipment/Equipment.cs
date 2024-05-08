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

    void Start() {
        HelmetSlot.InitialEquip();
        BodySlot.InitialEquip();
        PantsSlot.InitialEquip();
        WeaponSlot.InitialEquip();
        GloveSlot.InitialEquip();
        ShoesSlot.InitialEquip();
        PetSlot.InitialEquip();
        TrinketSlot.InitialEquip();
    }
}
