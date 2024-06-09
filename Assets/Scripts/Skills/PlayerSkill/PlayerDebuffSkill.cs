using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuffSkill : MonoBehaviour
{
    [SerializeField] float SkillSpeed = 1f;
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] float AttackDistance = 5f; // 캐스트 거리
    [SerializeField] float BoxHeight = 0.5f; // 박스의 높이
    [SerializeField] float BoxWidth = 1f; // 박스의 너비
    Rigidbody2D MyRigidbody;
    GameObject Player;
    PlayerMovement PlayerMovement;
    bool IsAttackDone = false;
    [SerializeField] bool CanHitMany = false;
    [SerializeField] float DebuffDuration = 10f;
    [SerializeField] float SlowDebuffFactor = 0.5f;
    public float CoolDown = 3f;
    public int MPUse = 0;
    float XSpeed;
    GameObject TargetMonster;
    Vector3 TargetPosition;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        MyRigidbody = GetComponent<Rigidbody2D>();
        PlayerMovement = FindObjectOfType<PlayerMovement>();
        XSpeed = PlayerMovement.transform.localScale.x * SkillSpeed;
        Invoke("DestroySelf", DestroyDelay);
        FlipSprite();
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
        else
        {
            transform.position = TargetPosition;
        }
        if (TargetMonster != null && !TargetMonster.GetComponent<BasicMonsterMovement>().IsAlive) {
            DestroySelf();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAttackDone)
        {
            return;
        }
        if (other is BoxCollider2D && other.gameObject.tag == "Monster")
        {
            BasicMonsterMovement MonsterMovement = other.gameObject.GetComponent<BasicMonsterMovement>();
            MonsterMovement.MonsterSlowDebuff(DebuffDuration, SlowDebuffFactor);
            if (!CanHitMany)
            {
                IsAttackDone = true;
            }
        }
    }

    void FlipSprite()
    {
        if (XSpeed < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void CheckMonster()
    {
        Vector2 Origin = Player.transform.position;
        Vector2 Direction = PlayerMovement.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 Size = new Vector2(BoxWidth, BoxHeight);
        float Angle = 0f;

        RaycastHit2D[] Hits = Physics2D.BoxCastAll(Origin, Size, Angle, Direction, AttackDistance, LayerMask.GetMask("Monster"));
        float MinDistance = float.MaxValue;
        GameObject ClosestMonster = null;

        foreach (RaycastHit2D Hit in Hits)
        {
            if (Hit.collider != null && Hit.collider.CompareTag("Monster"))
            {
                BasicMonsterMovement MonsterMovement = Hit.collider.GetComponent<BasicMonsterMovement>();
                if (MonsterMovement != null && MonsterMovement.IsAlive)
                {
                    float Distance = Vector2.Distance(Origin, Hit.point);
                    if (Distance < MinDistance)
                    {
                        MinDistance = Distance;
                        ClosestMonster = Hit.collider.gameObject;
                    }
                }
            }
        }

        if (ClosestMonster != null)
        {
            TargetMonster = ClosestMonster;
        }
        else
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
