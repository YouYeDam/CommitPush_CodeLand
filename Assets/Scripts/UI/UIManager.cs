using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            // 씬이 로드될 때마다 Main Camera를 찾아서 Canvas의 Render Camera로 설정합니다.
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
    }
}