using System.Collections;
using UnityEngine;

public class BossMonsterBehavior : MonoBehaviour
{
    [SerializeField] bool IsSummonBoss = false; // 소환 스킬을 가진 보스인지
    [SerializeField] bool IsThrowingBoss = false; // 던지기 스킬을 가진 보스인지
    [SerializeField] bool IsDroppingBoss = false; // 낙하물 투하 스킬을 가진 보스인지
    [SerializeField] bool IsTeleportingBoss = false; // 텔레포트 스킬을 가진 보스인지
    [SerializeField] float SummonWaitTime = 20f; // 소환 스킬 쿨타임
    [SerializeField] float ThrowingWaitTime = 10f; // 던지기 스킬 쿨타임
    [SerializeField] float DroppingWaitTime = 20f; // 낙하물 투하 스킬 쿨타임
    [SerializeField] float TeleportingWaitTime = 30f; // 텔레포트 스킬 쿨타임
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

    IEnumerator SummonSkill() { // 소환 스킬
        while (PlayerMovement.IsAlive && MonsterSummoningSkill.SummonCount < MonsterSummoningSkill.MaxSummonCount) {
            yield return new WaitForSeconds(SummonWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterSummoningSkill.SummonMonsters();
        }
    }

    IEnumerator ThrowingSkill() { // 던지기 스킬
        while (PlayerMovement.IsAlive) {
            yield return new WaitForSeconds(ThrowingWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterThrowingSkill.ShootSkill();
        }
    }

    IEnumerator DroppingSkill() { // 낙하물 투하 스킬
        while (PlayerMovement.IsAlive) {
            yield return new WaitForSeconds(DroppingWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterDroppingSkill.ShootSkill();
        }
    }

    IEnumerator TeleportingSkill() { // 텔레포트 스킬
        while (PlayerMovement.IsAlive) {
            yield return new WaitForSeconds(TeleportingWaitTime);

            while (BasicMonsterMovement.IsSkilling) {
                yield return new WaitForSeconds(5f); // 스킬을 사용하는 중이면 추가 대기 시간
            }
            MonsterTeleportingSkill.MonsterTeleportSkill();
        }
    }
}
