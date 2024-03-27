using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    //씬 로드
    public void LoadScene(string SceneNameToLoad) {
        SceneManager.LoadScene(SceneNameToLoad);
    }
}
