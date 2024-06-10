using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // 파일 입출력을 위해 추가
using UnityEngine.SceneManagement;
using Unity.VisualScripting; // 씬 관리를 위해 추가

public class MainButtonManager : MonoBehaviour
{
    // public GameObject GameDataSlot;
    // private GameObject gameDataSlotInstance = null;
    public Button ContinueGameBtn;
    public Button NewGameBtn;
    public Button LoadGameBtn;
    SaveManager saveManager;
    // Start is called before the first frame update
    void Awake()
    {
        this.saveManager = FindObjectOfType<SaveManager>();
    }
    void Start()
    {
        ContinueGameBtn.onClick.AddListener(LoadSavedScene);
        // LoadGameBtn.onClick.AddListener(ActivateSlot);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 게임 데이터 여러개 저장할 거면 아래 코드들 사용해야함.
    // void ActivateSlot()
    // {
    //     if (gameDataSlotInstance != null)
    //     {
    //         return;
    //     }

    //     if (GameDataSlot != null)
    //     {
    //         Canvas canvas = GameObject.FindObjectOfType<Canvas>();
    //         gameDataSlotInstance = Instantiate(GameDataSlot, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
    //         gameDataSlotInstance.SetActive(true);
    //         Button[] SlotQuitBtn = gameDataSlotInstance.GetComponentsInChildren<Button>();
    //         SlotQuitBtn[0].onClick.AddListener(InactivateSlot);

    //         Debug.Log("fuck yo u " + SlotQuitBtn );
    //     }
    // }

    // void InactivateSlot()
    // {
    //     Debug.Log("inactive!!!");
    //     if (gameDataSlotInstance != null)
    //     {
    //         Destroy(gameDataSlotInstance);
    //         gameDataSlotInstance = null;
    //     }
    // }

    // void LoadPlayerProgress()
    // {
    //     string path = Path.Combine(Application.persistentDataPath, "playerData.json");
    //     if (File.Exists(path))
    //     {
    //         string json = File.ReadAllText(path);
    //         PlayerData data = JsonUtility.FromJson<PlayerData>(json);
    //         Debug.Log(data);
    //         // Do something with the loaded player data
    //     }
    //     else
    //     {
    //         Debug.LogError("저장된 플레이어 진행 상황이 없습니다.");
    //     }
    // }

    void LoadSavedScene()
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log(data);
            // 게임 시작을 위한 씬 로드
            Debug.Log(data.sceneName);
            SceneManager.LoadScene(data.sceneName);
            saveManager.InstantiateOnSavePoint(data);

        }
        else
        {
            Debug.LogError("저장된 플레이어 진행 상황이 없습니다.");
        }
    }

}
