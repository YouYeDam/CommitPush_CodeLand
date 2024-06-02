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
    [TextArea(3, 10)]
    public string DialogueText;  // 대화 내용
    public Sprite SpeakerImage;
}