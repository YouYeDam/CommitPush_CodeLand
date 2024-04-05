using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMonsterMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;
    [SerializeField] float FlipTimeHead = 1f;
    [SerializeField] float FlipTimeRear = 1f;
    [SerializeField] float WaitCanWalk = 1f;
    Rigidbody2D MyRigidbody;
    Animator MyAnimator;

    bool CanWalk = true;
    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
        StartCoroutine(RandomFlip());
        StartCoroutine(RandomStop());
    }

    void Update()
    {
        if (CanWalk) {
            MyRigidbody.velocity = new Vector2 (MoveSpeed, 0f);
        }
    }
    
    IEnumerator RandomFlip() { // 랜덤한 시간마다 스프라이트 반전 호출
        while (true) {
            float WaitTime = Random.Range(FlipTimeHead, FlipTimeRear);
            yield return new WaitForSeconds(WaitTime);

            FlipEnemyFacing();
        }
    }

    IEnumerator RandomStop() { // 랜덤한 시간마다 이동 멈춤 호출
        while (true) {
            float WaitTime = Random.Range(FlipTimeHead, FlipTimeRear);
            yield return new WaitForSeconds(WaitTime);

            StopWalking();
        }
    }
    void FlipEnemyFacing() { // 스프라이트 반전
        if (MyRigidbody.velocity.x == 0) {
            return;
        }
        MoveSpeed = -MoveSpeed;
        transform.localScale = new Vector2 (Mathf.Sign(MyRigidbody.velocity.x), 1f); 
    }

    void StopWalking() { // 이동 멈춤
        MyRigidbody.velocity = new Vector2 (0f, 0f);
        CanWalk = false;
        MyAnimator.SetBool("IsIdling", true);
        Invoke("RestartWalking", WaitCanWalk);
    }

    void RestartWalking() { // 재이동 시작
        MyAnimator.SetBool("IsIdling", false);
        CanWalk = true;
    }
}
