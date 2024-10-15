using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public bool CanInput;
    string ConnectPortalName;
    PlayerInteraction PlayerInteraction;
    public static PlayerManager Instance;
    public bool isMiniMapActive = true;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로드될 때마다 OnSceneLoaded를 호출하도록 등록
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    void Start() {
        PlayerInteraction = GetComponent<PlayerInteraction>();
    }
    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 객체가 파괴될 때 이벤트에서 제거하여 누수 방지
    }

    void OnSceneLoaded(Scene Scene, LoadSceneMode mode) {
        int PlayerNum = FindObjectsOfType<PlayerManager>().Length; // 현재 씬에 있는 PlayerManager의 수 확인.
        string CurrentSceneName = Scene.name; // 현재 씬의 이름을 가져오기.
        if (PlayerNum > 1 || CurrentSceneName == "Main Menu Scene") { // PlayerManager의 수가 1개보다 많거나, 메인 메뉴 씬이라면 PlayerManager의 파괴 
            Destroy(gameObject);
        }

        GameObject StopPlayerInput = GameObject.FindWithTag("StopPlayerInput"); // 플레이어 인풋이 불가능한 씬인지 확인

        if (StopPlayerInput != null) {
            CanInput = false;
        }
        else {
            CanInput = true;
        }

        if (ConnectPortalName != null) {
            SetNewPosition(ConnectPortalName); // 포탈 이용 시 플레이어를 연결된 포탈로 이동
        }
        ConnectPortalName = null;
    }

    public void SetConnectPortalName(string PortalName) {
        ConnectPortalName = PortalName;
    }

    public void SetNewPosition(string ConnectPortalName) { // 플레이어를 연결된 포탈의 위치로 이동
        GameObject ConnectPortal = GameObject.Find(ConnectPortalName);
        GameObject StartPosition = GameObject.Find("StartPosition");
        if (StartPosition != null) {
            Destroy(StartPosition);
        }
        if (ConnectPortal != null) {
            transform.position = ConnectPortal.transform.position;
        }
    }

    public void SetMiniMapState(bool state)
    {
        isMiniMapActive = state;
    }

}