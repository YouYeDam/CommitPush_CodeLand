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

    void Update() {
        float PlayerX = Player.transform.position.x;
        float PlayerY = Player.transform.position.y + 1.5f;
        transform.position = new Vector3(PlayerX, PlayerY, transform.position.z);
    }

    IEnumerator Heal() {
        float elapsedTime = 0f;
        while (elapsedTime < DestroyDelay) {
            if (!PlayerMovement.IsAlive) {
                DestroySelf();
                yield break; // 코루틴 종료
            }
            int HealHPAmountToInt = Mathf.RoundToInt(PlayerStatus.PlayerMaxHP * HealHPAmount);
            PlayerStatus.PlayerCurrentHP += HealHPAmountToInt;
            if (PlayerStatus.PlayerCurrentHP > PlayerStatus.PlayerMaxHP) {
                PlayerStatus.PlayerCurrentHP = PlayerStatus.PlayerMaxHP;
            }
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
    }

    void DestroySelf() {
        Destroy(gameObject);
    }
}
