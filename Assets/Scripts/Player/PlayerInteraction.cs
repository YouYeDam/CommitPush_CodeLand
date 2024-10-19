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
    void OnTriggerEnter2D(Collider2D other) { // 플레이어가 포탈 혹은 터널과 닿을 시 포탈 정보 저장
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

void OnTriggerExit2D(Collider2D other) { 
    if (other.gameObject.tag == "Portal") { // 플레이어가 포탈을 벗어날 시 초기화
        CanUsePortal = false;
        PortalInfo = null;
        LoadSceneName = null;
    }

    if (other.gameObject.tag == "Tunnel") { // 플레이어가 터널을 벗어날 시 초기화
        CanUseTunnel = false;
        PortalInfo = null;
    }
}

    void OnPortal() { // 포탈 이용
        if (CanUsePortal) {
            UIManager.DestroyAllTempInfo(); // 화면에 남아있는 임시 정보(데미지바, NPC이름 등) 전부 삭제
            
            if (LoadSceneName != null) {
                ConnectPortalName = PortalInfo.ConnectPortalName;
                PlayerManager.SetConnectPortalName(ConnectPortalName);
                SceneManager.LoadScene(LoadSceneName);
            }
        }
    }

    void OnTunnel() { // 터널 이용
        if (CanUseTunnel) {
            ConnectPortalName = PortalInfo.ConnectPortalName;
            GameObject ConnectPortal = GameObject.Find(ConnectPortalName);
            Player.transform.position = ConnectPortal.transform.position;
        }
    }
}
