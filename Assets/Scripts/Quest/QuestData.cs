using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "New Quest/Quest")]
public class QuestData : ScriptableObject
{
    public string Title; // 퀘스트 제목
    public string Description; // 퀘스트 설명
    public int ExperienceReward; // 경험치 보상
    public int BitReward; // 비트 보상
    public List<Item> ItemRewards; // 아이템 보상 리스트
    public List<QuestObjective> Objectives; // 퀘스트 목표 리스트
    public int RequiredLevel; // 퀘스트 요구 레벨 추가
}