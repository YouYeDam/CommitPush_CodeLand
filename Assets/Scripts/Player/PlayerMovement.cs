using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
Vector2 MoveInput;
Rigidbody2D MyRigidbody;
Animator MyAnimator;
CapsuleCollider2D MyCapsuleCollider;
BoxCollider2D MyBoxCollider;
public bool IsWalkingAllowed = true;

float StartGravity;
[SerializeField] float WalkDelay = 0.2f; // Walk 함수 호출을 지연할 시간
[SerializeField] float RunSpeed = 10f;
[SerializeField] float JumpSpeed = 5f;
[SerializeField] float ClimbSpeed = 5f;

void Start()
{
    MyRigidbody = GetComponent<Rigidbody2D>();
    MyAnimator = GetComponent<Animator>();
    MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
    MyBoxCollider = GetComponent<BoxCollider2D>();

    StartGravity = MyRigidbody.gravityScale;
}

void Update()
{
    if (IsWalkingAllowed)
    {
        Walk();
    }
    else
    {
        MyRigidbody.velocity = new Vector2(0, MyRigidbody.velocity.y);
        StartCoroutine(WalkWithDelay());
    }
    CheckWalk();
    ClimbLadder();
    FlipSprite();
}
void OnMove(InputValue Value) {
    MoveInput = Value.Get<Vector2>();
}

void Walk() { //플레이어 좌우이동
    bool PlayerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon;
    Vector2 PlayerVelocity = new Vector2 (MoveInput.x * RunSpeed, MyRigidbody.velocity.y); // 현재의 속도인 y가 무엇이든 동일한 속도를 유지하라는 의미
    MyRigidbody.velocity = PlayerVelocity;
    
    MyAnimator.SetBool("IsWalking", PlayerHasHorizontalSpeed);
} 

void CheckWalk() { //걷기 애니메이션 체크
    bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    if (!IsOnGround && !IsOnLadder) {
        MyAnimator.SetBool("IsWalking", false);
        MyAnimator.SetBool("IsJumping", true);
    }
    else {
        MyAnimator.SetBool("IsJumping", false);   
    }
}
void FlipSprite() { //플레이어 좌우반전
    bool PlayerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon; // abs: 절대값 반환, Epsilon: 0에 가까운 아주 작은 값 반환

    if (PlayerHasHorizontalSpeed) {
        transform.localScale = new Vector2 (Mathf.Sign(MyRigidbody.velocity.x), 1f); // 0보다 크면1, 작으면 -1 반환
    }
}

void OnJump(InputValue Value) { //플레이어 점프
    bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    if (!IsOnGround) {
        return;
    }
    if (Value.isPressed) {
        MyRigidbody.velocity += new Vector2 (0f, JumpSpeed);
    }
}

void ClimbLadder() { //플레이어 사다리 타기
    bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    if (IsOnGround && MoveInput.y == 0) {
        return;
    }
    if (!IsOnLadder) {
        MyAnimator.SetBool("IsClimbing", false);
        MyAnimator.SetBool("IsClimbingIdle", false);
        MyRigidbody.gravityScale = StartGravity;
        return;
    }
    float LadderSpeed = MoveInput.y * ClimbSpeed;
    MyRigidbody.gravityScale = 0f;
    MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, LadderSpeed);

    bool playerHasVerticalSpeed = Mathf.Abs(MyRigidbody.velocity.y) > Mathf.Epsilon;
    MyAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
    MyAnimator.SetBool("IsClimbingIdle", !playerHasVerticalSpeed);
    MyAnimator.SetFloat("LadderSpeed", Mathf.Abs(LadderSpeed)); // 사다리 속도를 Animator에 전달
}

IEnumerator WalkWithDelay()
{
    yield return new WaitForSeconds(WalkDelay);
    // 일정 시간이 지난 후에 다시 Walk 함수 호출을 허용
    IsWalkingAllowed = true;
}

}


