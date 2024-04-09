using UnityEngine;

public class PlayerAttackSkill : MonoBehaviour
{
[SerializeField] float SkillSpeed = 1f;
[SerializeField] float DestroyDelay = 0.5f;
[SerializeField] int Damage = 10;
Rigidbody2D MyRigidbody;
PlayerMovement Player;

float xSpeed;
void Start()
{
    MyRigidbody = GetComponent<Rigidbody2D>();
    Player = FindObjectOfType<PlayerMovement>();
    xSpeed = Player.transform.localScale.x * SkillSpeed;
    Invoke("DestroySelf", DestroyDelay);
    FlipSprite();
}
void Update()
{
    MyRigidbody.velocity = new Vector2 (xSpeed, 0f);
    
}

void OnTriggerEnter2D(Collider2D other) {
    if (other is BoxCollider2D && other.gameObject.tag == "Monster") {
        MonsterStatus MonsterStatus = other.gameObject.GetComponent<MonsterStatus>();
        if (MonsterStatus != null) {
            MonsterStatus.MonsterCurrentHealth -= Damage;
        }
    }
    if(other.gameObject.tag == "Monster") {
        Destroy(gameObject);    
    }
}

void FlipSprite() { //스킬 이펙트 좌우반전
if (xSpeed < 0) {
    transform.localScale = new Vector2(-1, 1);
}
}

void DestroySelf()
    {
        Destroy(gameObject);
    }
}