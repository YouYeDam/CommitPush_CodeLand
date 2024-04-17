using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    PlayerManager PlayerManager;
    private Vector2 InventoryOriginalPosition;
    private Vector2 CharacterOriginalPosition;
    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager");
        Inventory = UIManager.transform.GetChild(0).gameObject;
        Character = UIManager.transform.GetChild(1).gameObject;

        // 원래 위치 저장
        InventoryOriginalPosition = Inventory.GetComponent<RectTransform>().anchoredPosition;
        CharacterOriginalPosition = Character.GetComponent<RectTransform>().anchoredPosition;
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
            // 원래 위치로 복원
            Inventory.GetComponent<RectTransform>().anchoredPosition = InventoryOriginalPosition;
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
            // 원래 위치로 복원
            Character.GetComponent<RectTransform>().anchoredPosition = CharacterOriginalPosition;
        }
    }
}
