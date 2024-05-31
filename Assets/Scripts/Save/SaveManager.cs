using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.Animations;
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
    public int[] itemCounts = new int[40];
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
        equipmentSlots = null;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        // 조건문 처리 잘못하면 계속하기에서 에러 남.
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerObject = GameObject.FindWithTag("Player");
            Debug.Log("log10: Max Hp on instantiated: " + playerObject.GetComponent<PlayerStatus>().PlayerMaxHP);

            uiManager = GameObject.FindWithTag("UIManager");

            slots = uiManager.GetComponentsInChildren<Slot>(true); // 슬롯은 기본적으로 비활성화 돼있기 때문에 (true) 값을 설정.
            equipmentSlots = uiManager.GetComponentsInChildren<EquipmentSlot>(true); // 슬롯은 기본적으로 비활성화 돼있기 때문에 (true) 값을 설정.
            currentAniController = playerObject.GetComponent<Animator>().runtimeAnimatorController;
            if (playerObject != null)
            {
                Vector3 playerPosition = playerObject.transform.position;
                playerStatus = playerObject.GetComponent<PlayerStatus>();
                playerMoney = playerObject.GetComponent<PlayerMoney>();
                Debug.Log(playerStatus);
                SavePlayerProgress(playerPosition, SceneManager.GetActiveScene().name, playerStatus, currentAniController, playerMoney, slots, equipmentSlots);
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

    public void SavePlayerProgress(Vector3 playerPosition, string currentSceneName, PlayerStatus playerStatus, RuntimeAnimatorController currentAniController, PlayerMoney playerMoney, Slot[] slots, EquipmentSlot[] equipmentSlots)
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
        };

        SaveItems(slots, data);
        SaveEquipments(equipmentSlots, data);

        Debug.Log("log5: Max Hp on saving: " + data.PlayerMaxHP);
        // Debug.Log("log1666: slot saved: " + data.slots[0].Item);
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

        // Print the equipments if they are not null
        for (int i = 0; i < data.equipments.Length; i++)
        {
            if (data.equipments[i] != null)
            {
            Debug.Log("Log123: Equipment " + i + ": " + data.equipments[i]);
            }
        }
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
        }

        if (uiManager == null)
        {
            // LevelUpPoint
            uiManager = Instantiate(uiManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            uiManager.name = uiManagerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(uiManager); // 씬 전환 시 파괴되지 않도록 설정
            LoadItems(data);
            LoadEquipments(data);
        }

        if (uiManager != null && data.PlayerNameInfo != null && data.PlayerNameInfoInstance == null)
        {
            Debug.Log(1111);
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance = Instantiate(data.PlayerNameInfo, uiManager.transform);
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance.GetComponent<TMP_Text>().text = data.PlayerName;
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

    private void LoadItems(PlayerData data)
    {
        for (int i = 0; i < data.items.Length; i++)
        {
            Debug.Log("log889: " + i);
            if (data.items[i] != null)
            {
                uiManager.GetComponentsInChildren<Slot>(true)[i].AddItem(data.items[i], data.itemCounts[i]);
                Debug.Log("Item name of " + i + " " + data.items[i].name);
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
