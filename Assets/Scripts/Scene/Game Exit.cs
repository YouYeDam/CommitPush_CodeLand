using UnityEngine;
using UnityEngine.SceneManagement;

public class GameExit : MonoBehaviour
{
    public void ExitGame() {
        // Unity 에디터에서 실행 중이라면
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 게임에서 실행 중이라면
        Application.Quit();
        #endif
    }
}
