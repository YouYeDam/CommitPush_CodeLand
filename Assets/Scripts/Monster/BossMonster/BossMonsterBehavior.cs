using System.Collections;
using UnityEngine;

public class BossMonsterBehavior : MonoBehaviour
{
    [SerializeField] bool IsSummonBoss = false; // 소환 스킬을 가진 보스인지
    [SerializeField] bool IsThrowingBoss = false; // 던지기 스킬을 가진 보스인지
    [SerializeField] bool IsDroppingBoss = false; // 낙하물 투하 스킬을 가진 보스인지
    [SerializeField] bool IsTeleportingBoss = false; // 텔레포트 스킬을 가진 보스인지
    [SerializeField] float SummonWaitTime = 20f;
    [SerializeField] float ThrowingWaitTime = 10f;
    [SerializeField] float DroppingWaitTime = 20f;
    [SerializeField] float TeleportingWaitTime = 30f;
    MonsterSummoningSkill MonsterSummoningSkill;
    MonsterThrowingSkill MonsterThrowingSkill;
    MonsterDroppingSkill MonsterDroppingSkill;
    MonsterTeleportingSkill MonsterTeleportingSkill;
    PlayerMovement PlayerMovement;
    BasicMonsterMovement BasicMonsterMovement;

    void Start()
    {
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        BasicMonsterMovement = GetComponent<BasicMonsterMovement>();

        if (IsSummonBoss) {
            MonsterSummoningSkill = GetComponent<MonsterSummoningSkill>();
            StartCoroutine(SummonSkill());
        }

        if (IsThrowingBoss) {
            MonsterThrowingSkill = GetComponent<MonsterThrowingSkill>();
            StartCoroutine(ThrowingSkill());
        }

        if (IsDroppingBoss) {
            MonsterDroppingSkill = GetComponent<MonsterDroppingSkill>();
            StartCoroutine(DroppingSkill());    
        }

        if (IsTeleportingBoss) {
            MonsterTeleportingSkill = GetComponent<MonsterTeleportingSkill>();
            StartCoroutine(TeleportingSkill());    
        }
    }

    IEnumerator SummonSkill() {
        while (PlayerMovement.IsAlive && MonsterSummoningSkill.SummonCount < MonsterSummoningSkill.MaxSummonCount) {
            yield return new WaitForSeconds(SummonWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterSummoningSkill.SummonMonsters();
        }
    }

    IEnumerator ThrowingSkill() {
        while (PlayerMovement.IsAlive) {
            yield return new WaitForSeconds(ThrowingWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterThrowingSkill.ShootSkill();
        }
    }

    IEnumerator DroppingSkill() {
        while (PlayerMovement.IsAlive) {
            yield return new WaitForSeconds(DroppingWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterDroppingSkill.ShootSkill();
        }
    }

    IEnumerator TeleportingSkill() {
        while (PlayerMovement.IsAlive) {
            yield return new WaitForSeconds(TeleportingWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterTeleportingSkill.MonsterTeleportSkill();
        }
    }
}
