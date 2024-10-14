using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TMP_Text SpeakerNameText;
    public TMP_Text DialogueText;
    public GameObject DialogueBase;
    public TextMeshProUGUI ButtonText;
    public Image SpeakerImage;
    public Image PlayerImage;

    Dialogue Dialogue;
    int CurrentNodeIndex = 0;
    PlayerStatus PlayerStatus;
    Character Character;
    QuestManager QuestManager;
    NPC CurrentNPC;

    void Start() {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        Character = FindObjectOfType<Character>();
        QuestManager = FindObjectOfType<QuestManager>();
    }

    public void StartDialogue(Dialogue dialogue, NPC npc) { // 대화 시작
        this.Dialogue = dialogue;
        this.CurrentNPC = npc; // 대화 중인 NPC 설정
        DialogueBase.SetActive(true);
        CurrentNodeIndex = 0;  // 대화 시작 시 첫 노드 인덱스를 0으로 초기화
        DisplayNode();
    }

    void DisplayNode() { // 대화 진행
        if (CurrentNodeIndex < Dialogue.nodes.Length) { // 대화가 안끝난 경우
            DialogueNode node = Dialogue.nodes[CurrentNodeIndex]; // 현재 대화 노드
            SpeakerNameText.text = node.SpeakerName;
            DialogueText.text = node.DialogueText;
            SpeakerImage.sprite = node.SpeakerImage;

            if (SpeakerNameText.text == "플레이어") { // 플레이어가 말할 차례라면 플레이어로 변경
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
    
    public void OnNextButtonClicked() { // 사용자가 다음 버튼을 클릭할 때 다음 노드 표시
        DisplayNode();
    }

    void CheckQuestStatus() { // 해당 NPC의 퀘스트 진행 여부 확인 후 진행 상황에 맞게 처리
        if (CurrentNPC != null && QuestManager != null) {
            int CurrentQuestIndex = QuestManager.NpcQuestState.GetQuestIndex(CurrentNPC.NPCName); // 현재 퀘스트 인덱스

            if (CurrentQuestIndex < CurrentNPC.QuestsToGive.Count) { // NPC에게 받을 수 있거나 수행하고 있는 퀘스트가 존재한다면
                string QuestTitle = CurrentNPC.QuestsToGive[CurrentQuestIndex].Title;

                if (!string.IsNullOrEmpty(QuestTitle)) {
                    Quest quest = QuestManager.GetQuestByTitle(QuestTitle);

                    if (quest != null) {
                        if (QuestManager.IsQuestActive(QuestTitle)) { // 퀘스트 중 조건을 만족했다면 퀘스트 완료
                            if (quest.Objectives.TrueForAll(obj => obj.IsComplete())) {
                                QuestManager.CompleteQuest(QuestTitle);
                            }
                        }
                        else { // 퀘스트를 시작하지 않은 경우 퀘스트 시작
                            QuestManager.StartQuest(QuestTitle);
                        }
                    }
                }
            }
        }
        EndDialogue(); // 대화 종료
    }

    public void EndDialogue() { // 다이얼로그 창 초기화
        SpeakerNameText.text = "";
        DialogueText.text = "";
        ButtonText.color = Color.white;
        DialogueBase.SetActive(false);
    }
}
