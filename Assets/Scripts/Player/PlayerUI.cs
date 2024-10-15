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

        // UIManager에 연결된 UI 가져오기
        Character = UIManager.transform.GetChild(2).gameObject;
        Skill = UIManager.transform.GetChild(3).gameObject;
        Equipment = UIManager.transform.GetChild(4).gameObject;
        Inventory = UIManager.transform.GetChild(5).gameObject;
        Quest = UIManager.transform.GetChild(7).gameObject;
        Dialogue = UIManager.transform.GetChild(9).gameObject.transform.GetChild(0).gameObject;
        Shop = UIManager.transform.GetChild(6).gameObject;

        // 원래 위치 저장 (다시 열었을 때 위치 고정하도록)
        CharacterOriginalPosition = Character.GetComponent<RectTransform>().anchoredPosition;
        SkillOriginalPosition = Skill.GetComponent<RectTransform>().anchoredPosition;
        EquipmentOriginalPosition = Equipment.GetComponent<RectTransform>().anchoredPosition;
        InventoryOriginalPosition = Inventory.GetComponent<RectTransform>().anchoredPosition;
        QuestOriginalPosition = Quest.GetComponent<RectTransform>().anchoredPosition;
        ShopOriginalPosition = Shop.GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnInventory() { // 인벤토리 창 열기 (I키)
        if (Inventory == null || !PlayerManager.CanInput) {
            return;
        }
        if (Inventory.activeSelf) {
            Inventory.SetActive(false);
        }
        else {
            Inventory.SetActive(true);
            Inventory.GetComponent<RectTransform>().anchoredPosition = InventoryOriginalPosition; // 원래 위치로 복원
            
            if (InventoryButtonObject == null) { // 닫기 버튼 연결
                InventoryButtonObject = GameObject.Find("Inventory Close Button");
                InventoryButton = InventoryButtonObject.GetComponent<Button>();
                InventoryButton.onClick.AddListener(OnInventory);
            }
        }
    }
    public void OnCharacter() { // 캐릭터 창 열기 (C키)
        if (Character == null || !PlayerManager.CanInput) {
            return;
        }
        if (Character.activeSelf) {
            Character.SetActive(false);
        }
        else {
            Character.SetActive(true);
            Character.GetComponent<RectTransform>().anchoredPosition = CharacterOriginalPosition; // 원래 위치로 복원

            if (CharacterButtonObject == null) { // 닫기 버튼 연결
                CharacterButtonObject = GameObject.Find("Character Close Button");
                CharacterButton = CharacterButtonObject.GetComponent<Button>();
                CharacterButton.onClick.AddListener(OnCharacter);
                PlayerLevelUpController.ConnectButton();
            }
        }
    }

    public void OnQuest() { // 퀘스트 창 열기 (J키)
        if (Quest == null || !PlayerManager.CanInput) {
            return;
        }
        if (Quest.activeSelf) {
            Quest.SetActive(false);
        }
        else {
            Quest.SetActive(true);
            Quest.GetComponent<RectTransform>().anchoredPosition = QuestOriginalPosition; // 원래 위치로 복원

            if (QuestButtonObject == null) { // 닫기 버튼 연결
                QuestButtonObject = GameObject.Find("Quest Close Button");
                QuestButton = QuestButtonObject.GetComponent<Button>();
                QuestButton.onClick.AddListener(OnQuest);
            }
        }
    }

    public void OnSkill() { // 스킬 창 열기 (K키)
        if (Skill == null || !PlayerManager.CanInput) {
            return;
        }
        if (Skill.activeSelf) {
            Skill.SetActive(false);
        }
        else {
            Skill.SetActive(true);
            Skill.GetComponent<RectTransform>().anchoredPosition = SkillOriginalPosition; // 원래 위치로 복원

            if (SkillButtonObject == null) { // 닫기 버튼 연결
                SkillButtonObject = GameObject.Find("Skill Close Button");
                SkillButton = SkillButtonObject.GetComponent<Button>();
                SkillButton.onClick.AddListener(OnSkill);
            }
        }
    }

    public void OnEquipment() { // 장비 창 열기 (P키)
        if (Equipment == null || !PlayerManager.CanInput) {
            return;
        }
        if (Equipment.activeSelf) {
            Equipment.SetActive(false);
        }
        else {
            Equipment.SetActive(true);
            Equipment.GetComponent<RectTransform>().anchoredPosition = EquipmentOriginalPosition; // 원래 위치로 복원
            
            if (EquipmentButtonObject == null) { // 닫기 버튼 연결
                EquipmentButtonObject = GameObject.Find("Equipment Close Button");
                EquipmentButton = EquipmentButtonObject.GetComponent<Button>();
                EquipmentButton.onClick.AddListener(OnEquipment);
            }
        }
    }

    public void SetDialogueButton() { // 다이얼로그 창 버튼 설정
        DialogueButtonObject = GameObject.Find("Dialogue Close Button");
        DialogueButton = DialogueButtonObject.GetComponent<Button>();
        DialogueButton.onClick.AddListener(CloseDialogue);
    }
    public void CloseDialogue() { // 다이얼로그 창 닫기
        if (Dialogue.activeSelf) {
            Dialogue.SetActive(false);
        }
    }

    public void OpenShop() { // 상점 창 열기
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
        
        Shop.GetComponent<RectTransform>().anchoredPosition = ShopOriginalPosition; // 원래 위치로 복원
        if (ShopButtonObject == null) {
            ShopButtonObject = GameObject.Find("Shop Close Button");
            ShopButton = ShopButtonObject.GetComponent<Button>();
            ShopButton.onClick.AddListener(CloseShop);
        }
    }

    public void CloseShop() { // 상점 창 닫기
        if (Shop.activeSelf) {
            Shop.SetActive(false);
        }
        if (Inventory.activeSelf) {
            Inventory.SetActive(false);
        }
    }
}
