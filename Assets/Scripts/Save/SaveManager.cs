using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEditor;

[System.Serializable]
public class PlayerData
{
    public float x, y, z; // 플레이어 위치
    public int PlayerLevel;
    public string PlayerName;
    public string PlayerClass;
    public GameObject PlayerNameInfo; // 플레이어 이름 텍스트 프리팹
    public TMP_Text PlayerNameInfoText;
    public GameObject PlayerNameInfoInstance;
    public int PlayerCurrentHP;
    public int PlayerMaxHP;
    public int PlayerCurrentMP;
    public int PlayerMaxMP;
    public int PlayerCurrentEXP;
    public int PlayerMaxEXP;
    public int PlayerATK;
    public int PlayerDEF;
    public float PlayerAP;
    public float PlayerCrit;
    public int LevelUpPoint;
    public int Bit;
    public int Snippet;
    public string sceneName; // 현재 씬 이름
    public GameObject playerPrefab;
    public Animator playerAnimator; // 이거 필요 없었던 거 같은데
    public RuntimeAnimatorController runtimeAnimatorController;
    // 아이템 저장을 위해선 item 과 item count로 구성된 2차원 배열을 저장해야함.
    public Item[] items = new Item[40];
    public GameObject[] skillPrefabs = new GameObject[10];
    public GameObject[] quickSkillPrefabs = new GameObject[6];
    public int[] skillSlotReferencesIdx = new int[6];
    public int[] itemSlotReferencesIdx = new int[4];
    public Item[] quickItems = new Item[4];
    public int[] quickItemCounts = new int[4];
    public int[] itemCounts = new int[40];
    public bool[] isEquipment = new bool[40];
    public GameObject QSkill, WSkill, ESkill, RSkill, SSkill, DSkill;

    // 장비 저장 데이터
    public Item[] equipments = new Item[8];
}

public class SaveManager : MonoBehaviour
{
    public GameObject playerPrefab; // 플레이어 프리팹 참조
    public GameObject uiManagerPrefab; // UIManager 프리팹 참조
    public GameObject PaueseMenu;
    private GameObject PauseMenuInstance = null;
    // public GameObject GameDataSlot;
    PlayerStatus playerStatus;
    PlayerMoney playerMoney;
    public GameObject playerObject;
    GameObject uiManager;
    public Slot[] slots;
    public SkillSlot[] skillSlots;
    public EquipmentSlot[] equipmentSlots;
    
    GameObject Character;
    PlayerUI playerUI;
    public RuntimeAnimatorController currentAniController;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = null;
        currentAniController = null;
        uiManager = null;
        slots = null;
        skillSlots = null;
        equipmentSlots = null;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerObject = GameObject.FindWithTag("Player");
            Debug.Log("log10: Max Hp on instantiated: " + playerObject.GetComponent<PlayerStatus>().PlayerMaxHP);

            uiManager = GameObject.FindWithTag("UIManager");
            UIManager UM = uiManager.GetComponent<UIManager>();

