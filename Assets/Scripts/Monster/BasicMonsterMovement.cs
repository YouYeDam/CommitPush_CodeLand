using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;
    [SerializeField] float FlipTimeHead = 1f;
    [SerializeField] float FlipTimeRear = 1f;
    [SerializeField] float WaitCanWalk = 1f;
    [SerializeField] float DieDelay = 0.6f;
    Rigidbody2D MonsterRigidbody;
    Animator MonsterAnimator;
    MonsterStatus MonsterStatus;
    BoxCollider2D MonsterBoxCollider;
    CapsuleCollider2D MonsterCapsuleCollider;
    BoxCollider2D PlayerBoxCollider;
    CapsuleCollider2D PlayerCapsuleCollider;
    PlayerMovement PlayerMovement;
    PlayerStatus PlayerStatus;
    public bool IsAlive = true;

    bool CanWalk = true;
    void Start()
    {
        MonsterRigidbody = GetComponent<Rigidbody2D>();
        MonsterAnimator = GetComponent<Animator>();
        MonsterStatus = GetComponent<MonsterStatus>();
        MonsterBoxCollider = GetComponent<BoxCollider2D>();
        MonsterCapsuleCollider = GetComponent<CapsuleCollider2D>();
        PlayerBoxCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        PlayerCapsuleCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        StartCoroutine(RandomFlip());
        StartCoroutine(RandomStop());

        Physics2D.IgnoreCollision(PlayerCapsuleCollider, MonsterCapsuleCollider, true);
        Physics2D.IgnoreCollision(PlayerBoxCollider, MonsterCapsuleCollider, true);
    }

    void Update()
    {
        if (!IsAlive) {
            return;
        }
        Die();
        if (CanWalk) {
            Move();
        }
    }
    
    void Move() {
        if (!IsAlive) {
            return;
        }
        MonsterRigidbody.velocity = new Vector2 (MoveSpeed, 0f);
    }

    IEnumerator RandomFlip() { // 랜덤한 시간마다 스프라이트 반전 호출
        while (IsAlive) {
            float WaitTime = Random.Range(FlipTimeHead, FlipTimeRear);
            yield return new WaitForSeconds(WaitTime);

            FlipEnemyFacing();
        }
    }

    IEnumerator RandomStop() { // 랜덤한 시간마다 이동 멈춤 호출
        while (IsAlive) {
            float WaitTime = Random.Range(FlipTimeHead, FlipTimeRear);
            yield return new WaitForSeconds(WaitTime);

            StopWalking();
        }
    }
    void FlipEnemyFacing() { // 스프라이트 반전
        if (MonsterRigidbody.velocity.x == 0) {
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
        CanWalk = true;
    }

    void OnTriggerStay2D(Collider2D other) { // 몬스터와 플레이어 겹쳤을 때 데미지
        if (other.tag == "Player" && IsAlive) {
            PlayerMovement.Hurt(MonsterStatus.MonsterDamage);
        }
    }
    void Die() {
        if (MonsterStatus.MonsterCurrentHealth > 0) {
            return;
        }
        MonsterAnimator.SetBool("IsDying", true);
        IsAlive = false;
        PlayerStatus.GainEXP(MonsterStatus.MonsterEXP);
        StartCoroutine(DestroyAfterAnimation(DieDelay)); // 애니메이션 재생 후 몬스터 파괴
    }

    IEnumerator DestroyAfterAnimation(float DieDelay) {
    yield return new WaitForSeconds(DieDelay);
    Destroy(gameObject);
}
}
