using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTeleportingSkill : MonoBehaviour
{
    [SerializeField] float BackToIdleAnimTime = 0.35f;
    [SerializeField] float TeleportDelayTime = 1f;
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

    public void MonsterTeleportSkill() {
        MyAnimator.SetBool("IsTeleporting", true);
        BasicMonsterMovement.CanWalk = false;
        BasicMonsterMovement.IsSkilling = true;
        Vector3 TeleportPosition = Player.transform.position;
        StartCoroutine(TeleportRoutine(TeleportPosition));
    }

    IEnumerator TeleportRoutine(Vector3 TeleportPosition)
    {
        yield return new WaitForSeconds(BackToIdleAnimTime);
        MyAnimator.SetBool("IsTeleporting", false);
        BasicMonsterMovement.CanWalk = true;
        BasicMonsterMovement.IsSkilling = false;
        InstantiateEffect();
        transform.position = TeleportPosition;
    }

    void InstantiateEffect() {
        TeleportEffectInstance = Instantiate(TeleportEffect, TeleportSpot.position, transform.rotation);
        Invoke("DestroyEffect", DestroyEffectTime);
    }

    void DestroyEffect() {
        Destroy(TeleportEffectInstance);
    }
}
