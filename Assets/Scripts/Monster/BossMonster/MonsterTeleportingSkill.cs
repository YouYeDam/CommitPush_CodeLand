using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTeleportingSkill : MonoBehaviour
{
    [SerializeField] float TeleportDelayTime = 0.35f;
    [SerializeField] float DestroyEffectTime = 0.5f;
    GameObject TeleportEffectInstance;
    [SerializeField] GameObject TeleportEffect;
    [SerializeField] Transform TeleportSpot;
    Animator MyAnimator;
    BasicMonsterMovement BasicMonsterMovement;
    GameObject Player;

    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        BasicMonsterMovement = GetComponent<BasicMonsterMovement>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void MonsterTeleportSkill() { // 몬스터 텔레포트 스킬
        MyAnimator.SetBool("IsTeleporting", true);
        BasicMonsterMovement.CanWalk = false;
        BasicMonsterMovement.IsSkilling = true;
        Vector3 TeleportPosition = Player.transform.position;
        StartCoroutine(TeleportRoutine(TeleportPosition));
    }

    IEnumerator TeleportRoutine(Vector3 TeleportPosition) { // 대기시간 후 텔레포트 스킬 사용
        yield return new WaitForSeconds(TeleportDelayTime);
        MyAnimator.SetBool("IsTeleporting", false);
        BasicMonsterMovement.CanWalk = true;
        BasicMonsterMovement.IsSkilling = false;
        InstantiateEffect();
        transform.position = TeleportPosition;
    }

    void InstantiateEffect() { // 텔레포트 이펙트 연출
        TeleportEffectInstance = Instantiate(TeleportEffect, TeleportSpot.position, transform.rotation);
        Invoke("DestroyEffect", DestroyEffectTime);
    }

    void DestroyEffect() { // 대기시간 후 이펙트 삭제
        Destroy(TeleportEffectInstance);
    }
}
