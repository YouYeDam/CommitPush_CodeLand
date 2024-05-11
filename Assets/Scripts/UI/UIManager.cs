using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject StatusBar;
    
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Start() {
        StatusBar = gameObject.transform.GetChild(3).gameObject;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int UINum = FindObjectsOfType<PlayerManager>().Length;
        string currentSceneName = scene.name;

        if (UINum > 1 || currentSceneName == "Main Menu Scene")
        {
            Destroy(gameObject);
        }
        else
        {
            // 씬이 로드될 때마다 Main Camera를 찾아서 Canvas의 Render Camera로 설정.
            Camera MainCamera = Camera.main;
            if (MainCamera != null)
            {
                Canvas Canvas = GetComponent<Canvas>();
                if (Canvas != null)
                {
                    Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    Canvas.worldCamera = MainCamera;
                }
            }
        }

        // 씬이 로드될 때 플레이어 인풋이 막힌 상황이면 Player Status Bar도 비활성화
        GameObject StopPlayerInput = GameObject.FindWithTag("StopPlayerInput");
        if (StopPlayerInput != null) {
            StatusBar.SetActive(false);
        }
        else {
            StatusBar.SetActive(true);
        }
    }

    public void DestroyAllTempInfo()
    {
        GameObject[] tempInfos = GameObject.FindGameObjectsWithTag("TempInfo");
        foreach (GameObject tempInfo in tempInfos)
        {
            Destroy(tempInfo);
        }
    }
}