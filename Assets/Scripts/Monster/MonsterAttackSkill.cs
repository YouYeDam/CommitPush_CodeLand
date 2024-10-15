using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackSkill : MonoBehaviour
{
    [SerializeField] float SkillSpeed = 1f;
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] int Damage = 10;
    [SerializeField] bool IsHoming = false; // 유도탄 여부를 결정하는 변수
    [SerializeField] bool IsRotationing = false; // 회전하는 물체인지를 경정하는 변수
    [SerializeField] bool IsDropSkill = false; // 낙하물 투하 스킬 여부를 결정하는 변수
    [SerializeField] float DropPos = 10f; // 플레이어의 머리 위로부터 생성 위치
    Rigidbody2D MyRigidbody;
    public BasicMonsterMovement BasicMonsterMovement;
    public MonsterStatus MonsterStatus;
    bool IsAttackDone = false;
    [SerializeField] bool IsSlowingSkill;
    [SerializeField] float SlowTime = 3f;
    [SerializeField] float SlowFactor = 0.5f; // 이동속도 감소율
    [SerializeField] float RotationSpeed = 360f; // 발사체 회전 속도
    [SerializeField] float DestroyTime = 0.15f;

    public bool IsLeft = false;
    float XSpeed;
    GameObject Player;

    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");

        if (!IsHoming && !IsDropSkill) {
            if (IsLeft) {
                XSpeed = -BasicMonsterMovement.transform.localScale.x * SkillSpeed;
            }
            else {
                XSpeed = BasicMonsterMovement.transform.localScale.x * SkillSpeed;
            }
        }
        else if (IsDropSkill) {
            Vector3 dropPosition = new Vector3(Player.transform.position.x, Player.transform.position.y + DropPos, Player.transform.position.z);
            transform.position = dropPosition;
        }

        Invoke("DestroySelf", DestroyDelay);
    }

    void Update()
    {
        if (!IsHoming && !IsDropSkill) { // 기본 스킬
            MyRigidbody.velocity = new Vector2(XSpeed, 0f);
        }
        else if (IsHoming && Player != null) { // 유도 스킬
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            MyRigidbody.velocity = direction * SkillSpeed;
        }
        else if (IsDropSkill) { // 낙하물 스킬
            MyRigidbody.velocity = new Vector2(0f, -SkillSpeed);
        }

        FlipSprite();

        if (IsRotationing) { // 발사체가 회전하는 스킬이면 회전
            transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other) { // 플레이어를 피격할 시 실행
        if (IsAttackDone) { // 중복해서 피격당하는 버그 방지
            return;
        }

        if (other is BoxCollider2D && other.gameObject.tag == "Player") {
            IsAttackDone = true;
            PlayerMovement PlayerMovement = other.gameObject.GetComponent<PlayerMovement>();
            PlayerMovement.TakeDamage(Damage);

            if (IsSlowingSkill) { // 슬로우 디버프 스킬 시 실행
                PlayerMovement.SpeedChange(SlowTime, SlowFactor, SlowFactor, SlowFactor);
            }
        }

        if (other.gameObject.tag == "Player") { // 플레이어 피격 후 대기시간 이후 발사체 삭제
            Invoke("DestroySelf", DestroyTime);
        }
    }

    void FlipSprite() { // 발사체 뒤집기
        if (MyRigidbody.velocity.x < 0) {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (MyRigidbody.velocity.x > 0) {
            transform.localScale = new Vector2(1, 1);
        }
    }

    void DestroySelf() { // 발사체 삭제
        Destroy(gameObject);
    }
}
