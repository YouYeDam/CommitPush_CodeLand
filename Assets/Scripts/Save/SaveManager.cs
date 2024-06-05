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
    public float QSkillCoolDown, WSkillCoolDown, ESkillCoolDown, RSkillCoolDown, SSkillCoolDown, DSkillCoolDown;

    // 장비 저장 데이터
    public Item[] equipments = new Item[8];
}

public class SaveManager : MonoBehaviour
{
    public GameObject playerPrefab; // 플레이어 프리팹 참조
    public GameObject uiManagerPrefab; // UIManager 프리팹 참조
    public GameObject questManagerPrefab; // UIManager 프리팹 참조
    public GameObject PaueseMenu;
    private GameObject PauseMenuInstance = null;
    // public GameObject GameDataSlot;
    public GameObject playerObject;
    public GameObject questManagerObject;
    QuestManager questManager;
    UIManager uiManager;
    PlayerStatus playerStatus;
    GameObject uiManagerObject;
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
        questManagerObject = null;
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
            uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
            if (playerObject != null)
            {
                SavePlayerProgress(SceneManager.GetActiveScene().name, uiManager, playerObject, questManagerObject); 
                Debug.Log("플레이어 진행 상황이 저장되었습니다.");
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

    public void SavePlayerProgress(string currentSceneName, UIManager uIManager, GameObject playerObject, GameObject questManager)
    {
        Slot[] slots = uiManager.GetComponentsInChildren<Slot>(true); // 슬롯은 기본적으로 비활성화 돼있기 때문에 (true) 값을 설정. 모든 슬롯을 가져옴.
        EquipmentSlot[] equipmentSlots = uiManager.GetComponentsInChildren<EquipmentSlot>(true); 
        SkillSlot[] skillSlots = uiManager.GetComponentsInChildren<SkillSlot>(true);
        RuntimeAnimatorController currentAniController = playerObject.GetComponent<Animator>().runtimeAnimatorController;
        PlayerStatus playerStatus = playerObject.GetComponent<PlayerStatus>();
        PlayerMoney playerMoney = playerObject.GetComponent<PlayerMoney>();
        Vector3 playerPosition = playerObject.transform.position;
        PlayerSkills playerSkills = playerObject.GetComponent<PlayerSkills>();
        SkillQuickSlot[] skillQuickSlots = uIManager.GetComponentsInChildren<SkillQuickSlot>();
        ItemQuickSlot[] itemQuickSlots = uIManager.GetComponentsInChildren<ItemQuickSlot>();
        
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
            QSkill = playerSkills.QSkill,
            WSkill = playerSkills.WSkill,
            ESkill = playerSkills.ESkill,
            RSkill = playerSkills.RSkill,
            SSkill = playerSkills.SSkill,
            DSkill = playerSkills.DSkill,
            QSkillCoolDown = playerSkills.QSkillCoolDown,
            WSkillCoolDown = playerSkills.WSkillCoolDown,
            ESkillCoolDown = playerSkills.ESkillCoolDown,
            RSkillCoolDown = playerSkills.RSkillCoolDown,
            SSkillCoolDown = playerSkills.SSkillCoolDown,
            DSkillCoolDown = playerSkills.DSkillCoolDown,
            
        };
        
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
            Debug.Log("log1112321: "+equipmentSlots[i].Item);
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
        };
        // 정리된 배열들을 data에 저장
        data.quickItems = items;
        data.quickItemCounts = itemCounts;
        data.itemSlotReferencesIdx = referSlotsIdx;
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
            playerStatus = playerObject.GetComponent<PlayerStatus>();
            playerStatus.PlayerLevel = data.PlayerLevel;
            playerStatus.PlayerMaxHP = data.PlayerMaxHP;
            playerStatus.PlayerMaxMP = data.PlayerMaxMP;
            
            playerStatus.PlayerMaxEXP = data.PlayerMaxEXP;
            playerStatus.PlayerClass = data.PlayerClass;
            playerStatus.PlayerName = data.PlayerName;
            playerStatus.PlayerATK = data.PlayerATK;
            playerStatus.PlayerDEF = data.PlayerDEF;
            playerStatus.PlayerAP = data.PlayerAP;
            playerStatus.PlayerCrit = data.PlayerCrit;
            playerStatus.LevelUpPoint = data.LevelUpPoint;
            playerStatus.IsLoaded = true;

            playerObject.GetComponent<PlayerMoney>().Bit = data.Bit;
            playerObject.GetComponent<PlayerMoney>().Snippet = data.Snippet;

