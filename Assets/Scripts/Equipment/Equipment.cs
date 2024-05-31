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
        playerStatus = FindObjectOfType<PlayerStatus>();
        HelmetSlot.InitialEquip(playerStatus.IsLoaded);
        HelmetSlot.ConnectUIManager();

        BodySlot.InitialEquip(playerStatus.IsLoaded);
        BodySlot.ConnectUIManager();

        PantsSlot.InitialEquip(playerStatus.IsLoaded);
        PantsSlot.ConnectUIManager();

        WeaponSlot.InitialEquip(playerStatus.IsLoaded);
        WeaponSlot.ConnectUIManager();

        GloveSlot.InitialEquip(playerStatus.IsLoaded);
        GloveSlot.ConnectUIManager();

        ShoesSlot.InitialEquip(playerStatus.IsLoaded);
        ShoesSlot.ConnectUIManager();

        PetSlot.InitialEquip(playerStatus.IsLoaded);
        PetSlot.ConnectUIManager();

        TrinketSlot.InitialEquip(playerStatus.IsLoaded);
        TrinketSlot.ConnectUIManager();
    }
}
