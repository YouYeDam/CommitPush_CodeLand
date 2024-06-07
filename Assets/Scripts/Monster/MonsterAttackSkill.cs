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
    Rigidbody2D MyRigidbody;
    public BasicMonsterMovement BasicMonsterMovement;
    public MonsterStatus MonsterStatus;
    bool IsAttackDone = false;
    [SerializeField] bool IsSlowingSkill;
    [SerializeField] float SlowTime = 3f;
    [SerializeField] float SlowFactor = 0.5f;
    [SerializeField] float RotationSpeed = 360f;
    [SerializeField] float DestroyTime = 0.15f;

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
    }

    void Update()
    {
        if (!IsHoming)
        {
            MyRigidbody.velocity = new Vector2(XSpeed, 0f);
        }
        else if (Player != null)
        {
            Vector2 direction = (Player.transform.position - transform.position).normalized;
            MyRigidbody.velocity = direction * SkillSpeed;
        }

        FlipSprite();

        if (IsRotationing)
        {
            transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
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
                PlayerMovement.SpeedChange(SlowTime, SlowFactor, SlowFactor, SlowFactor);
            }
        }
        if (other.gameObject.tag == "Player")
        {
            Invoke("DestroySelf", DestroyTime);
        }
    }

    void FlipSprite()
    {
        if (MyRigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (MyRigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
