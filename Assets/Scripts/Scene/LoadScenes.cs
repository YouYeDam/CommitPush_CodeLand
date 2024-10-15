using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public void LoadScene(string SceneNameToLoad) { //씬 로드
        SceneManager.LoadScene(SceneNameToLoad);
    }
}
