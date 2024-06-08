using System.Collections.Generic;
using UnityEngine;

public class NPCQuestState : MonoBehaviour
{
    public Dictionary<string, int> npcQuestIndices = new Dictionary<string, int>();

    public void SetQuestIndex(string npcName, int index)
    {
        if (npcQuestIndices.ContainsKey(npcName))
        {
            npcQuestIndices[npcName] = index;
        }
        else
        {
            npcQuestIndices.Add(npcName, index);
        }
    }

    public int GetQuestIndex(string npcName)
    {
        if (npcQuestIndices.ContainsKey(npcName))
        {
            return npcQuestIndices[npcName];
        }
        else
        {
            return 0;
        }
    }
}
