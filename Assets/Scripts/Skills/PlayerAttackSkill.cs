using UnityEngine;

public class PlayerAttackSkill : MonoBehaviour
{
    [SerializeField] float SkillSpeed = 1f;
    [SerializeField] float DestroyDelay = 0.5f;
    [SerializeField] int Damage = 10;
    [SerializeField] float SkillCoefficient = 0.05f; // 스킬계수
    Rigidbody2D MyRigidbody;
    PlayerMovement PlayerMovement;
    PlayerStatus PlayerStatus;
    bool IsAttackDone = false;
    bool IsCrit = false;
    public float CoolDown = 3f;
    public int MPUse = 0;
    [SerializeField] float DestroyTime = 0.1f;
    float XSpeed;
    void Start()
    {
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
    }
    void Update()
    {
        MyRigidbody.velocity = new Vector2 (XSpeed, 0f);
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (IsAttackDone) {
            return;
        }
        if (other is BoxCollider2D && other.gameObject.tag == "Monster") {
            IsAttackDone = true;
            BasicMonsterMovement BaiscMonsterMovement = other.gameObject.GetComponent<BasicMonsterMovement>();
            BaiscMonsterMovement.TakeDamage(Damage);

            MonsterTakeDamageDisplay MonsterTakeDamageDisplay = other.gameObject.GetComponent<MonsterTakeDamageDisplay>();
            MonsterTakeDamageDisplay.DisplayDamageBar(Damage, IsCrit);
            Invoke("DestroySelf", DestroyTime);
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