using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public bool CanInput;
    // Awake 메소드 안에 있던 코드는 제거하고 OnSceneLoaded 메소드로 이동합니다.
    public void Awake() {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로드될 때마다 OnSceneLoaded를 호출하도록 등록
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 객체가 파괴될 때 이벤트에서 제거하여 누수 방지
    }

    private void OnSceneLoaded(Scene Scene, LoadSceneMode mode)
    {
        int PlayerNum = FindObjectsOfType<PlayerManager>().Length; // 현재 씬에 있는 PlayerManager의 수를 찾습니다.
        string CurrentSceneName = Scene.name; // 현재 씬의 이름을 가져옵니다.
        if (PlayerNum > 1 || CurrentSceneName == "Main Menu Scene") {
            Destroy(gameObject); // 조건에 해당하면 객체를 파괴합니다.
        }

        GameObject StopPlayerInput = GameObject.FindWithTag("StopPlayerInput");
        if (StopPlayerInput != null) {
            CanInput = false;
        }
        else {
            CanInput = true;
        }
    }
}