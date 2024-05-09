using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TMP_Text SpeakerNameText;  // 말하는 캐릭터 이름을 표시할 UI 요소
    public TMP_Text DialogueText;  // 대화 내용을 표시할 UI 요소
    public GameObject DialogueBase;
    public Image SpeakerImage; // 말하는 캐릭터 이미지
    public Image PlayerImage;
    private Dialogue Dialogue;  // 현재 대화
    private int CurrentNodeIndex = 0;  // 현재 노드 인덱스
    PlayerStatus PlayerStatus;
    Character Character;
    void Start() {
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        Character = FindObjectOfType<Character>();
    }

    public void StartDialogue(Dialogue Dialogue)
    {
        this.Dialogue = Dialogue;
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
            EndDialogue();  // 대화 노드가 더 이상 없으면 대화 종료
        }
    }
    
    public void OnNextButtonClicked()
    {
        DisplayNode();  // 사용자가 다음 버튼을 클릭할 때 다음 노드 표시
    }

    private void EndDialogue()
    {
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBase.SetActive(false);
    }
}
