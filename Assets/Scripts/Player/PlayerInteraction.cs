using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
PortalInfo PortalInfo;
string LoadSceneName;
bool CanUsePortal = false;

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
            SceneManager.LoadScene(LoadSceneName);
        }
    }
}