            slots = uiManager.GetComponentsInChildren<Slot>(true); // 슬롯은 기본적으로 비활성화 돼있기 때문에 (true) 값을 설정. 모든 슬롯을 가져옴.
            equipmentSlots = uiManager.GetComponentsInChildren<EquipmentSlot>(true); // 역시 모든 장비 슬롯 가져옴.
            skillSlots = uiManager.GetComponentsInChildren<SkillSlot>(true);
            currentAniController = playerObject.GetComponent<Animator>().runtimeAnimatorController;
            if (playerObject != null)
            {
                Vector3 playerPosition = playerObject.transform.position;
                playerStatus = playerObject.GetComponent<PlayerStatus>();
                playerMoney = playerObject.GetComponent<PlayerMoney>();
                Debug.Log(playerStatus);
                SavePlayerProgress(playerPosition, SceneManager.GetActiveScene().name, playerStatus, currentAniController, playerMoney, slots, equipmentSlots, skillSlots, UM, playerObject); // 이거 그냥 결국 playerObject랑 uiManager 두개로 단순화 할 수 있을텐데. 나중에 보고 하기.
                Debug.Log("플레이어 진행 상황이 저장되었습니다.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData data = LoadPlayerProgress();
            if (data != null)
            {
                string json = JsonUtility.ToJson(data);
                Debug.Log("저장된 플레이어 진행 상황: " + json);
                Debug.Log(playerStatus);
            }
            else
            {
                Debug.Log("저장된 플레이어 진행 상황이 없습니다.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "Main Menu Scene")
            {
                ActivatePauseMenu();
            }
        }
    }

    public void SavePlayerProgress(Vector3 playerPosition, string currentSceneName, PlayerStatus playerStatus, RuntimeAnimatorController currentAniController, PlayerMoney playerMoney, Slot[] slots, EquipmentSlot[] equipmentSlots, SkillSlot[] skillSlots, UIManager uIManager, GameObject playerObject)
    {
        PlayerData data = new PlayerData
        {
            x = playerPosition.x,
            y = playerPosition.y,
            z = playerPosition.z,
            sceneName = currentSceneName, // 현재 씬 이름 저장
            PlayerLevel = playerStatus.PlayerLevel,
            PlayerMaxHP = playerStatus.PlayerMaxHP,
            PlayerCurrentHP = playerStatus.PlayerCurrentHP, // 플레이어 체력 저장
            PlayerMaxMP = playerStatus.PlayerMaxMP,
            PlayerCurrentMP = playerStatus.PlayerCurrentMP,
            PlayerMaxEXP = playerStatus.PlayerMaxEXP,
            PlayerCurrentEXP = playerStatus.PlayerCurrentEXP,
            PlayerName = playerStatus.PlayerName,
            PlayerClass = playerStatus.PlayerClass,
            PlayerNameInfo = playerStatus.PlayerNameInfo,
            PlayerNameInfoText = playerStatus.PlayerNameInfoText,
            PlayerNameInfoInstance = playerStatus.PlayerNameInfoInstance,
            PlayerATK = playerStatus.PlayerATK,
            PlayerDEF = playerStatus.PlayerDEF,
            PlayerAP = playerStatus.PlayerAP,
            PlayerCrit = playerStatus.PlayerCrit,
            LevelUpPoint = playerStatus.LevelUpPoint,
            Bit = playerMoney.Bit,
            Snippet = playerMoney.Snippet,
            runtimeAnimatorController = currentAniController,
            QSkill = playerObject.GetComponent<PlayerSkills>().QSkill,
            WSkill = playerObject.GetComponent<PlayerSkills>().WSkill,
            ESkill = playerObject.GetComponent<PlayerSkills>().ESkill,
            RSkill = playerObject.GetComponent<PlayerSkills>().RSkill,
            SSkill = playerObject.GetComponent<PlayerSkills>().SSkill,
            DSkill = playerObject.GetComponent<PlayerSkills>().DSkill,
        };
        SkillQuickSlot[] skillQuickSlots = uIManager.GetComponentsInChildren<SkillQuickSlot>();
        ItemQuickSlot[] itemQuickSlots = uIManager.GetComponentsInChildren<ItemQuickSlot>();
        SaveItems(slots, data);
        SaveSkills(skillSlots, data);
        SaveQuickSkills(skillQuickSlots, data);
        SaveQuickItems(itemQuickSlots, data);
        SaveEquipments(equipmentSlots, data);

        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(path, json);
        PlayerPrefs.SetString("PlayerProgress", json);
        PlayerPrefs.Save();
    }

    private static void SaveEquipments(EquipmentSlot[] equipmentSlots, PlayerData data)
    {
        Item[] equipments = new Item[equipmentSlots.Length];

        // 각 슬롯의 equipment item을 저장
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipments[i] = equipmentSlots[i].Item;
        };
        // 정리된 배열들을 data에 저장
        data.equipments = equipments;
    }
    private static void SaveQuickSkills(SkillQuickSlot[] skillQuickSlots, PlayerData data){
        GameObject[] skillPrefabs = new GameObject[skillQuickSlots.Length];
        int[] skillSlotReferencesIdx = new int[skillQuickSlots.Length];
        for (int i = 0; i < skillQuickSlots.Length; i++)
        {
            skillPrefabs[i] = skillQuickSlots[i].SkillPrefab;
            if(skillQuickSlots[i].SlotReference != null){
            skillSlotReferencesIdx[i] = skillQuickSlots[i].SlotReference.transform.GetSiblingIndex();}
        };
        // 정리된 배열들을 data에 저장
        data.quickSkillPrefabs = skillPrefabs;
        data.skillSlotReferencesIdx = skillSlotReferencesIdx;
    }

    private static void SaveSkills(SkillSlot[] skillSlots, PlayerData data)
    {
        GameObject[] skillPrefabs = new GameObject[skillSlots.Length];

        // 각 슬롯의 skillprefab을 저장
        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillPrefabs[i] = skillSlots[i].SkillPrefab;
        };
        // 정리된 배열들을 data에 저장
        data.skillPrefabs = skillPrefabs;
    }

