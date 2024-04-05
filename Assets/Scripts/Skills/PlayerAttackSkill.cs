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
    MonsterStatus MonsterStatus = other.gameObject.GetComponent<MonsterStatus>();
    if (MonsterStatus != null) {
        MonsterStatus.MonsterCurrentHealth -= Damage;
    }
}
void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject.tag == "Ground") {
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