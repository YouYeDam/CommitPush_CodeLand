using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    PlayerManager PlayerManager;
    private Vector2 InventoryOriginalPosition;
    private Vector2 CharacterOriginalPosition;

    GameObject InventoryButtonObject;
    GameObject CharacterButtonObject;
    Button InventoryButton;
    Button CharacterButton;
    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager");
        Inventory = UIManager.transform.GetChild(0).gameObject;
        Character = UIManager.transform.GetChild(1).gameObject;

        // 원래 위치 저장
        InventoryOriginalPosition = Inventory.GetComponent<RectTransform>().anchoredPosition;
        CharacterOriginalPosition = Character.GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnInventory() {
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

            // 버튼으로 닫게하기
            if (InventoryButtonObject == null) {
                InventoryButtonObject = GameObject.Find("Inventory Close Button");
                InventoryButton = InventoryButtonObject.GetComponent<Button>();
                InventoryButton.onClick.AddListener(OnInventory);
            }
        }
    }
    public void OnCharacter() {
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

            // 버튼으로 닫게하기
            if (CharacterButtonObject == null) {
                CharacterButtonObject = GameObject.Find("Character Close Button");
                CharacterButton = CharacterButtonObject.GetComponent<Button>();
                CharacterButton.onClick.AddListener(OnCharacter);
            }
        }
    }
}