    private static void SaveItems(Slot[] slots, PlayerData data)
    {
        Item[] items = new Item[slots.Length];
        int[] itemCounts = new int[slots.Length];

        // 각 슬롯의 Item과 ItemCount를 배열에 저장합니다. 함수를 통해 넘겨받은 슬롯의 아이템들을 배열로 정리
        for (int i = 0; i < slots.Length; i++)
        {
            items[i] = slots[i].Item;
            itemCounts[i] = slots[i].ItemCount;
        };
        // 정리된 배열들을 data에 저장
        data.items = items;
        data.itemCounts = itemCounts;
    }
    private static void SaveQuickItems(ItemQuickSlot[] itemQuickSlots, PlayerData data){
        Item[] items = new Item[itemQuickSlots.Length];
        int[] itemCounts = new int[itemQuickSlots.Length];
        int[] referSlotsIdx = new int[itemQuickSlots.Length];
        for (int i = 0; i < itemQuickSlots.Length; i++)
        {
            items[i] = itemQuickSlots[i].Item;
            itemCounts[i] = itemQuickSlots[i].ItemCount;
            if(itemQuickSlots[i].SlotReference != null){
            referSlotsIdx[i] = itemQuickSlots[i].SlotReference.transform.GetSiblingIndex();}
            Debug.Log("log1112" + referSlotsIdx[i]);
        };
        // 정리된 배열들을 data에 저장
        data.quickItems = items;
        data.quickItemCounts = itemCounts;
        data.itemSlotReferencesIdx = referSlotsIdx;
        Debug.Log("log3332: "+data.itemSlotReferencesIdx[0]);
    }

