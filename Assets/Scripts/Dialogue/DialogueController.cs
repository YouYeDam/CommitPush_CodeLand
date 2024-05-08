using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueController : MonoBehaviour
{
    public TMP_Text SpeakerNameText;  // 말하는 캐릭터 이름을 표시할 UI 요소
    public TMP_Text DialogueText;  // 대화 내용을 표시할 UI 요소
    public GameObject ResponseButtonBase;  // 응답 버튼들을 담을 패널
    public GameObject DialogueBase;
    public Button ResponseButton;  // 응답 버튼 프리팹
    public Image SpeakerImage; // 말하는 캐릭터 이미지
    private DialogueNode currentNode;  // 현재 대화 노드

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueBase.SetActive(true);
        currentNode = dialogue.nodes[0];
        DisplayNode(currentNode);
    }

    private void DisplayNode(DialogueNode node)
    {
        SpeakerNameText.text = node.SpeakerName;
        DialogueText.text = node.DialogueText;
        SpeakerImage.sprite = node.SpeakerImage;

        // 응답 버튼 초기화
        foreach (Transform child in ResponseButtonBase.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueResponse response in node.Responses)
        {
            ResponseButton.GetComponentInChildren<TMP_Text>().text = response.ResponseText;
        }
    }

    public void OnResponseButtonClicked(DialogueResponse response)
    {
        if (response.NextNode != null)
        {
            DisplayNode(response.NextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        SpeakerNameText.text = "";
        DialogueText.text = "";
        ResponseButtonBase.SetActive(false);
        DialogueBase.SetActive(false);
    }
}

