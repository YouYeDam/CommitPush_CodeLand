using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject StatusBar;
    [SerializeField] GameObject ItemQuickSlot;
    [SerializeField] GameObject SkillQuickSlot;

    DialogueController DialogueController;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DialogueController = gameObject.transform.GetChild(9).gameObject.GetComponent<DialogueController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Start() {
        StatusBar = gameObject.transform.GetChild(8).gameObject;
        ItemQuickSlot = gameObject.transform.GetChild(0).gameObject;
        SkillQuickSlot = gameObject.transform.GetChild(1).gameObject;
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        int UINum = FindObjectsOfType<UIManager>().Length;
        string currentSceneName = scene.name;

        if (UINum > 1 || currentSceneName == "Main Menu Scene") { // UI Manager가 2개 이상 되지 않도록 설정
            Destroy(gameObject);
        }
        else {  // 씬이 로드될 때마다 Main Camera를 찾아서 Canvas의 Render Camera로 설정
            Camera MainCamera = Camera.main;
            if (MainCamera != null) {
                Canvas Canvas = GetComponent<Canvas>();

                if (Canvas != null) {
                    Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    Canvas.worldCamera = MainCamera;
                }
            }
            
        }

        if (DialogueController != null && DialogueController.DialogueBase.activeSelf) {
            DialogueController.EndDialogue();
        }

        // 씬이 로드될 때 플레이어 인풋이 막힌 상황이면 스테이터스바, 퀵슬롯들도 비활성화
        GameObject StopPlayerInput = GameObject.FindWithTag("StopPlayerInput");

        if (StopPlayerInput != null) {
            StatusBar.SetActive(false);
            ItemQuickSlot.SetActive(false);
            SkillQuickSlot.SetActive(false);
        }
        else {
            StatusBar.SetActive(true);
            ItemQuickSlot.SetActive(true);
            SkillQuickSlot.SetActive(true);
        }
    }

    public void DestroyAllTempInfo() { // 임시 생성된 정보(몬스터 체력바, NPC 이름 등) 삭제
        GameObject[] tempInfos = GameObject.FindGameObjectsWithTag("TempInfo");

        foreach (GameObject tempInfo in tempInfos) {
            Destroy(tempInfo);
        }
    }
}