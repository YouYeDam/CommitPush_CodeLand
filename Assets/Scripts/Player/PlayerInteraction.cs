using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
    {
    PortalInfo PortalInfo;
    UIManager UIManager;
    PlayerManager PlayerManager;
    GameObject Player;
    string LoadSceneName;
    string ConnectPortalName;
    bool CanUsePortal = false;
    bool CanUseTunnel = false;

    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        UIManager = FindObjectOfType<UIManager>();
        Player = GameObject.FindGameObjectWithTag("Player");

    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Portal") {
            CanUsePortal = true;
            PortalInfo = other.gameObject.GetComponent<PortalInfo>();
            LoadSceneName = PortalInfo.LoadSceneName;
        }

        if (other.gameObject.tag == "Tunnel") {
            CanUseTunnel = true;
            PortalInfo = other.gameObject.GetComponent<PortalInfo>();
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

    void OnTunnel() {
        if (CanUseTunnel) {
            ConnectPortalName = PortalInfo.ConnectPortalName;
            GameObject ConnectPortal = GameObject.Find(ConnectPortalName);
            Player.transform.position = ConnectPortal.transform.position;
        }
    }
}
