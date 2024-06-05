using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string Title; // 퀘스트 제목
    public string QuestDescription; // 퀘스트 설명
    public int ExperienceReward; // 경험치 보상
    public int BitReward; // 비트 보상
    public List<Item> ItemRewards; // 아이템 보상 리스트
    public List<QuestObjective> Objectives; // 퀘스트 목표 리스트
    public bool IsCompleted; // 퀘스트 완료 여부
    public bool IsReadyToComplete; // 퀘스트 완료 준비 여부
    public int RequiredLevel; // 퀘스트 요구 레벨

    public Quest(string title, string description, int experienceReward, int bitReward, List<Item> itemRewards, List<QuestObjective> objectives, int requiredLevel)
    {
        Title = title;
        QuestDescription = description;
        ExperienceReward = experienceReward;
        BitReward = bitReward;
        ItemRewards = itemRewards;
        Objectives = objectives;
        IsCompleted = false;
        IsReadyToComplete = false;
        RequiredLevel = requiredLevel;
    }
}