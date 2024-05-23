using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkills : MonoBehaviour
{
    Animator MyAnimator;
    CapsuleCollider2D MyCapsuleCollider;
    BasicMonsterMovement BasicMonsterMovement;
    [SerializeField] GameObject Projectile;
    [SerializeField] Transform SkillSpot;
    [SerializeField] float BackToIdleAnimTime = 0.35f;
    [SerializeField] float Delay = 1f; // 피격 후 스킬 시전 대기시간
    public float UseSkillDistance = 8f; // 스킬 사용 거리
    public bool IsSkilling = false;

    void Start() {
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
    }

    public void StartShootSkill() {
        StartCoroutine(ShootSkill());
    }

    IEnumerator ShootSkill() {
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));

        if (IsOnLadderGround || IsOnGround) {
            yield return new WaitForSeconds(Delay); // 1초 대기
            GameObject ProjectileInstance = Instantiate(Projectile, SkillSpot.position, transform.rotation);
            MyAnimator.SetBool("IsAttacking", true);
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
        MyAnimator.SetBool("IsAttacking", false);
        IsSkilling = false;
        BasicMonsterMovement.CanWalk = true;
    }
}
