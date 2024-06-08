using System.Collections.Generic;
using UnityEngine;

public class NPCQuestState : MonoBehaviour
{
    public Dictionary<string, int> NpcQuestIndices = new Dictionary<string, int>();

    public void SetQuestIndex(string npcName, int index)
    {
        if (NpcQuestIndices.ContainsKey(npcName))
        {
            NpcQuestIndices[npcName] = index;
        }
        else
        {
            NpcQuestIndices.Add(npcName, index);
        }
    }

    public int GetQuestIndex(string npcName)
    {
        if (NpcQuestIndices.ContainsKey(npcName))
        {
            return NpcQuestIndices[npcName];
        }
        else
        {
            return 0;
        }
    }
}
