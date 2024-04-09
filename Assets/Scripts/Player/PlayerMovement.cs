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
    SpriteRenderer MySpriteRenderer;
    PlayerStatus PlayerStatus;
    public bool IsWalkingAllowed = true;
    public bool IsAlive = true;
    bool IsInvincible = false;
    float StartGravity;
    Color Color;
    [SerializeField] float WalkDelayTime = 0.2f; // Walk 함수 호출을 지연할 시간
    [SerializeField] public float InvincibleTime = 1f;
    [SerializeField] float RunSpeed = 10f;
    [SerializeField] float JumpSpeed = 5f;
    [SerializeField] float ClimbSpeed = 5f;
    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
        MyCapsuleCollider = GetComponent<CapsuleCollider2D>();
        MyBoxCollider = GetComponent<BoxCollider2D>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerStatus = GetComponent<PlayerStatus>();
        StartGravity = MyRigidbody.gravityScale;

        Color = MySpriteRenderer.color;
    }

    void Update()
    {
        if (IsAlive == false) {
            return;
        }
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
    void OnMove(InputValue Value) { //키보드로 좌우상하 입력받기
        MoveInput = Value.Get<Vector2>();
    }

    void Walk() { //플레이어 좌우이동
        if (IsAlive == false) {
            return;
        }

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
        if (IsAlive == false) {
            return;
        }
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (!IsOnGround) {
            return;
        }
        if (Value.isPressed) {
            MyRigidbody.velocity += new Vector2 (0f, JumpSpeed);
        }
    }

    void ClimbLadder() { //플레이어 사다리 타기
        if (IsAlive == false) {
            return;
        }
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

    public void Hurt(float Damage) { // 플레이어 피격
        if (IsAlive == false) {
            return;
        }
        if (IsInvincible) { // 무적상태면 실행안함
            return;
        }
        PlayerStatus.PlayerCurrentHealth -= Damage;
        Color.a = 0.5f;
        MySpriteRenderer.color = Color;
        IsInvincible = true;
        StartCoroutine(InvincibleDelay()); // 무적 활성화 초기화
        StartCoroutine(MoveHurtPosition(0.2f)); // 플레이어 넉백
        if (PlayerStatus.PlayerCurrentHealth <= 0) {
            Die();
        }
    }

    void Die() { // 플레이어 죽음
        IsAlive = false;
        MyAnimator.SetBool("IsDying", true);
    }

    IEnumerator WalkWithDelay() { // 공격 시 이동 제한
        yield return new WaitForSeconds(WalkDelayTime);
        // 일정 시간이 지난 후에 다시 Walk 함수 호출을 허용
        IsWalkingAllowed = true;
    }

    IEnumerator InvincibleDelay() { // 피격 시 잠깐동안 무적
        yield return new WaitForSeconds(InvincibleTime);
        IsInvincible = false;
        Color.a = 1f;
        MySpriteRenderer.color = Color;
    }

    IEnumerator MoveHurtPosition(float Duration) {
    float StartTime = Time.time;
    Vector3 StartPosition = transform.position;
    Vector3 EndPosition = StartPosition + ((transform.localScale.x > 0 ? Vector3.left : Vector3.right) * 0.7f);

    while (Time.time < StartTime + Duration) {
        float t = (Time.time - StartTime) / Duration;
        transform.position = Vector3.Lerp(StartPosition, EndPosition, t);
        yield return null;
    }
}

}