            playerObject.GetComponent<PlayerSkills>().QSkill = data.QSkill;
            playerObject.GetComponent<PlayerSkills>().WSkill = data.WSkill;
            playerObject.GetComponent<PlayerSkills>().ESkill = data.ESkill;
            playerObject.GetComponent<PlayerSkills>().RSkill = data.RSkill;
            playerObject.GetComponent<PlayerSkills>().SSkill = data.SSkill;
            playerObject.GetComponent<PlayerSkills>().DSkill = data.DSkill;

            playerObject.GetComponent<PlayerSkills>().QSkillCoolDown = data.QSkillCoolDown;
            playerObject.GetComponent<PlayerSkills>().WSkillCoolDown = data.WSkillCoolDown;
            playerObject.GetComponent<PlayerSkills>().ESkillCoolDown = data.ESkillCoolDown;
            playerObject.GetComponent<PlayerSkills>().RSkillCoolDown = data.RSkillCoolDown;
            playerObject.GetComponent<PlayerSkills>().SSkillCoolDown = data.SSkillCoolDown;
            playerObject.GetComponent<PlayerSkills>().DSkillCoolDown = data.DSkillCoolDown;

            // set current values.
            playerStatus.PlayerCurrentMP = data.PlayerCurrentMP;
            playerStatus.PlayerCurrentHP = data.PlayerCurrentHP;
            Debug.Log("log444: "+ data.PlayerCurrentHP);
            Debug.Log("log444: "+ playerStatus.PlayerCurrentHP);
            playerStatus.PlayerCurrentEXP = data.PlayerCurrentEXP;

            
        }

        if (uiManagerObject == null)
        {
            // LevelUpPoint
            uiManagerObject = Instantiate(uiManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            uiManager = uiManagerObject.GetComponent<UIManager>();
            uiManagerObject.name = uiManagerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(uiManagerObject); // 씬 전환 시 파괴되지 않도록 설정
            
            InitialEquipmentItems();
            LoadEquipments(data);

            LoadItems(data);    
            LoadQuickItems(data);
            LoadSkills(data);
            LoadQuickSkills(data);
            ShopSlotInit();
            
            // 이 지점에서 초기화 시켜줘야 오류가 없음.
            playerObject.GetComponent<PlayerLevelUpController>().Character = uiManagerObject.GetComponentInChildren<Character>(true).gameObject;
            ////////////
            playerStatus.PlayerCurrentMP = data.PlayerCurrentMP;
            playerStatus.PlayerCurrentHP = data.PlayerCurrentHP;
            Debug.Log("log444: "+ data.PlayerCurrentHP);
            Debug.Log("log444: "+ playerStatus.PlayerCurrentHP);
            playerStatus.PlayerCurrentEXP = data.PlayerCurrentEXP;
        }

        // questmanager initialize
        if (questManagerObject == null){
            questManagerObject = Instantiate(questManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            questManager = FindObjectOfType<QuestManager>();
            questManagerObject.name = questManagerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(questManagerObject); // 씬 전환 시 파괴되지 않도록 설정
        }

        // name tag initialize
        if (uiManagerObject != null && data.PlayerNameInfo != null && data.PlayerNameInfoInstance == null)
        {
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance = Instantiate(data.PlayerNameInfo, uiManagerObject.transform);
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance.GetComponent<TMP_Text>().text = data.PlayerName;
            playerStatus.PlayerCurrentMP = data.PlayerCurrentMP;
            playerStatus.PlayerCurrentHP = data.PlayerCurrentHP;
            Debug.Log("log444: "+ data.PlayerCurrentHP);
            Debug.Log("log444: "+ playerStatus.PlayerCurrentHP);
            playerStatus.PlayerCurrentEXP = data.PlayerCurrentEXP;
        }
        
        // {
        //     StatusBar statusBar = uiManager.GetComponent<StatusBar>();
        //     statusBar.PlayerStatus = playerStatus;
        //     Debug.Log("log4441: " + playerStatus.PlayerCurrentHP);
        //     if (playerStatus.PlayerCurrentHP > 0)
        //         statusBar.HPMeter.fillAmount = (float)playerStatus.PlayerCurrentHP / playerStatus.PlayerMaxHP;
        //     else
        //         statusBar.HPMeter.fillAmount = 0;

        //     if (playerStatus.PlayerCurrentMP > 0)
        //         statusBar.MPMeter.fillAmount = (float)playerStatus.PlayerCurrentMP / playerStatus.PlayerMaxMP;
        //     else
        //         statusBar.MPMeter.fillAmount = 0;

        //     if (playerStatus.PlayerCurrentEXP > 0)
        //         statusBar.EXPMeter.fillAmount = (float)playerStatus.PlayerCurrentEXP / playerStatus.PlayerMaxEXP;
        //     else
        //         statusBar.EXPMeter.fillAmount = 0;
                
        //     Debug.Log("log4442: " +playerStatus.PlayerCurrentHP);
        //     statusBar.HPDisplay.text = playerStatus.PlayerCurrentHP + " / " + playerStatus.PlayerMaxHP;
        //     statusBar.MPDisplay.text = playerStatus.PlayerCurrentMP + " / " + playerStatus.PlayerMaxMP;
        //     statusBar.EXPDisplay.text = playerStatus.PlayerCurrentEXP + " / " + playerStatus.PlayerMaxEXP;
        //     statusBar.PlayerLV.text = "LV." + playerStatus.PlayerLevel + " " + playerStatus.PlayerClass;
        //     statusBar.PlayerName.text = playerStatus.PlayerName;
        // }
    }

    private void InitialEquipmentItems(){
        EquipmentItem[] equipmentItems = uiManager.GetComponentsInChildren<EquipmentItem>();
        for(int i = 0; i < equipmentItems.Length; i++){
            equipmentItems[i].PlayerStatus = playerStatus;
        }
    }

    private void ShopSlotInit(){
        ShopSlot[] shopSlot = uiManager.GetComponentsInChildren<ShopSlot>(true);
        for (int i = 0; i < shopSlot.Length; i++){
            shopSlot[i].QuestManager = questManager;
        }
    }

    private void LoadSkills(PlayerData data){
        for (int i = 0; i < data.skillPrefabs.Length; i++){
            if(data.skillPrefabs[i] != null){
                uiManagerObject.GetComponentsInChildren<SkillSlot>(true)[i].AddSkill(data.skillPrefabs[i]);
            }
        }
    }
    private void LoadQuickSkills(PlayerData data){
        for (int i = 0; i < data.quickSkillPrefabs.Length; i++){
            SkillQuickSlot skillQuickSlot = uiManagerObject.GetComponentsInChildren<SkillQuickSlot>(true)[i];
            skillQuickSlot.PlayerSkills = playerObject.GetComponent<PlayerSkills>();
            SkillSlot refer_slot = null;
            if(data.quickSkillPrefabs[i] != null){
                refer_slot = uiManagerObject.GetComponentsInChildren<SkillSlot>(true)[data.skillSlotReferencesIdx[i]];
                skillQuickSlot.AddSkill(data.quickSkillPrefabs[i], refer_slot); //여기서 그냥 null 값으로 넘어갔구나. slot도 같이 넘겨줘야하네 그럼.
            }
        }
    }

    private void LoadEquipments(PlayerData data)
    {
        
        for (int i = 0; i < data.equipments.Length; i++)
        {
            uiManagerObject.GetComponentsInChildren<EquipmentSlot>(true)[i].Item = null;
            if (data.equipments[i] != null)
            {
                Debug.Log("log999: added" + i);
                uiManagerObject.GetComponentsInChildren<EquipmentSlot>(true)[i].AddItem(data.equipments[i]);
            }
        }
    }

    private void LoadItems(PlayerData data) // data의 아이템이 아이템인지 장비인지 구별해야함. isEquipment 함수를 자료형에 추가하고. 또 장비의 슬롯 인덱스도 저장해야함.
    {
        for (int i = 0; i < data.items.Length; i++)
        {
            if (data.items[i] != null)
            {
                uiManagerObject.GetComponentsInChildren<Slot>(true)[i].AddItem(data.items[i], data.itemCounts[i]);
            }
        }
    }
    private void LoadQuickItems(PlayerData data) // data의 아이템이 아이템인지 장비인지 구별해야함. isEquipment 함수를 자료형에 추가하고. 또 장비의 슬롯 인덱스도 저장해야함.
    {
        for (int i = 0; i < data.quickItems.Length; i++)
        {
            Slot refer_slot = uiManagerObject.GetComponentsInChildren<Slot>(true)[data.itemSlotReferencesIdx[i]];
            refer_slot.ItemToolTip = uiManagerObject.GetComponentInChildren<ItemToolTip>(true);
                Debug.Log("log1232 " + refer_slot.ItemToolTip);

            if (data.quickItems[i] != null)
            {
                uiManagerObject.GetComponentsInChildren<ItemQuickSlot>(true)[i].AddItem(data.quickItems[i], data.quickItemCounts[i], refer_slot);
                
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
        GameObject uiManagerObject = GameObject.FindWithTag("UIManager");
        GameObject questManager = GameObject.FindWithTag("QuestManager");
        GameObject saveManager = GameObject.FindWithTag("SaveManager");
        GameObject player = GameObject.FindWithTag("Player");
        Destroy(soundManager);
        Destroy(uiManagerObject);
        Destroy(questManager);
        Destroy(saveManager);
        Destroy(player);
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
