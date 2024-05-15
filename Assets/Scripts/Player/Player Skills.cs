using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkills : MonoBehaviour
{
    Animator MyAnimator;
    CapsuleCollider2D MyCapsuleCollider;
    BoxCollider2D[] MyBoxColliders;
    PlayerMovement PlayerMovement;
    PlayerManager PlayerManager;
    PlayerStatus PlayerStatus;
    [SerializeField] GameObject NormalAttack;
    [SerializeField] GameObject QSkill;
    [SerializeField] Transform SkillSpot;
    [SerializeField] Transform BuffSpot;
    [SerializeField] float GlobalCoolDown = 0.3f;
    [SerializeField] float QSkillCoolDown;
    [SerializeField] float BackToIdleAnimTime = 0.2f;

    bool CanAttack = true;
    bool CanQSkill = true;
    int QSKillMPUse;
    void Start() {
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        MyBoxColliders = GetComponents<BoxCollider2D>();
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        PlayerManager = GetComponent<PlayerManager>();
        SetSkillsCoolTime();
        GlobalCoolDown -= PlayerStatus.PlayerAP; // 글쿨 가속력 공식: 글쿨 - 가속력
    }

    void OnNormalAttack() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                Instantiate(NormalAttack, SkillSpot.position, transform.rotation);
                MyAnimator.SetBool("IsAttacking", true);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정
                
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }

    void OnQSkill() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || QSkill == null || PlayerStatus.PlayerCurrentMP < QSKillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanQSkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                Instantiate(QSkill, SkillSpot.position, transform.rotation);
                MyAnimator.SetBool("IsAttacking", true);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanQSkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= QSKillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }

                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("RestQSkill", QSkillCoolDown); // 쿨다운 이후 RestQSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }
    void ResetCanAttack() {
        CanAttack = true;
    }

    void RestQSkill() {
        CanQSkill = true;
    }
    void BackToIdleAnim() {
        MyAnimator.SetBool("IsAttacking", false);
    }



    void SetSkillsCoolTime() {
        PlayerAttackSkill PlayerQAttackSkill = QSkill.GetComponent<PlayerAttackSkill>();
        PlayerBuffSkill PlayerQBuffSkill = QSkill.GetComponent<PlayerBuffSkill>();
        if (PlayerQAttackSkill != null) {
            QSkillCoolDown = PlayerQAttackSkill.CoolDown;
            QSKillMPUse = PlayerQAttackSkill.MPUse;
        }
        else if (PlayerQBuffSkill != null) {
            QSkillCoolDown = PlayerQBuffSkill.CoolDown;
            QSKillMPUse = PlayerQBuffSkill.MPUse;
        }
    }
}