    public void InstantiateOnSavePoint(PlayerData data)
    {

        if (playerObject == null)
        {
            Debug.Log("Wow");
            playerObject = Instantiate(playerPrefab, new Vector3(data.x, data.y, data.z), Quaternion.identity);
            playerObject.name = playerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(playerObject); // 씬 전환 시 파괴되지 않도록 설정

            playerObject.GetComponent<Animator>().runtimeAnimatorController = data.runtimeAnimatorController;

            playerObject.GetComponent<PlayerStatus>().PlayerLevel = data.PlayerLevel;
            playerObject.GetComponent<PlayerStatus>().PlayerCurrentHP = data.PlayerCurrentHP;
            playerObject.GetComponent<PlayerStatus>().PlayerMaxHP = data.PlayerMaxHP;
            playerObject.GetComponent<PlayerStatus>().PlayerCurrentMP = data.PlayerCurrentMP;
            playerObject.GetComponent<PlayerStatus>().PlayerMaxMP = data.PlayerMaxMP;
            playerObject.GetComponent<PlayerStatus>().PlayerCurrentEXP = data.PlayerCurrentEXP;
            playerObject.GetComponent<PlayerStatus>().PlayerMaxEXP = data.PlayerMaxEXP;
            playerObject.GetComponent<PlayerStatus>().PlayerClass = data.PlayerClass;
            playerObject.GetComponent<PlayerStatus>().PlayerName = data.PlayerName;
            playerObject.GetComponent<PlayerStatus>().PlayerATK = data.PlayerATK;
            playerObject.GetComponent<PlayerStatus>().PlayerDEF = data.PlayerDEF;
            playerObject.GetComponent<PlayerStatus>().PlayerAP = data.PlayerAP;
            playerObject.GetComponent<PlayerStatus>().PlayerCrit = data.PlayerCrit;
            playerObject.GetComponent<PlayerStatus>().LevelUpPoint = data.LevelUpPoint;
            playerObject.GetComponent<PlayerStatus>().IsLoaded = true;

            playerObject.GetComponent<PlayerMoney>().Bit = data.Bit;
            playerObject.GetComponent<PlayerMoney>().Snippet = data.Snippet;
            playerObject.GetComponent<PlayerSkills>().QSkill = data.QSkill;
            playerObject.GetComponent<PlayerSkills>().WSkill = data.WSkill;
            playerObject.GetComponent<PlayerSkills>().ESkill = data.ESkill;
            playerObject.GetComponent<PlayerSkills>().RSkill = data.RSkill;
            playerObject.GetComponent<PlayerSkills>().SSkill = data.SSkill;
            playerObject.GetComponent<PlayerSkills>().DSkill = data.DSkill;
        }

        if (uiManager == null)
        {
            // LevelUpPoint
            uiManager = Instantiate(uiManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            uiManager.name = uiManagerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(uiManager); // 씬 전환 시 파괴되지 않도록 설정
            LoadItems(data);
            LoadQuickItems(data);
            LoadEquipments(data);
            LoadSkills(data);
            LoadQuickSkills(data);
        }

        if (uiManager != null && data.PlayerNameInfo != null && data.PlayerNameInfoInstance == null)
        {
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance = Instantiate(data.PlayerNameInfo, uiManager.transform);
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance.GetComponent<TMP_Text>().text = data.PlayerName;
        }

    }

    private void LoadSkills(PlayerData data){
        for (int i = 0; i < data.skillPrefabs.Length; i++){
            if(data.skillPrefabs[i] != null){
                uiManager.GetComponentsInChildren<SkillSlot>(true)[i].AddSkill(data.skillPrefabs[i]);
            }
        }
    }
    private void LoadQuickSkills(PlayerData data){

        for (int i = 0; i < data.quickSkillPrefabs.Length; i++){
            SkillSlot refer_slot = null;
            if(data.quickSkillPrefabs[i] != null){
                // 아니 왜 제대로 넘겨줘도 안 됨
                Debug.Log("log1212: 1111" + data.quickSkillPrefabs[i] + " " + data.skillSlotReferencesIdx[i]);
                refer_slot = uiManager.GetComponentsInChildren<SkillSlot>(true)[data.skillSlotReferencesIdx[i]];
                uiManager.GetComponentsInChildren<SkillQuickSlot>(true)[i].AddSkill(data.quickSkillPrefabs[i], refer_slot); //여기서 그냥 null 값으로 넘어갔구나. slot도 같이 넘겨줘야하네 그럼.
            }
        }
    }

    private void LoadEquipments(PlayerData data)
    {
        for (int i = 0; i < data.equipments.Length; i++)
        {
            Debug.Log("log999: " + i);
            if (data.equipments[i] != null)
            {
                uiManager.GetComponentsInChildren<EquipmentSlot>(true)[i].AddItem(data.equipments[i]);
            }
        }
    }

    private void LoadItems(PlayerData data) // data의 아이템이 아이템인지 장비인지 구별해야함. isEquipment 함수를 자료형에 추가하고. 또 장비의 슬롯 인덱스도 저장해야함.
    {
        for (int i = 0; i < data.items.Length; i++)
        {
            if (data.items[i] != null)
            {
                uiManager.GetComponentsInChildren<Slot>(true)[i].AddItem(data.items[i], data.itemCounts[i]);
            }
        }
    }
    private void LoadQuickItems(PlayerData data) // data의 아이템이 아이템인지 장비인지 구별해야함. isEquipment 함수를 자료형에 추가하고. 또 장비의 슬롯 인덱스도 저장해야함.
    {
        Debug.Log("log7771" + data.itemSlotReferencesIdx[0]);
        for (int i = 0; i < data.quickItems.Length; i++)
        {
            Slot refer_slot = null;
            if (data.quickItems[i] != null)
            {
                refer_slot = uiManager.GetComponentsInChildren<Slot>(true)[data.itemSlotReferencesIdx[i]];
                uiManager.GetComponentsInChildren<ItemQuickSlot>(true)[i].AddItem(data.quickItems[i], data.quickItemCounts[i], refer_slot);
                Debug.Log("log2223 " + data.itemSlotReferencesIdx[i]);
                Debug.Log("log2223 " + uiManager.GetComponentsInChildren<ItemQuickSlot>(true)[i].SlotReference);
            }
        }
    }

    void ActivatePauseMenu()
    {
        if (PauseMenuInstance != null)
        {
            return;
        }
        if (PaueseMenu != null)
        {
            PauseMenuInstance = Instantiate(PaueseMenu, new Vector3(0, 0, 0), Quaternion.identity);
            PauseMenuInstance.SetActive(true);
            Button[] PauseMenuBtns = PauseMenuInstance.GetComponentsInChildren<Button>();
            PauseMenuBtns[0].onClick.AddListener(InactivatePauseMenu);
            PauseMenuBtns[1].onClick.AddListener(InactivatePauseMenu);
            PauseMenuBtns[2].onClick.AddListener(GoBackHome);
        }
    }

    void GoBackHome()
    {
        GameObject soundManager = GameObject.FindWithTag("SoundManager");
        Destroy(soundManager);
        Debug.Log("log3: sound manager destroied");
        SceneManager.LoadScene("Main Menu Scene");
    }
    void InactivatePauseMenu()
    {
        Debug.Log("inactive!!!");
        if (PauseMenuInstance != null)
        {
            Destroy(PauseMenuInstance);
            PauseMenuInstance = null;
        }
    }

    public PlayerData LoadPlayerProgress()
    {
        if (PlayerPrefs.HasKey("PlayerProgress"))
        {
            string json = PlayerPrefs.GetString("PlayerProgress");
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        return null; // 저장된 데이터가 없는 경우
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 씬의 이름이 특정 씬의 이름과 일치하는지 확인합니다.
        if (scene.name == "Main Menu Scene")
        {
            // 원하는 오브젝트를 파괴합니다.
            Debug.Log("log1: save manager destroy.");
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 이벤트에서 메서드를 제거합니다.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
