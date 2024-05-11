using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
    {
    PortalInfo PortalInfo;
    UIManager UIManager;
    PlayerManager PlayerManager;
    string LoadSceneName;
    string ConnectPortalName;
    bool CanUsePortal = false;

    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = FindObjectOfType<UIManager>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Portal") {
            CanUsePortal = true;
            PortalInfo = other.gameObject.GetComponent<PortalInfo>();
            LoadSceneName = PortalInfo.LoadSceneName;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Portal") {
            CanUsePortal = false;
            PortalInfo = null;
            LoadSceneName = null;
        }
    }

    void OnPortal() {
        if (CanUsePortal) {
            UIManager.DestroyAllTempInfo();
            if (LoadSceneName != null) {
                ConnectPortalName = PortalInfo.ConnectPortalName;
                PlayerManager.SetConnectPortalName(ConnectPortalName);
                SceneManager.LoadScene(LoadSceneName);
            }
        }
    }
}
