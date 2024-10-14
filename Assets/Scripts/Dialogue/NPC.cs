using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class QuestDialogue
{
    public Dialogue PreQuestDialogue; // 퀘스트 받기 이전 대화
    public Dialogue InProgressDialogue; // 퀘스트 진행중 대화
    public Dialogue ReadyToCompleteDialogue; // 퀘스트 완료 대기 대화
    public Dialogue PostQuestDialogue; // 퀘스트 완료 후 대화
}

public class NPC : MonoBehaviour
{
    public List<QuestData> QuestsToGive; // NPC가 줄 퀘스트 리스트
    public List<QuestDialogue> QuestDialogues; // 퀘스트 별 대화 리스트

    public Dialogue DefaultDialogue; // 퀘스트가 없는 경우 기본 대화
    public GameObject UIManager;
    [SerializeField] public string NPCName;
    [SerializeField] public string NPCJob;
    public GameObject NPCNameInfo;
    public TMP_Text NPCNameInfoText;
    public GameObject NPCNameInfoInstance;
    public float NPCNameInfoPos = 0.7f;
    public GameObject NPCJobInfo;
    public TMP_Text NPCJobInfoText;
    public GameObject NPCJobInfoInstance;
    public GameObject NPCQuestStatus;
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

    void SetNPCNameInfoPosition() { // NPC 이름 위치 갱신
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

    void SetNPCJobInfoPosition() { // NPC 직업 위치 갱신
        if (NPCJobInfoInstance != null) {
            Vector3 newPosition = transform.position + Vector3.down * (NPCNameInfoPos + 0.4f);
            NPCJobInfoInstance.transform.position = newPosition;
            NPCJobInfoInstance.transform.SetAsFirstSibling();
        }
    }

    public Dialogue GetCurrentDialogue() { // 현재 대화 가져오기
        if (questManager != null && CurrentQuestIndex < QuestsToGive.Count && CurrentQuestIndex < QuestDialogues.Count) {
            Quest quest = questManager.GetQuestByTitle(QuestsToGive[CurrentQuestIndex].Title);
            QuestDialogue questDialogue = QuestDialogues[CurrentQuestIndex];

            if (CurrentQuestIndex > 0) { // 퀘스트를 하나 이상 완료한 상태라면
                Quest PreviousQuest = questManager.GetQuestByTitle(QuestsToGive[CurrentQuestIndex - 1].Title);
                QuestDialogue PreviousQuestDialogue = QuestDialogues[CurrentQuestIndex - 1];

                if (PreviousQuest != null && PreviousQuest.IsCompleted) {
                    if (questManager.PlayerStatus.PlayerLevel < quest.RequiredLevel) { // 다음 퀘스트의 요구 레벨을 만족하지 못하는 경우
                        return PreviousQuestDialogue.PostQuestDialogue; // 이전 퀘스트 완료 후 나타나는 대화 출력
                    }
                }
            }

            if (quest != null) { // 퀘스트 진행 상태별로 알맞은 대화 출력
                if (quest.IsCompleted) 
                {
                    return questDialogue.PostQuestDialogue;
                }
                else if (quest.IsReadyToComplete) 
                {
                    return questDialogue.ReadyToCompleteDialogue;
                }
                else if (questManager.IsQuestActive(QuestsToGive[CurrentQuestIndex].Title)) 
                {
                    return questDialogue.InProgressDialogue;
                }
                else if (questManager.PlayerStatus.PlayerLevel < quest.RequiredLevel) // 현재 퀘스트의 요구 레벨을 만족하지 못할 경우 DefaultDialogue 반환
                {
                    return DefaultDialogue; 
                }
                else 
                {
                    return questDialogue.PreQuestDialogue;
                }
            }
        }
        return DefaultDialogue;
    }

    public void UpdateQuestStatus() { // 퀘스트 상태에 맞게 진행 상황 업데이트 (문자부호를 통해)
        if (questManager != null && CurrentQuestIndex < QuestsToGive.Count) {
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

    void DisplayQuestStatus(string status, Color color) { // 퀘스트 상태 시각적으로 전시
        if (UIManager != null && NPCQuestStatus != null) {
            if (NPCQuestStatusInstance == null) {
                NPCQuestStatusInstance = Instantiate(NPCQuestStatus, UIManager.transform);
                NPCQuestStatusText = NPCQuestStatusInstance.GetComponent<TMP_Text>();
            }

            NPCQuestStatusText.text = status;
            NPCQuestStatusText.color = color;
            SetNPCQuestStatusPosition();
        }
    }

    void SetNPCQuestStatusPosition() { // 퀘스트 상태 위치 조정
        if (NPCQuestStatusInstance != null) {
            Vector3 newPosition = transform.position + Vector3.up * NPCQuestStatusPos;
            NPCQuestStatusInstance.transform.position = newPosition;
            NPCQuestStatusInstance.transform.SetAsFirstSibling();
        }
    }

    private void DestroyQuestStatus() { // 퀘스트 진행이 막혔거나 모두 완료한 생태라면 문자 부호 삭제
        if (NPCQuestStatusInstance != null) {
            Destroy(NPCQuestStatusInstance);
            NPCQuestStatusInstance = null;
        }
    }
}
