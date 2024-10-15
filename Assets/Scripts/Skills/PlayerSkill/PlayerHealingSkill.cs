using System.Collections;
using UnityEngine;

public class PlayerHealingSkill : MonoBehaviour
{
    [SerializeField] float DestroyDelay = 3f;
    [SerializeField] float HealHPAmount = 0.1f; // 전체 체력 대비 퍼센티지
    GameObject Player;
    PlayerStatus PlayerStatus;
    PlayerMovement PlayerMovement;
    public float CoolDown = 60f;
    public int MPUse = 0;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        StartCoroutine(Heal());
        Invoke("DestroySelf", DestroyDelay); 
    }

    void Update() { // 스킬 이펙트 플레이어 위치에 따라 갱신
        float PlayerX = Player.transform.position.x;
        float PlayerY = Player.transform.position.y + 1.5f;
        transform.position = new Vector3(PlayerX, PlayerY, transform.position.z);
    }

    IEnumerator Heal() { // 힐 기능 구현
        float ElapsedTime = 0f;

        while (ElapsedTime < DestroyDelay) {
            if (!PlayerMovement.IsAlive) {
                DestroySelf();
                yield break; // 코루틴 종료
            }

            int HealHPAmountToInt = Mathf.RoundToInt(PlayerStatus.PlayerMaxHP * HealHPAmount); // 반올림 적용
            PlayerStatus.PlayerCurrentHP += HealHPAmountToInt;

            if (PlayerStatus.PlayerCurrentHP > PlayerStatus.PlayerMaxHP) { // 플레이어 최대 체력을 넘을 수 없도록
                PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP;
            }
            yield return new WaitForSeconds(1f);
            ElapsedTime += 1f;
        }
    }

    void DestroySelf() { // 스킬 파괴
        Destroy(gameObject);
    }
}
