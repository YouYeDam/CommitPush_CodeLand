using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Dialogue Dialogue;
    public GameObject UIManager;
    [SerializeField] public string NPCName;
    public GameObject NPCNameInfo; // NPC 정보 텍스트 프리팹
    public TMP_Text NPCNameInfoText;
    public GameObject NPCNameInfoInstance;
    public float NPCNameInfoPos = 0.7f;
    
    void Start() {
        UIManager = GameObject.Find("UIManager");
        DisplayNPCNameInfo();
    }

    void Update() {
        UpdateNPCNameInfo();
    }

    public void DisplayNPCNameInfo() { // NPC 이름 보이기
        if (UIManager != null && NPCNameInfo != null && NPCNameInfoInstance == null) {
            NPCNameInfoInstance = Instantiate(NPCNameInfo, UIManager.transform); // 캔버스의 자식으로 할당
            NPCNameInfoText = NPCNameInfoInstance.GetComponent<TMP_Text>();
            NPCNameInfoText.text = NPCName;
        }
    }

    void UpdateNPCNameInfo() {
        if (NPCNameInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * NPCNameInfoPos;
            NPCNameInfoInstance.transform.position = newPosition;
            NPCNameInfoInstance.transform.SetAsFirstSibling();
        }
    }
}