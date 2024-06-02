using System;

[Serializable]
public class QuestObjective
{
    public enum ObjectiveType
    {
        Kill,    // 몬스터 처치
        Collect  // 아이템 수집
    }

    public ObjectiveType Type; // 목표 타입
    public string TargetName; // 목표 대상 이름 (몬스터 또는 아이템)
    public int RequiredAmount; // 목표 달성을 위한 수량
    public int CurrentAmount; // 현재 달성한 수량

    public QuestObjective(ObjectiveType type, string targetName, int requiredAmount)
    {
        Type = type;
        TargetName = targetName;
        RequiredAmount = requiredAmount;
        CurrentAmount = 0;
    }
    
    // 목표 달성 여부를 확인하는 메서드
    public bool IsComplete()
    {
        return CurrentAmount >= RequiredAmount;
    }

    // 목표를 초기화하는 메서드
    public void ResetCurrentAmount()
    {
        CurrentAmount = 0;
    }
}
