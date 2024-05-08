using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueNode[] nodes;  // 대화 노드 배열
}

[System.Serializable]
public class DialogueNode
{
    public string SpeakerName;  // 말하는 캐릭터의 이름
    public string DialogueText;  // 대화 내용
    public Sprite SpeakerImage;
    public DialogueResponse[] Responses;  // 대화에 대한 응답 옵션들
}

[System.Serializable]
public class DialogueResponse
{
    public string ResponseText;  // 응답 텍스트
    public DialogueNode NextNode;  // 선택 시 진행될 다음 노드
}