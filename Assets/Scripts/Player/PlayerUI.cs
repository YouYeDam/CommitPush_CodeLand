using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    [SerializeField] GameObject Equipment;
    [SerializeField] GameObject Dialogue;
    [SerializeField] public GameObject Shop;
    PlayerManager PlayerManager;
    private Vector2 InventoryOriginalPosition;
    private Vector2 CharacterOriginalPosition;
    private Vector2 EquipmentOriginalPosition;
    private Vector2 ShopOriginalPosition;

    GameObject InventoryButtonObject;
    GameObject CharacterButtonObject;
    GameObject EquipmentButtonObject;
    public GameObject ShopButtonObject;
    public GameObject DialogueButtonObject;

    Button InventoryButton;
    Button CharacterButton;
    Button EquipmentButton;
    Button DialogueButton;
    Button ShopButton;

    PlayerLevelUpController PlayerLevelUpController;

    void Start() {
        PlayerLevelUpController = GetComponent<PlayerLevelUpController>();
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager");
        Character = UIManager.transform.GetChild(1).gameObject;
        Equipment = UIManager.transform.GetChild(2).gameObject;
        Inventory = UIManager.transform.GetChild(3).gameObject;
        Dialogue = UIManager.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject;
        Shop = UIManager.transform.GetChild(7).gameObject;
        // 원래 위치 저장
        InventoryOriginalPosition = Inventory.GetComponent<RectTransform>().anchoredPosition;
        CharacterOriginalPosition = Character.GetComponent<RectTransform>().anchoredPosition;
        EquipmentOriginalPosition = Equipment.GetComponent<RectTransform>().anchoredPosition;
        ShopOriginalPosition = Shop.GetComponent<RectTransform>().anchoredPosition;
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

            // 닫기 버튼 연결
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

            // 닫기 버튼 연결
            if (CharacterButtonObject == null) {
                CharacterButtonObject = GameObject.Find("Character Close Button");
                CharacterButton = CharacterButtonObject.GetComponent<Button>();
                CharacterButton.onClick.AddListener(OnCharacter);
                PlayerLevelUpController.ConnectButton();
            }
        }
    }
    public void OnEquipment() {
        if (Equipment == null || !PlayerManager.CanInput) {
            return;
        }
        if (Equipment.activeSelf) {
            Equipment.SetActive(false);
        }
        else {
            Equipment.SetActive(true);
            // 원래 위치로 복원
            Equipment.GetComponent<RectTransform>().anchoredPosition = EquipmentOriginalPosition;

            // 닫기 버튼 연결
            if (EquipmentButtonObject == null) {
                EquipmentButtonObject = GameObject.Find("Equipment Close Button");
                EquipmentButton = EquipmentButtonObject.GetComponent<Button>();
                EquipmentButton.onClick.AddListener(OnEquipment);
            }
        }
    }

    public void SetDialogueButton() {
        DialogueButtonObject = GameObject.Find("Dialogue Close Button");
        DialogueButton = DialogueButtonObject.GetComponent<Button>();
        DialogueButton.onClick.AddListener(CloseDialogue);
    }
    public void CloseDialogue() {
        if (Dialogue.activeSelf) {
            Dialogue.SetActive(false);
        }
    }

    public void OpenShop() {
        if (Shop == null || !PlayerManager.CanInput) {
            return;
        }
        if (!Shop.activeSelf) {
            Shop.SetActive(true);
        }
        if (!Inventory.activeSelf) { // 인벤토리 창도 같이 열기
            OnInventory();
        }
        if (Dialogue.activeSelf) { // 대화창이 열려있으면 대화창 닫기
            CloseDialogue();
        }
        // 원래 위치로 복원
        Shop.GetComponent<RectTransform>().anchoredPosition = ShopOriginalPosition;
        if (ShopButtonObject == null) {
            ShopButtonObject = GameObject.Find("Shop Close Button");
            ShopButton = ShopButtonObject.GetComponent<Button>();
            ShopButton.onClick.AddListener(CloseShop);
        }
    }

    public void CloseShop() {
        if (Shop.activeSelf) {
            Shop.SetActive(false);
        }
        if (Inventory.activeSelf) {
            Inventory.SetActive(false);
        }
    }
}
