using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackSkill : MonoBehaviour
{
    [SerializeField] float SkillSpeed = 1f;
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] int Damage = 10;
    Rigidbody2D MyRigidbody;
    public BasicMonsterMovement BasicMonsterMovement;
    public MonsterStatus MonsterStatus;
    bool IsAttackDone = false;
    public bool IsLeft = false;
    float XSpeed;
    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        if (IsLeft) {
            XSpeed = -BasicMonsterMovement.transform.localScale.x * SkillSpeed;
        }
        else {
            XSpeed = BasicMonsterMovement.transform.localScale.x * SkillSpeed;
        }
        Invoke("DestroySelf", DestroyDelay);
        FlipSprite();
        Damage = MonsterStatus.MonsterDamage * 2;
    }
    void Update()
    {
        MyRigidbody.velocity = new Vector2 (XSpeed, 0f);
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (IsAttackDone) {
            return;
        }
        if (other is BoxCollider2D && other.gameObject.tag == "Player") {
            IsAttackDone = true;
            PlayerMovement PlayerMovement = other.gameObject.GetComponent<PlayerMovement>();
            PlayerMovement.TakeDamage(Damage);
        }
        if(other.gameObject.tag == "Player") {
            Invoke("DestroySelf", 0.15f);
        }
    }

    void FlipSprite() { //스킬 이펙트 좌우반전
        if (XSpeed < 0) {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
