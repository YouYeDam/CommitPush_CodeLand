using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject UICanvas;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    PlayerManager PlayerManager;
    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        UICanvas = GameObject.Find("UIManager");
        Inventory = UICanvas.transform.GetChild(0).gameObject;
        Character = UICanvas.transform.GetChild(1).gameObject;
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
