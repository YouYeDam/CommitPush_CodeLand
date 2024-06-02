using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject UIManager;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Character;
    [SerializeField] GameObject Skill;
    [SerializeField] GameObject Equipment;
    [SerializeField] GameObject Dialogue;
    [SerializeField] GameObject Quest;
    [SerializeField] public GameObject Shop;
    PlayerManager PlayerManager;
    private Vector2 InventoryOriginalPosition;
    private Vector2 CharacterOriginalPosition;
    private Vector2 SkillOriginalPosition;
    private Vector2 EquipmentOriginalPosition;
    private Vector2 ShopOriginalPosition;
    private Vector2 QuestOriginalPosition;

    GameObject InventoryButtonObject;
    GameObject CharacterButtonObject;
    GameObject SkillButtonObject;
    GameObject EquipmentButtonObject;
    GameObject QuestButtonObject;
    public GameObject ShopButtonObject;
    public GameObject DialogueButtonObject;

    Button InventoryButton;
    Button CharacterButton;
    Button SkillButton;
    Button EquipmentButton;
    Button QuestButton;
    Button DialogueButton;
    Button ShopButton;

    PlayerLevelUpController PlayerLevelUpController;

    void Start() {
        PlayerLevelUpController = GetComponent<PlayerLevelUpController>();
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = GameObject.Find("UIManager");
        Character = UIManager.transform.GetChild(2).gameObject;
        Skill = UIManager.transform.GetChild(3).gameObject;
        Equipment = UIManager.transform.GetChild(4).gameObject;
        Inventory = UIManager.transform.GetChild(5).gameObject;
        Quest = UIManager.transform.GetChild(7).gameObject;
        Dialogue = UIManager.transform.GetChild(9).gameObject.transform.GetChild(0).gameObject;
        Shop = UIManager.transform.GetChild(6).gameObject;

        // 원래 위치 저장
        CharacterOriginalPosition = Character.GetComponent<RectTransform>().anchoredPosition;
        SkillOriginalPosition = Skill.GetComponent<RectTransform>().anchoredPosition;
        EquipmentOriginalPosition = Equipment.GetComponent<RectTransform>().anchoredPosition;
        InventoryOriginalPosition = Inventory.GetComponent<RectTransform>().anchoredPosition;
        QuestOriginalPosition = Quest.GetComponent<RectTransform>().anchoredPosition;
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

    public void OnQuest() {
        if (Quest == null || !PlayerManager.CanInput) {
            return;
        }
        if (Quest.activeSelf) {
            Quest.SetActive(false);
        }
        else {
            Quest.SetActive(true);
            // 원래 위치로 복원
            Quest.GetComponent<RectTransform>().anchoredPosition = QuestOriginalPosition;

            // 닫기 버튼 연결
            if (QuestButtonObject == null) {
                QuestButtonObject = GameObject.Find("Quest Close Button");
                QuestButton = QuestButtonObject.GetComponent<Button>();
                QuestButton.onClick.AddListener(OnQuest);
            }
        }
    }
    public void OnSkill() {
        if (Skill == null || !PlayerManager.CanInput) {
            return;
        }
        if (Skill.activeSelf) {
            Skill.SetActive(false);
        }
        else {
            Skill.SetActive(true);
            // 원래 위치로 복원
            Skill.GetComponent<RectTransform>().anchoredPosition = SkillOriginalPosition;

            // 닫기 버튼 연결
            if (SkillButtonObject == null) {
                SkillButtonObject = GameObject.Find("Skill Close Button");
                SkillButton = SkillButtonObject.GetComponent<Button>();
                SkillButton.onClick.AddListener(OnSkill);
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
