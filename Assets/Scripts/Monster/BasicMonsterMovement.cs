using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;
    [SerializeField] float FlipTimeHead = 1f; // 좌우 반전 대기시간(최소 쿨타임)
    [SerializeField] float FlipTimeRear = 10f; // 좌우 반전 대기시간(최대 쿨타임)
    [SerializeField] float StopTimeHead = 5f; // 멈춤 대기시간(최소 쿨타임)
    [SerializeField] float StopTimeRear = 10f; // 멈춤 대기시간(최대 쿨타임)
    [SerializeField] float WaitCanWalk = 1f;
    [SerializeField] float DieDelay = 0.8f;
    [SerializeField] float PlayerOverlapRange = 0.1f; // 플레이어와 겹침 확인 범위
    [SerializeField] float FollowDelay = 15f;
    Rigidbody2D MonsterRigidbody;
    Animator MonsterAnimator;
    MonsterStatus MonsterStatus;
    MonsterTakeDamageDisplay MonsterTakeDamageDisplay;
    public BoxCollider2D MonsterBoxCollider;
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
    public bool IsAttackMonster = false; // 스킬을 사용하는 몬스터인지
    public bool IsTakeDamge = false;
    public bool CanWalk = true;
    public bool CanTriggerDamage = true; // 데미지를 줄 수 있는 상태인지(리스폰 후 시간이 지나서 데미지를 줄 수 있는지)
    bool CanAttack = true;
    public bool IsSkilling = false;
    private Coroutine StopAggressiveModeCoroutine;
    [SerializeField] float AttackDelayTime = 3f;

    Vector3 StartPosition; // 초기 위치
    Quaternion StartRotation; // 초기 회전
    GameObject MonsterObject; // 몬스터 프리팹
    GameObject MonsterRegenerationControllerObject; // 몬스터 재생성기 오브젝트
    MonsterRegenerationController MonsterRegenerationController;

    float OriginalMoveSpeed; // 기본 몬스터 스피드
    float OriginalDirection; // 기본 몬스터 방향

    void Start()
    {
        MonsterRigidbody = GetComponent<Rigidbody2D>();
        MonsterRigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep; // 몬스터가 항상 물리 연산을 받도록 강제(버그 방지)
        MonsterAnimator = GetComponent<Animator>();
        MonsterStatus = GetComponent<MonsterStatus>();
        MonsterBoxCollider = GetComponent<BoxCollider2D>();
        MonsterCapsuleCollider = GetComponent<CapsuleCollider2D>();
        MonsterDropItem = GetComponent<MonsterDropItem>();
        MonsterTakeDamageDisplay = GetComponent<MonsterTakeDamageDisplay>();

        if (IsAttackMonster) {
            MonsterSkills = GetComponent<MonsterSkills>();
        }

        PlayerBoxCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        PlayerCapsuleCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        SetRandomBehavior();

        // 플레이어와 몬스터가 겹칠 수 있도록 충돌 무시
        Physics2D.IgnoreCollision(PlayerCapsuleCollider, MonsterCapsuleCollider, true);
        Physics2D.IgnoreCollision(PlayerBoxCollider, MonsterCapsuleCollider, true);

        Player = GameObject.FindGameObjectWithTag("Player");

        StartPosition = transform.position;
        StartRotation = transform.rotation;
        MonsterObject = this.gameObject;

        if (!MonsterStatus.IsBossMonster && !MonsterStatus.IsSummonedMonster) { // 보스몬스터도 아니고 소환된 몬스터도 아닐 경우 리스폰
            MonsterRegenerationControllerObject = GameObject.Find("MonsterRegenerationObject");
            MonsterRegenerationController = MonsterRegenerationControllerObject.GetComponent<MonsterRegenerationController>();
        }
        OriginalMoveSpeed = MoveSpeed;
        OriginalDirection = Mathf.Sign(MoveSpeed);;
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
        else if (IsTakeDamge){ // 플레이어에게 피격 시 플레이어 쪽으로 이동(어그로 획득)
            MoveToPlayer();
        }
    }
    
    void Move() { // 몬스터 이동
        if (!IsAlive) {
            return;
        }
        MonsterRigidbody.velocity = new Vector2 (MoveSpeed, 0f);

        if (IsLeft) { // 스프라이트가 바라보는 방향으로 이동할 수 있도록
            transform.localScale = new Vector2(-Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
        }
        else {
            transform.localScale = new Vector2(Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
        }
    }

    void MoveToPlayer() { // 플레이어 어그로 기능
        if (!IsAlive) {
            return;
        }

        if (Player != null) {
            RestartWalking(); // 멈춰있었다면 다시 이동가능하도록
            Vector2 PlayerPosition = Player.transform.position;
            Vector2 DirectionToPlayer = (PlayerPosition - (Vector2)transform.position).normalized;

            // 플레이어가 몬스터 겹침 확인 범위 내에 있다면 몬스터 이동 멈춤 (계속 뒤집는 행동 방지)
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

            if (IsAttackMonster && CanAttack) { // 어그로 끌렸더라도 스킬은 사용하도록
                StartCoroutine(AttackSkillRoutine());
                MonsterRigidbody.velocity = new Vector2 (0f, 0f); // 동일하게 멈춰서 스킬 사용
            }
        }

        if (!PlayerMovement.IsAlive) { // 플레이어 사망 시 어그로 풀림
            IsTakeDamge = false;
            MonsterAnimator.SetBool("IsIdling", false);
            SetRandomBehavior(); // 다시 랜덤 행동 시작
        }
    }

    IEnumerator RandomFlip() { // 몬스터 랜덤 좌우반전
        while (IsAlive) {
            if (IsTakeDamge) { // 피격 시 랜덤 좌우반전 끔
                yield break;
            }
            float WaitTime = Random.Range(FlipTimeHead, FlipTimeRear);
            yield return new WaitForSeconds(WaitTime);

            if (IsTakeDamge) { // 피격 시 코루틴 중단
                yield break;
            }
            FlipEnemyFacing(); // 플레이어 방향으로 스프라이트 뒤집기
        }
    }

    IEnumerator RandomStop() { // 몬스터 랜덤 멈춤
        while (IsAlive) {
            if (IsTakeDamge) { // 피격 시 랜덤 멈춤 끔
                yield break;
            }
            float WaitTime = Random.Range(StopTimeHead, StopTimeRear);
            yield return new WaitForSeconds(WaitTime);

            if (IsTakeDamge) { // 피격 시 코루틴 중단
                yield break;
            }
            StopWalking(); // 랜덤 이동을 멈추고 플레이어에게만 다가가기 위해
        }
    }

    public void SetRandomBehavior() { // 랜덤 행동 세팅
        StartCoroutine(RandomFlip());
        StartCoroutine(RandomStop());
    }

    public void FlipEnemyFacing() { // 스프라이트 반전
        bool IsOnGround = MonsterCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!IsOnGround) {
            return;
        }
        MoveSpeed = -MoveSpeed;
        transform.localScale = new Vector2 (Mathf.Sign(MonsterRigidbody.velocity.x), 1f);
    }

    public void StopWalking() { // 이동 멈춤
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
        if (other.tag == "Player" && IsAlive && CanTriggerDamage) {
            PlayerMovement.TakeDamage(MonsterStatus.MonsterDamage);
        }
    }

    public void TakeDamage(int Damage, bool IsCrit) { // 피격 시 데미지 받기
        if (MonsterStatus != null) {
            if (MonsterStatus.BiggerThanPlayerLevel) { // 몬스터 LV > 플레이어 LV
                Damage = Mathf.RoundToInt(Damage * (1 - MonsterStatus.LevelDiff / 100f)); // 받는 데미지 감소
            }
            else { // 플레이어 LV > 몬스터 LV
                Damage = Mathf.RoundToInt(Damage * (1 + MonsterStatus.LevelDiff / 100f)); // 받는 데미지 증가
            }

            MonsterStatus.MonsterCurrentHealth -= Damage;
            if (MonsterStatus.MonsterCurrentHealth < 0) { // 몬스터 체력이 음수값으로 떨어지지 않도록
                MonsterStatus.MonsterCurrentHealth = 0;
            }

            MonsterStatus.DisplayHPMeter();
            MonsterStatus.DisplayMonsterInfo();
            MonsterTakeDamageDisplay.DisplayDamageBar(Damage, IsCrit);
            IsTakeDamge = true;

            // 기존의 어그로 멈춤 코루틴이 있다면 중단
            if (StopAggressiveModeCoroutine != null) {
                StopCoroutine(StopAggressiveModeCoroutine);
            }

            // 새로운 어그로 멈춤 코루틴 시작
            if (!MonsterStatus.IsBossMonster) {
                StopAggressiveModeCoroutine = StartCoroutine(StopAggressiveMode(FollowDelay));
            }
        }
    }

    IEnumerator StopAggressiveMode(float FollowDelay) { // 어그로 멈춤 코루틴
        yield return new WaitForSeconds(FollowDelay);
        IsTakeDamge = false;
        yield return new WaitForSeconds(0.1f);

        if (MonsterRigidbody.velocity.x != 0) {
            MonsterAnimator.SetBool("IsIdling", false);
        }
        else {
        MonsterAnimator.SetBool("IsIdling", true);
        }

        if (PlayerMovement.IsAlive) {
            SetRandomBehavior();
        }
    }

    void Die() { // 몬스터 죽음
        if (MonsterStatus.MonsterCurrentHealth > 0) { // 체력이 0보다 크면 그냥 반환
            return;
        }

        MonsterAnimator.SetBool("IsDying", true);
        if (MonsterRegenerationController) { // 몬스터 리스폰 
            RestoreOriginalSpeed(OriginalMoveSpeed, OriginalDirection);
            MonsterRegenerationController.RegenerateMonster(MonsterObject, StartPosition, StartRotation);
        }
        IsAlive = false;
        PlayerStatus.GainEXP(MonsterStatus.MonsterEXP);
        MonsterDropItem.DropItems();
        MonsterBoxCollider.enabled = false; // 플레이어 스킬 먹힘 방지 (몬스터 죽는 애니메이션 동안 스킬 흡수 못하도록)

        if (MonsterStatus.IsSummoningMonster && !MonsterStatus.IsBossMonster) {
            GenerateMonster GenerateMonster = GetComponent<GenerateMonster>();
            GenerateMonster.GenerateMonsters();
        }

        StartCoroutine(DestroyAfterAnimation(DieDelay)); // 애니메이션 재생 후 몬스터 파괴
        
        QuestManager questManager = FindObjectOfType<QuestManager>(); // 퀘스트 목표 업데이트
        if (questManager != null) {
            questManager.UpdateObjective(MonsterStatus.MonsterName, 1); // 몬스터 이름과 처치 수량을 전달
        }
    }

    IEnumerator DestroyAfterAnimation(float DieDelay) { // 애니메이션 재생 후 몬스터 삭제
        yield return new WaitForSeconds(DieDelay);
        Destroy(MonsterStatus.HPMeterInstance);
        Destroy(MonsterStatus.MonsterInfoInstance);
        Destroy(gameObject);
    }

    IEnumerator AttackSkillRoutine() { // 스킬 사용 몬스터 루틴
        while (IsAlive && PlayerMovement.IsAlive && IsTakeDamge) {
            if (CanAttack) {
                // 플레이어와 몬스터 사이의 거리 계산
                float DistanceToPlayer = Vector2.Distance(Player.transform.position, transform.position);

                // 플레이어가 앞뒤 사정거리 내에 있을 때만 스킬 발사
                if (DistanceToPlayer <= MonsterSkills.UseSkillDistance) {
                    while (IsSkilling) {
                        yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
                    }

                    MonsterSkills.StartShootSkill();
                    IsSkilling = true; 
                    CanAttack = false;
                    yield return new WaitForSeconds(AttackDelayTime);
                    IsSkilling = false;
                    CanAttack = true;
                }
            }
            yield return null; // 한 프레임 대기
        }
    }

    public void MonsterSlowDebuff(float SlowDuration, float SlowDebuffFactor) { // 몬스터 이동속도 감소 디버프
        StartCoroutine(ApplySlowDebuff(SlowDuration, SlowDebuffFactor));
    }

    IEnumerator ApplySlowDebuff(float SlowDuration, float SlowDebuffFactor) { // 몬스터 이동속도 감소 디버프 루틴
        float SavedMoveSpeed = Mathf.Abs(MoveSpeed); // 디버프 당시 속도 저장
        float SavedDirection = Mathf.Sign(MoveSpeed); //  디버프 당시 방향 저장

        MoveSpeed = SavedDirection * SavedMoveSpeed * SlowDebuffFactor;

        // MoveSpeed가 절댓값 2 미만이면 2로 설정 (이동속도가 너무 떨어지는 것 방지)
        if (Mathf.Abs(MoveSpeed) < 2f) {
            MoveSpeed = SavedDirection * 2f;
        }

        yield return new WaitForSeconds(SlowDuration);

        RestoreOriginalSpeed(SavedMoveSpeed, SavedDirection);
    }

    void RestoreOriginalSpeed(float OriginalMoveSpeed, float OriginalDirection) { // 몬스터 이동속도 복구
        MoveSpeed = OriginalDirection * OriginalMoveSpeed;
    }
}
