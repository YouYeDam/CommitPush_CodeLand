using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetSkill : MonoBehaviour
{
    [SerializeField] float SkillSpeed = 1f;
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] int Damage = 10;
    [SerializeField] float SkillCoefficient = 0.05f; // 스킬계수
    [SerializeField] float AttackXRange = 5f;
    [SerializeField] float AttackYRange = 0.5f;
    Rigidbody2D MyRigidbody;
    GameObject Player;
    PlayerMovement PlayerMovement;
    PlayerStatus PlayerStatus;
    bool IsAttackDone = false;
    [SerializeField] bool CanHitMany = false;
    bool IsCrit = false;
    public float CoolDown = 3f;
    public int MPUse = 0;
    float XSpeed;
    GameObject TargetMonster;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MyRigidbody = GetComponent<Rigidbody2D>();
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        XSpeed = PlayerMovement.transform.localScale.x * SkillSpeed;
        Invoke("DestroySelf", DestroyDelay);
        FlipSprite();
        
        PlayerStatus = FindObjectOfType<PlayerStatus>();    
        Damage = Mathf.CeilToInt(Damage * (1 + SkillCoefficient * PlayerStatus.PlayerATK)); // 데미지 공식: 스킬계수 * 플레이어ATK
        Damage = Mathf.FloorToInt(Damage * Random.Range(1.0f, 1.31f)); // 데미지 랜덤값: 계산된 데미지의 1 ~ 1.3배로 조정
        if (Random.value < PlayerStatus.PlayerCrit) {
            IsCrit = true;
            Damage *= 2; // 크리티컬 공식: 최종 데미지 * 2
        }

        CheckMonster(); // 시작 시 오브젝트 위치를 타겟 몬스터의 위치로 변경
    }

    void Update()
    {
        MyRigidbody.velocity = new Vector2(XSpeed, 0f);

        // Update에서 타겟 몬스터 위치로 지속적으로 갱신
        if (TargetMonster != null)
        {
            transform.position = TargetMonster.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (IsAttackDone) {
            return;
        }
        if (other is BoxCollider2D && other.gameObject.tag == "Monster") {
            BasicMonsterMovement BaiscMonsterMovement = other.gameObject.GetComponent<BasicMonsterMovement>();
            BaiscMonsterMovement.TakeDamage(Damage, IsCrit);
            if (!CanHitMany) {
                IsAttackDone = true;
            }
        }
    }

    void FlipSprite() { //스킬 이펙트 좌우반전
        if (XSpeed < 0) {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void CheckMonster()
    {
        Vector2 Direction = PlayerMovement.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 Origin = Player.transform.position;
        Vector2 Size = new Vector2(AttackXRange, AttackYRange);
        float Angle = 0f;

        RaycastHit2D[] Hits = Physics2D.BoxCastAll(Origin, Size, Angle, Direction, AttackXRange, LayerMask.GetMask("Monster"));
        foreach (RaycastHit2D Hit in Hits)
        {
            if (Hit.collider != null && Hit.collider.CompareTag("Monster"))
            {
                BasicMonsterMovement monsterMovement = Hit.collider.GetComponent<BasicMonsterMovement>();
                if (monsterMovement != null && monsterMovement.IsAlive)
                {
                    TargetMonster = Hit.collider.gameObject;
                    return;
                }
            }
        }

        // 몬스터가 없으면 Player로부터 AttackXRange 지점 끝에 위치하게 설정
        GameObject TargetPoint = new GameObject("TargetPoint");
        TargetPoint.transform.position = Player.transform.position + new Vector3(Direction.x * AttackXRange, 0, 0);
        TargetMonster = TargetPoint;
    }

    void DestroySelf()
    {
        if (TargetMonster != null && TargetMonster.name == "TargetPoint")
        {
            Destroy(TargetMonster);
        }
        Destroy(gameObject);
    }
}
