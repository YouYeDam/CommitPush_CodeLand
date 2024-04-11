using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    void OnInventory() {
        if (Inventory.activeSelf) {
            Inventory.SetActive(false);
        }
        else {
            Inventory.SetActive(true);
        }
    }
    void OnCharacter() {
        if (Character.activeSelf) {
            Character.SetActive(false);
        }
        else {
            Character.SetActive(true);
        }
    }
}
