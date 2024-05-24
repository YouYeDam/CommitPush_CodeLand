using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterThrowingSkill : MonoBehaviour
{
    Animator MyAnimator;
    CapsuleCollider2D MyCapsuleCollider;
    BasicMonsterMovement BasicMonsterMovement;
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform ThrowingSpot;
    [SerializeField] float BackToIdleAnimTime = 0.35f;
    public float UseSkillDistance = 8f; // 스킬 사용 거리
    public bool IsSkilling = false;

    void Start() {
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
    }
    public void ShootSkill() {
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));

        if (IsOnLadderGround || IsOnGround) {
            GameObject ProjectileInstance = Instantiate(Projectile, ThrowingSpot.position, transform.rotation);
            MyAnimator.SetBool("IsThrowing", true);
            IsSkilling = true;
            BasicMonsterMovement.CanWalk = false;
            Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            
            // MonsterAttackSkill 컴포넌트를 가져와 필요한 값을 할당
            MonsterAttackSkill MonsterAttackSkill = ProjectileInstance.GetComponent<MonsterAttackSkill>();
            if (MonsterAttackSkill != null) {
                BasicMonsterMovement BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
                MonsterStatus MonsterStatus = GetComponent<MonsterStatus>();
                MonsterAttackSkill.BasicMonsterMovement = BasicMonsterMovement;
                MonsterAttackSkill.MonsterStatus = MonsterStatus;
                MonsterAttackSkill.IsLeft = BasicMonsterMovement.IsLeft;
            }
            if (!BasicMonsterMovement.IsAlive) {
                Destroy(ProjectileInstance);
            }
        }
    }

    void BackToIdleAnim() {
        MyAnimator.SetBool("IsThrowing", false);
        IsSkilling = false;
        BasicMonsterMovement.CanWalk = true;
    }
}
