using System.Collections.Generic;
using UnityEngine;

public class NPCQuestState : MonoBehaviour
{
    public Dictionary<string, int> NpcQuestIndices = new Dictionary<string, int>(); // 퀘스트를 딕셔너리로 관리 => NPC이름, 퀘스트 인덱스

    public void SetQuestIndex(string NpcName, int Index) { // NPC가 이미 딕셔너리에 존재하면 퀘스트 인덱스를 업데이트
        if (NpcQuestIndices.ContainsKey(NpcName)) {
            NpcQuestIndices[NpcName] = Index;
        }
        else { // 새로운 NPC-퀘스트 인덱스 추가
            NpcQuestIndices.Add(NpcName, Index);
        }
    }

    public int GetQuestIndex(string NpcName) { // 딕셔너리에 해당 NPC가 있으면 퀘스트 인덱스를 반환
        if (NpcQuestIndices.ContainsKey(NpcName)) { 
            return NpcQuestIndices[NpcName];
        }
        else {
            return 0;
        }
    }
}
