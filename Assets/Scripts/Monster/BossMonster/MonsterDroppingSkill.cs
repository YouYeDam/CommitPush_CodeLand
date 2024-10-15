using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDroppingSkill : MonoBehaviour
{
    Animator MyAnimator;
    CapsuleCollider2D MyCapsuleCollider;
    BasicMonsterMovement BasicMonsterMovement;
    GameObject DropObjectInstance;
    [SerializeField] GameObject DropObject;
    [SerializeField] Transform SkillSpot;
    [SerializeField] float BackToIdleAnimTime = 0.35f;
    [SerializeField] float SkillDelayForAnim = 0f; // 애니메이션 재생 후 스킬 발사 대기시간
    public float UseSkillDistance = 60f; // 스킬 사용 거리 (사거리)

    void Start() {
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
    }
    public void ShootSkill() { //낙하물 투하 스킬
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));

        if (IsOnLadderGround || IsOnGround) { // 지면에 있을 경우에만
            Invoke("InstantiateSkill", SkillDelayForAnim);
            MyAnimator.SetBool("IsDropping", true);
            BasicMonsterMovement.IsSkilling = true;
            BasicMonsterMovement.CanWalk = false;
            Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
        }
    }

    void BackToIdleAnim() { // 애니메이션 복구
        MyAnimator.SetBool("IsDropping", false);
        BasicMonsterMovement.IsSkilling = false;
        BasicMonsterMovement.CanWalk = true;
    }

    void InstantiateSkill() { // 발사체 생성
        DropObjectInstance = Instantiate(DropObject, SkillSpot.position, transform.rotation);
        MonsterAttackSkill MonsterAttackSkill = DropObjectInstance.GetComponent<MonsterAttackSkill>();

        if (MonsterAttackSkill != null) {
            BasicMonsterMovement BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
            MonsterStatus MonsterStatus = GetComponent<MonsterStatus>();
            MonsterAttackSkill.BasicMonsterMovement = BasicMonsterMovement;
            MonsterAttackSkill.MonsterStatus = MonsterStatus;
            MonsterAttackSkill.IsLeft = BasicMonsterMovement.IsLeft;
        }
    }
}
