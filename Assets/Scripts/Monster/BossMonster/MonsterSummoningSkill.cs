using System.Collections;
using UnityEngine;

public class MonsterSummoningSkill : MonoBehaviour
{
    [SerializeField] float SummonBackToIdleAnimTime = 0.35f;
    [SerializeField] float SummonDelayTime = 1f;
    [SerializeField] public int SummonCount = 0; // 현재 소환한 횟수
    [SerializeField] public int MaxSummonCount = 30; // 최대 소환 가능 개체수
    public Transform SummonSpot;
    Animator MyAnimator;
    BasicMonsterMovement BasicMonsterMovement;
    GenerateMonster GenerateMonster;

    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
        GenerateMonster = GetComponent<GenerateMonster>();
    }

    public void SummonMonsters() { // 몬스터 소환
        if (SummonCount >= MaxSummonCount) { // 몬스터를 최대 소환 가능 개체수 이상만큼 소환하면 더이상 소환 X
            return;
        }

        MyAnimator.SetBool("IsSummoning", true);
        BasicMonsterMovement.CanWalk = false;
        BasicMonsterMovement.IsSkilling = true;

        StartCoroutine(SummonRoutine());
    }

    IEnumerator SummonRoutine() { // 대기시간 후 몬스터 소환
        yield return new WaitForSeconds(SummonBackToIdleAnimTime);
        MyAnimator.SetBool("IsSummoning", false);
        BasicMonsterMovement.CanWalk = true;
        BasicMonsterMovement.IsSkilling = false;

        yield return new WaitForSeconds(SummonDelayTime);
        GenerateMonster.GenerateMonsters(SummonSpot.position);
        SummonCount++;
    }
}
