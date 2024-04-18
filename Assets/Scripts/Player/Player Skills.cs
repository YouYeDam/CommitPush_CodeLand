using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkills : MonoBehaviour
{
    Animator MyAnimator;
    CapsuleCollider2D MyCapsuleCollider;
    PlayerMovement PlayerMovement;
    PlayerManager PlayerManager;
    [SerializeField] GameObject NormalAttack;
    [SerializeField] Transform SkillSpot;
    [SerializeField] float NormalAttackCoolTime = 0.3f;
    [SerializeField] float BackToIdleAnimTime = 0.2f;

    bool CanUseNormalAttack = true;

    void Start() {
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        PlayerManager = GetComponent<PlayerManager>();
    }

    void OnNormalAttack(InputValue value) {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        if (CanUseNormalAttack) {
            if (!IsOnLadder || IsOnLadderGround) {
                Instantiate(NormalAttack, SkillSpot.position, transform.rotation);
                MyAnimator.SetBool("IsAttacking", true);
                CanUseNormalAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정
                
                Invoke("ResetNormalAttack", NormalAttackCoolTime); // 2초 후에 ResetSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 1초 후에 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }
    void ResetNormalAttack()
        {
            CanUseNormalAttack = true;
        }

    void BackToIdleAnim() {
        MyAnimator.SetBool("IsAttacking", false);
    }

}

