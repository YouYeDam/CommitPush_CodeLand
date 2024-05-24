using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackSkill : MonoBehaviour
{
    [SerializeField] float SkillSpeed = 1f;
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] int Damage = 10;
    [SerializeField] bool IsHoming = false; // 유도탄 여부를 결정하는 변수
    Rigidbody2D MyRigidbody;
    public BasicMonsterMovement BasicMonsterMovement;
    public MonsterStatus MonsterStatus;
    bool IsAttackDone = false;
    [SerializeField] bool IsSlowingSkill;
    [SerializeField] float SlowTime = 3f;
    [SerializeField] float SlowFactor = 0.5f;
    public bool IsLeft = false;
    float XSpeed;
    GameObject Player;

    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player");

        if (!IsHoming)
        {
            if (IsLeft)
            {
                XSpeed = -BasicMonsterMovement.transform.localScale.x * SkillSpeed;
            }
            else
            {
                XSpeed = BasicMonsterMovement.transform.localScale.x * SkillSpeed;
            }
        }

        Invoke("DestroySelf", DestroyDelay);
        FlipSprite();
    }

    void Update()
    {
        if (!IsHoming)
        {
            MyRigidbody.velocity = new Vector2(XSpeed, 0f);
        }
    }

    void FixedUpdate()
    {
        if (IsHoming && Player != null)
        {
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            MyRigidbody.velocity = direction * SkillSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsAttackDone)
        {
            return;
        }
        if (other is BoxCollider2D && other.gameObject.tag == "Player")
        {
            IsAttackDone = true;
            PlayerMovement PlayerMovement = other.gameObject.GetComponent<PlayerMovement>();
            PlayerMovement.TakeDamage(Damage);
            if (IsSlowingSkill)
            {
                PlayerMovement.ApplySlow(SlowTime, SlowFactor);
            }
        }
        if (other.gameObject.tag == "Player")
        {
            Invoke("DestroySelf", 0.15f);
        }
    }

    void FlipSprite()
    {
        if (XSpeed < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
