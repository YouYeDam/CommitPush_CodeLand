using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] GameObject NormalAttack;
    [SerializeField] SkillSlot InitialSkillSlot;

    void Start() {
        SetNormalAttackSlot();
    }
    public void SetNormalAttackSlot() { // 첫 번째 스킬 슬롯에 기본공격 할당
        InitialSkillSlot.AddSkill(NormalAttack);
    }
}
