using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue/Dialogue")] // 다이얼로그 어트리뷰트 생성
public class Dialogue : ScriptableObject
{
    public DialogueNode[] nodes; // 대화 노드 배열
}

[System.Serializable]
public class DialogueNode
{
    public string SpeakerName;
    [TextArea(3, 10)]
    public string DialogueText; // 대화 내용
    public Sprite SpeakerImage;
}