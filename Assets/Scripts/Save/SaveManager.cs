using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.Animations;

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
    public string sceneName; // 현재 씬 이름
    public GameObject playerPrefab;
    public Animator playerAnimator; // 이거 필요 없었던 거 같은데
    public RuntimeAnimatorController runtimeAnimatorController;
}

public class SaveManager : MonoBehaviour
{
    public GameObject playerPrefab; // 플레이어 프리팹 참조
    public GameObject uiManagerPrefab; // UIManager 프리팹 참조
    public GameObject PaueseMenu;
    private GameObject PauseMenuInstance = null;
    // public GameObject GameDataSlot;
    PlayerStatus playerStatus;
    public GameObject playerObject;
    GameObject uiManager;
    GameObject Character;
    PlayerUI playerUI;
    public RuntimeAnimatorController currentAniController;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = null;
        currentAniController = null;
        uiManager = null;
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
            uiManager = GameObject.FindWithTag("UIManager");
            currentAniController = playerObject.GetComponent<Animator>().runtimeAnimatorController;
            if (playerObject != null)
            {
                Vector3 playerPosition = playerObject.transform.position;
                playerStatus = playerObject.GetComponent<PlayerStatus>();
                Debug.Log(playerStatus);
                SavePlayerProgress(playerPosition, SceneManager.GetActiveScene().name, playerStatus, currentAniController);
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

    public void SavePlayerProgress(Vector3 playerPosition, string currentSceneName, PlayerStatus playerStatus, RuntimeAnimatorController currentAniController)
    {
        PlayerData data = new PlayerData
        {
            x = playerPosition.x,
            y = playerPosition.y,
            z = playerPosition.z,
            sceneName = currentSceneName, // 현재 씬 이름 저장

            PlayerLevel = playerStatus.PlayerLevel,
            PlayerCurrentHP = playerStatus.PlayerCurrentHP, // 플레이어 체력 저장
            PlayerName = playerStatus.PlayerName,
            PlayerClass = playerStatus.PlayerClass,
            PlayerNameInfo = playerStatus.PlayerNameInfo,
            PlayerNameInfoText = playerStatus.PlayerNameInfoText,
            PlayerNameInfoInstance = playerStatus.PlayerNameInfoInstance,
            runtimeAnimatorController = currentAniController
        };
        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(path, json);
        PlayerPrefs.SetString("PlayerProgress", json);
        PlayerPrefs.Save();
    }

    public void InstantiateOnSavePoint(PlayerData data)
    {

        if (playerObject == null)
        {
            playerObject = Instantiate(playerPrefab, new Vector3(data.x, data.y, data.z), Quaternion.identity);
            playerObject.name = playerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(playerObject); // 씬 전환 시 파괴되지 않도록 설정
            playerObject.GetComponent<PlayerStatus>().PlayerLevel = data.PlayerLevel;
            playerObject.GetComponent<PlayerStatus>().PlayerCurrentHP = data.PlayerCurrentHP;
            playerObject.GetComponent<PlayerStatus>().PlayerClass = data.PlayerClass;
            playerObject.GetComponent<Animator>().runtimeAnimatorController = data.runtimeAnimatorController; // hell yeah
            playerObject.GetComponent<PlayerStatus>().PlayerName = data.PlayerName;
        }
        if (uiManager == null)
        {
            uiManager = Instantiate(uiManagerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            uiManager.name = uiManagerPrefab.name; // "(Clone)" 접미사 제거
            DontDestroyOnLoad(uiManager); // 씬 전환 시 파괴되지 않도록 설정

        }

        if (uiManager != null && data.PlayerNameInfo != null && data.PlayerNameInfoInstance == null)
        {
            Debug.Log(1111);
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance = Instantiate(data.PlayerNameInfo, uiManager.transform);
            playerObject.GetComponent<PlayerStatus>().PlayerNameInfoInstance.GetComponent<TMP_Text>().text = data.PlayerName;
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
