using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TMP_Text SpeakerNameText;  // 말하는 캐릭터 이름을 표시할 UI 요소
    public TMP_Text DialogueText;  // 대화 내용을 표시할 UI 요소
    public GameObject DialogueBase;
    public TextMeshProUGUI ButtonText;
    public Image SpeakerImage; // 말하는 캐릭터 이미지
    public Image PlayerImage;
    private Dialogue Dialogue;  // 현재 대화
    private int CurrentNodeIndex = 0;  // 현재 노드 인덱스
    PlayerStatus PlayerStatus;
    Character Character;
    QuestManager QuestManager;
    NPC CurrentNPC; // 현재 대화 중인 NPC

    void Start() {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        Character = FindObjectOfType<Character>();
        QuestManager = FindObjectOfType<QuestManager>();
    }

    public void StartDialogue(Dialogue dialogue, NPC npc)
    {
        this.Dialogue = dialogue;
        this.CurrentNPC = npc; // 대화 중인 NPC 설정
        DialogueBase.SetActive(true);
        CurrentNodeIndex = 0;  // 대화 시작 시 첫 노드 인덱스를 0으로 초기화
        DisplayNode();
    }

    private void DisplayNode()
    {
        if (CurrentNodeIndex < Dialogue.nodes.Length)
        {
            DialogueNode node = Dialogue.nodes[CurrentNodeIndex];
            SpeakerNameText.text = node.SpeakerName;
            DialogueText.text = node.DialogueText;
            SpeakerImage.sprite = node.SpeakerImage;
            if (SpeakerNameText.text == "플레이어") {
                SpeakerNameText.text = PlayerStatus.PlayerName;
                PlayerImage = Character.CharacterImage;
                SpeakerImage.sprite = PlayerImage.sprite;
            }
            CurrentNodeIndex++;  // 다음 노드로 인덱스 증가
        }
        else
        {
            CheckQuestStatus(); // 퀘스트 상태 확인 후 대화 종료
        }
    }
    
    public void OnNextButtonClicked()
    {
        DisplayNode();  // 사용자가 다음 버튼을 클릭할 때 다음 노드 표시
    }

    private void CheckQuestStatus()
    {
        if (CurrentNPC != null && QuestManager != null)
        {
            // 현재 퀘스트 인덱스 사용
            int currentQuestIndex = QuestManager.NpcQuestState.GetQuestIndex(CurrentNPC.NPCName);
            if (currentQuestIndex < CurrentNPC.QuestsToGive.Count)
            {
                string questTitle = CurrentNPC.QuestsToGive[currentQuestIndex].Title;
                if (!string.IsNullOrEmpty(questTitle))
                {
                    Quest quest = QuestManager.GetQuestByTitle(questTitle);
                    if (quest != null)
                    {
                        if (QuestManager.IsQuestActive(questTitle))
                        {
                            // 퀘스트 중 조건을 만족했다면 퀘스트 완료
                            if (quest.Objectives.TrueForAll(obj => obj.IsComplete()))
                            {
                                QuestManager.CompleteQuest(questTitle);
                            }
                        }
                        else
                        {
                            // 퀘스트를 시작하지 않은 경우 퀘스트 시작
                            QuestManager.StartQuest(questTitle);
                        }
                    }
                }
            }
        }

        EndDialogue(); // 대화 종료
    }

    public void EndDialogue()
    {
        SpeakerNameText.text = "";
        DialogueText.text = "";
        ButtonText.color = Color.white;
        DialogueBase.SetActive(false);
    }
}
