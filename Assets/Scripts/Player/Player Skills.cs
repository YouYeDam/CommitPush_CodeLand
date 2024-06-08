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
    public GameObject QSkill;
    public GameObject WSkill;
    public GameObject ESkill;
    public GameObject RSkill;
    public GameObject SSkill;
    public GameObject DSkill;

    [SerializeField] Transform SkillSpot;
    [SerializeField] Transform BuffSpot;
    [SerializeField] float GlobalCoolDown = 0.3f;
    [SerializeField] public float QSkillCoolDown;
    [SerializeField] public float WSkillCoolDown;
    [SerializeField] public float ESkillCoolDown;
    [SerializeField] public float RSkillCoolDown;
    [SerializeField] public float SSkillCoolDown;
    [SerializeField] public float DSkillCoolDown;

    [SerializeField] float BackToIdleAnimTime = 0.2f;

    bool CanAttack = true;
    public bool CanQSkill = true;
    public bool CanWSkill = true;
    public bool CanESkill = true;
    public bool CanRSkill = true;
    public bool CanSSkill = true;
    public bool CanDSkill = true;

    public float QSkillRemainingCoolDown = 0;
    public float WSkillRemainingCoolDown = 0;
    public float ESkillRemainingCoolDown = 0;
    public float RSkillRemainingCoolDown = 0;
    public float SSkillRemainingCoolDown = 0;
    public float DSkillRemainingCoolDown = 0;

    int QSkillMPUse;
    int WSkillMPUse;
    int ESkillMPUse;
    int RSkillMPUse;
    int SSkillMPUse;
    int DSkillMPUse;

    void Start() {
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        MyBoxColliders = GetComponents<BoxCollider2D>();
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        PlayerManager = GetComponent<PlayerManager>();
    }

    void Update() {
        UpdateSkillCoolDown(ref QSkillRemainingCoolDown);
        UpdateSkillCoolDown(ref WSkillRemainingCoolDown);
        UpdateSkillCoolDown(ref ESkillRemainingCoolDown);
        UpdateSkillCoolDown(ref RSkillRemainingCoolDown);
        UpdateSkillCoolDown(ref SSkillRemainingCoolDown);
        UpdateSkillCoolDown(ref DSkillRemainingCoolDown);
        SkillCoolDownReduce();
    }

    void UpdateSkillCoolDown(ref float SkillCoolDown) {
        if (SkillCoolDown > 0) {
            SkillCoolDown -= Time.deltaTime;
            if (SkillCoolDown < 0) {
                SkillCoolDown = 0;
            }
        }
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
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || QSkill == null || PlayerStatus.PlayerCurrentMP < QSkillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanQSkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                CheckSkillType(QSkill);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanQSkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= QSkillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }
                QSkillRemainingCoolDown = QSkillCoolDown;
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("ResetQSkill", QSkillCoolDown); // 쿨다운 이후 RestQSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }

    void OnWSkill() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || WSkill == null || PlayerStatus.PlayerCurrentMP < WSkillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanWSkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                CheckSkillType(WSkill);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanWSkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= WSkillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }
                WSkillRemainingCoolDown = WSkillCoolDown;
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("ResetWSkill", WSkillCoolDown); // 쿨다운 이후 RestQSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }
    void OnESkill() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || ESkill == null || PlayerStatus.PlayerCurrentMP < ESkillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanESkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                CheckSkillType(ESkill);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanESkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= ESkillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }
                ESkillRemainingCoolDown = ESkillCoolDown;
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("ResetESkill", ESkillCoolDown); // 쿨다운 이후 RestQSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }
    void OnRSkill() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || RSkill == null || PlayerStatus.PlayerCurrentMP < RSkillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanRSkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                CheckSkillType(RSkill);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanRSkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= RSkillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }
                RSkillRemainingCoolDown = RSkillCoolDown;
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("ResetRSkill", RSkillCoolDown); // 쿨다운 이후 ResetRSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }

    void OnSSkill() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || SSkill == null || PlayerStatus.PlayerCurrentMP < SSkillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanSSkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                CheckSkillType(SSkill);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanSSkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= SSkillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }
                SSkillRemainingCoolDown = SSkillCoolDown;
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("ResetSSkill", SSkillCoolDown); // 쿨다운 이후 ResetSSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }
    void OnDSkill() {
        if (PlayerMovement.IsAlive == false || !PlayerManager.CanInput || DSkill == null || PlayerStatus.PlayerCurrentMP < DSkillMPUse) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (CanAttack && CanDSkill) {
            if (!IsOnLadder || IsOnLadderGround || IsSteppingLadder) {
                CheckSkillType(DSkill);
                CanAttack = false; // 스킬을 사용한 후 플래그를 false로 설정
                CanDSkill = false;
                PlayerMovement.IsWalkingAllowed = false; // 스킬을 사용한 후 이동 멈춤 설정

                PlayerStatus.PlayerCurrentMP -= DSkillMPUse;
                if (PlayerStatus.PlayerCurrentMP < 0) {
                    PlayerStatus.PlayerCurrentMP = 0;
                }
                DSkillRemainingCoolDown = DSkillCoolDown;
                Invoke("ResetCanAttack", GlobalCoolDown); // 쿨다운 이후 ResetCanAttack 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("ResetDSkill", DSkillCoolDown); // 쿨다운 이후 ResetDSkill 함수를 호출하여 스킬 사용 가능 상태로 변경
                Invoke("BackToIdleAnim", BackToIdleAnimTime); // 일정 시간 이후 BackToIdleAnim 함수를 호출하여 Idle 애니메이션으로 변경
            }
        }
    }

    void CheckSkillType(GameObject Skill) {
        switch(Skill.GetComponent<SkillInfo>().SkillType) {
            case "공격형":
                Instantiate(Skill, SkillSpot.position, transform.rotation);
                MyAnimator.SetBool("IsAttacking", true);
                break;
            case "버프형":
                Instantiate(Skill, BuffSpot.position, transform.rotation);
                MyAnimator.SetBool("IsBuffing", true);
                break;
            case "타겟형":
                Instantiate(Skill, SkillSpot.position, transform.rotation);
                MyAnimator.SetBool("IsAttacking", true);
                break;
            case "디버프형":
                Instantiate(Skill, SkillSpot.position, transform.rotation);
                MyAnimator.SetBool("IsAttacking", true);
                break;
            case "회복형":
                Instantiate(Skill, BuffSpot.position, transform.rotation);
                MyAnimator.SetBool("IsBuffing", true);
                break;
        }
    }
    void ResetCanAttack() {
        CanAttack = true;
    }
    void ResetQSkill() {
        CanQSkill = true;
    }
    void ResetWSkill() {
        CanWSkill = true;
    }
    void ResetESkill() {
        CanESkill = true;
    }
    void ResetRSkill() {
        CanRSkill = true;
    }
    void ResetSSkill() {
        CanSSkill = true;
    }
    void ResetDSkill() {
        CanDSkill = true;
    }
    void BackToIdleAnim() {
        MyAnimator.SetBool("IsAttacking", false);
        MyAnimator.SetBool("IsBuffing", false);
    }

    public void SetSkillsCoolTime(string ButtonKey)
    {
        GameObject Skill = GetSkillByButtonKey(ButtonKey);

        if (Skill != null)
        {
            PlayerAttackSkill AttackSkill = Skill.GetComponent<PlayerAttackSkill>();
            PlayerBuffSkill BuffSkill = Skill.GetComponent<PlayerBuffSkill>();
            PlayerTargetSkill TargetSkill = Skill.GetComponent<PlayerTargetSkill>();
            PlayerDebuffSkill DebuffSkill = Skill.GetComponent<PlayerDebuffSkill>();
            PlayerHealingSkill HealingSkill = Skill.GetComponent<PlayerHealingSkill>();

            if (AttackSkill != null)
            {
                SetSkillProperties(ButtonKey, AttackSkill.CoolDown, AttackSkill.MPUse);
            }
            else if (BuffSkill != null)
            {
                SetSkillProperties(ButtonKey, BuffSkill.CoolDown, BuffSkill.MPUse);
            }
            else if (TargetSkill != null)
            {
                SetSkillProperties(ButtonKey, TargetSkill.CoolDown, TargetSkill.MPUse);
            }
            else if (DebuffSkill != null)
            {
                SetSkillProperties(ButtonKey, DebuffSkill.CoolDown, DebuffSkill.MPUse);
            }
            else if (HealingSkill != null)
            {
                SetSkillProperties(ButtonKey, HealingSkill.CoolDown, HealingSkill.MPUse);
            }
        }
    }

    private GameObject GetSkillByButtonKey(string ButtonKey)
    {
        switch (ButtonKey)
        {
            case "Q": return QSkill;
            case "W": return WSkill;
            case "E": return ESkill;
            case "R": return RSkill;
            case "S": return SSkill;
            case "D": return DSkill;
            default: return null;
        }
    }

    private void SetSkillProperties(string ButtonKey, float CoolDown, int MPUse)
    {
        switch (ButtonKey)
        {
            case "Q":
                QSkillCoolDown = CoolDown;
                QSkillMPUse = MPUse;
                break;
            case "W":
                WSkillCoolDown = CoolDown;
                WSkillMPUse = MPUse;
                break;
            case "E":
                ESkillCoolDown = CoolDown;
                ESkillMPUse = MPUse;
                break;
            case "R":
                RSkillCoolDown = CoolDown;
                RSkillMPUse = MPUse;
                break;
            case "S":
                SSkillCoolDown = CoolDown;
                SSkillMPUse = MPUse;
                break;
            case "D":
                DSkillCoolDown = CoolDown;
                DSkillMPUse = MPUse;
                break;
        }
    }

    void SkillCoolDownReduce() {
        QSkillCoolDown = QSkillCoolDown * (1 - PlayerStatus.PlayerAP);
        if (QSkillCoolDown < 0.3f) {
            QSkillCoolDown = 0.3f;
        }

        WSkillCoolDown = WSkillCoolDown * (1 - PlayerStatus.PlayerAP);
        if (WSkillCoolDown < 0.3f) {
            WSkillCoolDown = 0.3f;
        }

        ESkillCoolDown = ESkillCoolDown * (1 - PlayerStatus.PlayerAP);
        if (ESkillCoolDown < 0.3f) {
            ESkillCoolDown = 0.3f;
        }

        RSkillCoolDown = RSkillCoolDown * (1 - PlayerStatus.PlayerAP);
        if (RSkillCoolDown < 0.3f) {
            RSkillCoolDown = 0.3f;
        }

        SSkillCoolDown = SSkillCoolDown * (1 - PlayerStatus.PlayerAP);
        if (SSkillCoolDown < 0.3f) {
            SSkillCoolDown = 0.3f;
        }

        DSkillCoolDown = DSkillCoolDown * (1 - PlayerStatus.PlayerAP);
        if (DSkillCoolDown < 0.3f) {
            DSkillCoolDown = 0.3f;
        }
    }
}

