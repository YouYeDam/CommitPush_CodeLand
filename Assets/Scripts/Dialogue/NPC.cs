using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class QuestDialogue
{
    public Dialogue PreQuestDialogue;
    public Dialogue InProgressDialogue;
    public Dialogue PostQuestDialogue;
    public Dialogue ReadyToCompleteDialogue;
}

public class NPC : MonoBehaviour
{
    public List<QuestData> QuestsToGive; // NPC가 줄 퀘스트 데이터 리스트
    public List<QuestDialogue> QuestDialogues; // 퀘스트 별 대화 데이터 리스트
    public int currentQuestIndex = 0;

    public Dialogue DefaultDialogue; // 퀘스트가 없는 경우 기본 대화
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
    public GameObject NPCQuestStatus; // NPC 퀘스트 상태 텍스트 프리팹
    public TMP_Text NPCQuestStatusText;
    public GameObject NPCQuestStatusInstance;
    public float NPCQuestStatusPos = 1f;
    public bool IsShop = false;
    public bool HavingJob = false;

    private QuestManager QuestManager;

    void Start() {
        UIManager = GameObject.Find("UIManager");
        QuestManager = FindObjectOfType<QuestManager>();
        DisplayNPCNameInfo();
        DisplayNPCJobInfo();
        UpdateQuestStatus();
    }

    void Update() {
        SetNPCNameInfoPosition();
        SetNPCJobInfoPosition();
        SetNPCQuestStatusPosition();
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
    public Dialogue GetCurrentDialogue()
    {
        if (QuestManager != null && currentQuestIndex < QuestsToGive.Count && currentQuestIndex < QuestDialogues.Count)
        {
            Quest quest = QuestManager.GetQuestByTitle(QuestsToGive[currentQuestIndex].Title);
            QuestDialogue questDialogue = QuestDialogues[currentQuestIndex];

            if (currentQuestIndex > 0)
            {
                Quest previousQuest = QuestManager.GetQuestByTitle(QuestsToGive[currentQuestIndex - 1].Title);
                QuestDialogue previousQuestDialogue = QuestDialogues[currentQuestIndex - 1];

                if (previousQuest != null && previousQuest.IsCompleted)
                {
                    // 다음 퀘스트의 요구 레벨을 만족하지 못하는 경우
                    if (QuestManager.PlayerStatus.PlayerLevel < quest.RequiredLevel)
                    {
                        return previousQuestDialogue.PostQuestDialogue;
                    }
                }
            }

            if (quest != null)
            {
                if (quest.IsCompleted)
                {
                    return questDialogue.PostQuestDialogue;
                }
                else if (quest.IsReadyToComplete)
                {
                    return questDialogue.ReadyToCompleteDialogue; // 퀘스트 완료 가능 상태일 때의 대화 반환
                }
                else if (QuestManager.IsQuestActive(QuestsToGive[currentQuestIndex].Title))
                {
                    return questDialogue.InProgressDialogue;
                }
                else if (QuestManager.PlayerStatus.PlayerLevel < quest.RequiredLevel)
                {
                    return DefaultDialogue; // 현재 퀘스트의 요구 레벨을 만족하지 못할 경우 DefaultDialogue 반환
                }
                else
                {
                    return questDialogue.PreQuestDialogue;
                }
            }
        }
        return DefaultDialogue;
    }

    public void UpdateQuestStatus()
    {
        if (QuestManager != null && currentQuestIndex < QuestsToGive.Count)
        {
            Quest quest = QuestManager.GetQuestByTitle(QuestsToGive[currentQuestIndex].Title);
            if (quest != null)
            {
                if (quest.IsCompleted)
                {
                    DestroyQuestStatus();
                }
                else if (quest.IsReadyToComplete)
                {
                    DisplayQuestStatus("?", Color.yellow);
                }
                else if (QuestManager.IsQuestActive(QuestsToGive[currentQuestIndex].Title))
                {
                    DisplayQuestStatus("?", Color.gray);
                }
                else if (QuestManager.PlayerStatus.PlayerLevel >= quest.RequiredLevel)
                {
                    DisplayQuestStatus("!", Color.yellow);
                }
            }
        }
    }

    private void DisplayQuestStatus(string status, Color color)
    {
        if (UIManager != null && NPCQuestStatus != null)
        {
            if (NPCQuestStatusInstance == null)
            {
                NPCQuestStatusInstance = Instantiate(NPCQuestStatus, UIManager.transform);
                NPCQuestStatusText = NPCQuestStatusInstance.GetComponent<TMP_Text>();
            }
            NPCQuestStatusText.text = status;
            NPCQuestStatusText.color = color;
            SetNPCQuestStatusPosition();
        }
    }

    void SetNPCQuestStatusPosition() {
        if (NPCQuestStatusInstance != null) {
            Vector3 newPosition = transform.position + Vector3.up * NPCQuestStatusPos;
            NPCQuestStatusInstance.transform.position = newPosition;
            NPCQuestStatusInstance.transform.SetAsFirstSibling();
        }
    }

    private void DestroyQuestStatus()
    {
        if (NPCQuestStatusInstance != null)
        {
            Destroy(NPCQuestStatusInstance);
            NPCQuestStatusInstance = null;
        }
    }
}
