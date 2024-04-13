using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    PlayerManager PlayerManager;
    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager");
        Inventory = UIManager.transform.GetChild(0).gameObject;
        Character = UIManager.transform.GetChild(1).gameObject;
    }

    void OnInventory() {
        if (Inventory == null || !PlayerManager.CanInput) {
            return;
        }
        if (Inventory.activeSelf) {
            Inventory.SetActive(false);
        }
        else {
            Inventory.SetActive(true);
        }
    }
    void OnCharacter() {
        if (Character == null || !PlayerManager.CanInput) {
            return;
        }
        if (Character.activeSelf) {
            Character.SetActive(false);
        }
        else {
            Character.SetActive(true);
        }
    }
}
