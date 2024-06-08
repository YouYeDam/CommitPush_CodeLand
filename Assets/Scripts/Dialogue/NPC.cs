using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class QuestDialogue
{
    public Dialogue PreQuestDialogue;
    public Dialogue InProgressDialogue;
    public Dialogue ReadyToCompleteDialogue;
    public Dialogue PostQuestDialogue;
}

public class NPC : MonoBehaviour
{
    public List<QuestData> QuestsToGive; // NPC가 줄 퀘스트 데이터 리스트
    public List<QuestDialogue> QuestDialogues; // 퀘스트 별 대화 데이터 리스트

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

    private QuestManager questManager;
    public int CurrentQuestIndex;

    void Start()
    {
        UIManager = GameObject.Find("UIManager");
        questManager = FindObjectOfType<QuestManager>();
        CurrentQuestIndex = questManager.NpcQuestState.GetQuestIndex(NPCName);
        DisplayNPCNameInfo();
        DisplayNPCJobInfo();
        UpdateQuestStatus();
    }

    void Update()
    {
        SetNPCNameInfoPosition();
        SetNPCJobInfoPosition();
        SetNPCQuestStatusPosition();
        UpdateQuestStatus();
        CurrentQuestIndex = questManager.NpcQuestState.GetQuestIndex(NPCName);
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
        if (questManager != null && CurrentQuestIndex < QuestsToGive.Count && CurrentQuestIndex < QuestDialogues.Count)
        {
            Quest quest = questManager.GetQuestByTitle(QuestsToGive[CurrentQuestIndex].Title);
            QuestDialogue questDialogue = QuestDialogues[CurrentQuestIndex];

            if (CurrentQuestIndex > 0)
            {
                Quest previousQuest = questManager.GetQuestByTitle(QuestsToGive[CurrentQuestIndex - 1].Title);
                QuestDialogue previousQuestDialogue = QuestDialogues[CurrentQuestIndex - 1];

                if (previousQuest != null && previousQuest.IsCompleted)
                {
                    // 다음 퀘스트의 요구 레벨을 만족하지 못하는 경우
                    if (questManager.PlayerStatus.PlayerLevel < quest.RequiredLevel)
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
                else if (questManager.IsQuestActive(QuestsToGive[CurrentQuestIndex].Title))
                {
                    return questDialogue.InProgressDialogue;
                }
                else if (questManager.PlayerStatus.PlayerLevel < quest.RequiredLevel)
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
        if (questManager != null && CurrentQuestIndex < QuestsToGive.Count)
        {
            Quest quest = questManager.GetQuestByTitle(QuestsToGive[CurrentQuestIndex].Title);
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
                else if (questManager.IsQuestActive(QuestsToGive[CurrentQuestIndex].Title))
                {
                    DisplayQuestStatus("?", Color.gray);
                }
                else if (questManager.PlayerStatus.PlayerLevel >= quest.RequiredLevel)
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
