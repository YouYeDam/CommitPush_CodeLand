using UnityEngine;

public class PlayerAttackSkill : MonoBehaviour
{
[SerializeField] float SkillSpeed = 1f;
[SerializeField] float DestroyDelay = 0.5f;
[SerializeField] int Damage = 10;
[SerializeField] float SkillCoefficient = 0.1f; // 스킬계수
Rigidbody2D MyRigidbody;
PlayerMovement PlayerMovement;
PlayerStatus PlayerStatus;
bool IsAttack = false;
bool IsCrit = false;
float xSpeed;
void Start()
{
    MyRigidbody = GetComponent<Rigidbody2D>();
    PlayerMovement = FindObjectOfType<PlayerMovement>();
    xSpeed = PlayerMovement.transform.localScale.x * SkillSpeed;
    Invoke("DestroySelf", DestroyDelay);
    FlipSprite();
    
    PlayerStatus = FindObjectOfType<PlayerStatus>();    
    Damage = Mathf.CeilToInt(Damage * (1 + SkillCoefficient * PlayerStatus.PlayerATK)); // 데미지 공식: 스킬계수 * 플레이어ATK
    Damage = Mathf.FloorToInt(Damage * Random.Range(1.0f, 1.31f)); // 데미지 랜덤값: 계산된 데미지의 1 ~ 1.3배로 조정
    if (Random.value < PlayerStatus.PlayerCrit) {
        IsCrit = true;
        Damage *= 2; // 크리티컬 공식: 최종 데미지 * 2
    }
}
void Update()
{
    MyRigidbody.velocity = new Vector2 (xSpeed, 0f);
    
}

void OnTriggerEnter2D(Collider2D other) {
    if (IsAttack) {
        return;
    }
    if (other is BoxCollider2D && other.gameObject.tag == "Monster") {
        IsAttack = true;
        BasicMonsterMovement BaiscMonsterMovement = other.gameObject.GetComponent<BasicMonsterMovement>();
        BaiscMonsterMovement.TakeDamage(Damage);

        MonsterTakeDamageDisplay MonsterTakeDamageDisplay = other.gameObject.GetComponent<MonsterTakeDamageDisplay>();
        MonsterTakeDamageDisplay.DisplayDamageBar(Damage, IsCrit);
    }
    if(other.gameObject.tag == "Monster") {
        Invoke("DestroySelf", 0.1f);
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