using System.Collections;
using UnityEngine;

public class MonsterSummoningSkill : MonoBehaviour
{
    [SerializeField] float SummonBackToIdleAnimTime = 0.35f;
    [SerializeField] float SummonDelayTime = 1f;
    [SerializeField] public int SummonCount = 0;
    [SerializeField] public int MaxSummonCount = 30;
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

    public void SummonMonsters()
    {
        if (SummonCount >= MaxSummonCount)
        {
            return;
        }

        MyAnimator.SetBool("IsSummoning", true);
        BasicMonsterMovement.CanWalk = false;
        BasicMonsterMovement.IsSkilling = true;

        StartCoroutine(SummonRoutine());
    }

    IEnumerator SummonRoutine()
    {
        yield return new WaitForSeconds(SummonBackToIdleAnimTime);
        MyAnimator.SetBool("IsSummoning", false);
        BasicMonsterMovement.CanWalk = true;
        BasicMonsterMovement.IsSkilling = false;

        yield return new WaitForSeconds(SummonDelayTime);
        GenerateMonster.GenerateMonsters(SummonSpot.position);
        SummonCount++;
    }
}
