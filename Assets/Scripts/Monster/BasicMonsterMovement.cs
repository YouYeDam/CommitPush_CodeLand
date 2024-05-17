using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;
    [SerializeField] float FlipTimeHead = 1f;
    [SerializeField] float FlipTimeRear = 10f;
    [SerializeField] float StopTimeHead = 5f;
    [SerializeField] float StopTimeRear = 10f;
    [SerializeField] float WaitCanWalk = 1f;
    [SerializeField] float DieDelay = 0.8f;
    [SerializeField] float PlayerOverlapRange = 0.1f;
    [SerializeField] float FollowDelay = 15f;
    Rigidbody2D MonsterRigidbody;
    Animator MonsterAnimator;
    MonsterStatus MonsterStatus;
    BoxCollider2D MonsterBoxCollider;
    CapsuleCollider2D MonsterCapsuleCollider;
    BoxCollider2D PlayerBoxCollider;
    CapsuleCollider2D PlayerCapsuleCollider;
    MonsterDropItem MonsterDropItem;
    PlayerMovement PlayerMovement;
    PlayerStatus PlayerStatus;
    MonsterSkills MonsterSkills;
    GameObject Player;

    public bool IsAlive = true;
    public bool IsLeft = true; // 초기 몬스터가 바라보는 방향
    public bool IsAttackMonster = false;
    public bool IsTakeDamge = false;
    public bool CanWalk = true;
    bool CanAttack = true;
    [SerializeField] float AttackDelayTime = 3f;

    Vector3 StartPosition; // 초기 위치
    Quaternion StartRotation; // 초기 회전
    GameObject MonsterObject; // 몬스터 프리팹
    GameObject MonsterRegenerationControllerObject; // 몬스터 재생성기 오브젝트
    MonsterRegenerationController MonsterRegenerationController;

    void Start()
    {
        MonsterRigidbody = GetComponent<Rigidbody2D>();
        MonsterRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        MonsterAnimator = GetComponent<Animator>();
        MonsterStatus = GetComponent<MonsterStatus>();
        MonsterBoxCollider = GetComponent<BoxCollider2D>();
        MonsterCapsuleCollider = GetComponent<CapsuleCollider2D>();
        MonsterDropItem = GetComponent<MonsterDropItem>();
        if (IsAttackMonster) {
            MonsterSkills = GetComponent<MonsterSkills>();
        }
        PlayerBoxCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        PlayerCapsuleCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        StartCoroutine(RandomFlip());
        StartCoroutine(RandomStop());

        Physics2D.IgnoreCollision(PlayerCapsuleCollider, MonsterCapsuleCollider, true);
        Physics2D.IgnoreCollision(PlayerBoxCollider, MonsterCapsuleCollider, true);

        Player = GameObject.FindGameObjectWithTag("Player");

        StartPosition = transform.position;
        StartRotation = transform.rotation;
        MonsterObject = this.gameObject;
        MonsterRegenerationControllerObject = GameObject.Find("MonsterRegenerationObject");
        MonsterRegenerationController = MonsterRegenerationControllerObject.GetComponent<MonsterRegenerationController>();
    }

    void Update()
    {
        if (!IsAlive) {
            return;
        }
        Die();
        if (CanWalk && !IsTakeDamge) {
            Move();
        }
        else if (IsTakeDamge){
            MoveToPlayer();
        }
    }
    
    public void Move() {
        if (!IsAlive) {
            return;
        }
        MonsterRigidbody.velocity = new Vector2 (MoveSpeed, 0f);
        if (IsLeft) {
            transform.localScale = new Vector2(-Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
        }
        else {
            transform.localScale = new Vector2(Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
        }
    }

    void MoveToPlayer() {
        if (!IsAlive) {
            return;
        }
        if (Player != null) {
            RestartWalking();
            Vector2 PlayerPosition = Player.transform.position;
            Vector2 DirectionToPlayer = (PlayerPosition - (Vector2)transform.position).normalized;
            if (DirectionToPlayer.x <= PlayerOverlapRange && DirectionToPlayer.x >= -PlayerOverlapRange) {
                MonsterRigidbody.velocity = new Vector2 (0f, 0f);
                MonsterAnimator.SetBool("IsIdling", true);
            }
            else {
                MoveSpeed = Mathf.Sign(DirectionToPlayer.x) * Mathf.Abs(MoveSpeed);
                MonsterRigidbody.velocity = new Vector2 (MoveSpeed, 0f);
                MonsterAnimator.SetBool("IsIdling", false);
                if (IsLeft) {
                transform.localScale = new Vector2(-Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
                }
                else {
                transform.localScale = new Vector2(Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
                }
                if (!CanWalk && IsAttackMonster) {
                    MonsterRigidbody.velocity = new Vector2 (0f, 0f);
                    MonsterAnimator.SetBool("IsIdling", true);
                }
            }

            if (IsAttackMonster && CanAttack) {
                StartCoroutine(AttackSkillRoutine());
                MonsterRigidbody.velocity = new Vector2 (0f, 0f);
            }
        }

        if (!PlayerMovement.IsAlive) {
            IsTakeDamge = false;
            SetRandomBehavior();
        }
    }

    IEnumerator RandomFlip() { // 랜덤한 시간마다 스프라이트 반전 호출
        while (IsAlive && !IsTakeDamge) {
            float WaitTime = Random.Range(FlipTimeHead, FlipTimeRear);
            yield return new WaitForSeconds(WaitTime);

            FlipEnemyFacing();
        }
    }

    IEnumerator RandomStop() { // 랜덤한 시간마다 이동 멈춤 호출
        while (IsAlive && !IsTakeDamge) {
            float WaitTime = Random.Range(StopTimeHead, StopTimeRear);
            yield return new WaitForSeconds(WaitTime);

            StopWalking();
        }
    }

    public void SetRandomBehavior() {
        StartCoroutine(RandomFlip());
        StartCoroutine(RandomStop());
    }
    void FlipEnemyFacing() { // 스프라이트 반전
        bool IsOnGround = MonsterCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!IsOnGround) {
            return;
        }
        MoveSpeed = -MoveSpeed;
        transform.localScale = new Vector2 (Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
    }

    void StopWalking() { // 이동 멈춤
        MonsterRigidbody.velocity = new Vector2 (0f, 0f);
        CanWalk = false;
        MonsterAnimator.SetBool("IsIdling", true);
        Invoke("RestartWalking", WaitCanWalk);
    }

    void RestartWalking() { // 재이동 시작
        MonsterAnimator.SetBool("IsIdling", false);
        if (!IsTakeDamge) {
        CanWalk = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) { // 몬스터와 플레이어 겹쳤을 때 데미지
        if (other.tag == "Player" && IsAlive) {
            PlayerMovement.TakeDamage(MonsterStatus.MonsterDamage);
        }
    }

    public void TakeDamage(int Damage) {
        if (MonsterStatus != null) {
            MonsterStatus.MonsterCurrentHealth -= Damage;
            MonsterStatus.DisplayHPMeter();
            MonsterStatus.DisplayMonsterInfo();
            IsTakeDamge = true;
            StartCoroutine(StopAggressiveMode(FollowDelay));
        }
    }

    IEnumerator StopAggressiveMode(float FollowDelay) {
        yield return new WaitForSeconds(FollowDelay);

        IsTakeDamge = false;
        if (MonsterRigidbody.velocity.x != 0) {
            MonsterAnimator.SetBool("IsIdling", false);
        }
        else {
        MonsterAnimator.SetBool("IsIdling", true);
        }
        SetRandomBehavior();
    }

    void Die() {
        if (MonsterStatus.MonsterCurrentHealth > 0) {
            return;
        }
        MonsterAnimator.SetBool("IsDying", true);
        MonsterRegenerationController.RegenerateMonster(MonsterObject, StartPosition, StartRotation);
        IsAlive = false;
        PlayerStatus.GainEXP(MonsterStatus.MonsterEXP);
        MonsterDropItem.DropItems();
        MonsterBoxCollider.enabled = false; // 플레이어 스킬 먹힘 방지
        StartCoroutine(DestroyAfterAnimation(DieDelay)); // 애니메이션 재생 후 몬스터 파괴
    }
    IEnumerator DestroyAfterAnimation(float DieDelay) {
        yield return new WaitForSeconds(DieDelay);
        Destroy(MonsterStatus.HPMeterInstance);
        Destroy(MonsterStatus.MonsterInfoInstance);
        Destroy(gameObject);
    }
    IEnumerator AttackSkillRoutine() {
        while (IsAlive && PlayerMovement.IsAlive && IsTakeDamge) {
            if (CanAttack) {
                // 플레이어와 몬스터 사이의 거리 계산
                float DistanceToPlayer = Vector2.Distance(Player.transform.position, transform.position);
                
                // 플레이어가 앞뒤 사정거리 내에 있을 때만 스킬 발사
                if (DistanceToPlayer <= MonsterSkills.UseSkillDistance) {
                    MonsterSkills.StartShootSkill();
                    CanAttack = false; // 공격 후 잠시 공격 불가 상태로 설정
                    yield return new WaitForSeconds(AttackDelayTime); // 공격 후 지연 시간 대기
                    CanAttack = true; // 다시 공격 가능 상태로 설정
                }
            }
            yield return null; // 한 프레임 대기
        }
    }
}
