using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Dialogue Dialogue;
    public GameObject UIManager;
    [SerializeField] public string NPCName;
    [SerializeField] public string NPCJob;
    public GameObject NPCNameInfo; // NPC 정보 텍스트 프리팹
    public TMP_Text NPCNameInfoText;
    public GameObject NPCNameInfoInstance;
    public float NPCNameInfoPos = 0.7f;
    public GameObject NPCJobInfo; // NPC 직업 텍스트 프리팹
    public TMP_Text NPCJobInfoText;
    public GameObject NPCJobInfoInstance;
    public bool IsShop = false;
    public bool HavingJob = false;
    void Start() {
        UIManager = GameObject.Find("UIManager");
        DisplayNPCNameInfo();
        DisplayNPCJobInfo();
    }
    void Update() {
        SetNPCNameInfoPosition();
        SetNPCJobInfoPosition();
    }
    public void DisplayNPCNameInfo() { // NPC 이름 보이기
        if (UIManager != null && NPCNameInfo != null && NPCNameInfoInstance == null) {
            NPCNameInfoInstance = Instantiate(NPCNameInfo, UIManager.transform); // 캔버스의 자식으로 할당
            NPCNameInfoText = NPCNameInfoInstance.GetComponent<TMP_Text>();
            NPCNameInfoText.text = NPCName;
        }
    }

    void SetNPCNameInfoPosition() {
        if (NPCNameInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * NPCNameInfoPos;
            NPCNameInfoInstance.transform.position = newPosition;
            NPCNameInfoInstance.transform.SetAsFirstSibling();
        }
    }

    public void DisplayNPCJobInfo() { // NPC 직업 보이기
        if (UIManager != null && NPCJobInfo != null && NPCJobInfoInstance == null) {
            NPCJobInfoInstance = Instantiate(NPCJobInfo, UIManager.transform); // 캔버스의 자식으로 할당
            NPCJobInfoText = NPCJobInfoInstance.GetComponent<TMP_Text>();
            NPCJobInfoText.text = NPCJob;
        }
    }

    void SetNPCJobInfoPosition() {
        if (NPCJobInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * (NPCNameInfoPos + 0.4f);
            NPCJobInfoInstance.transform.position = newPosition;
            NPCJobInfoInstance.transform.SetAsFirstSibling();
        }
    }
}