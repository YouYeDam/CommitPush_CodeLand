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
    BoxCollider2D[] MyBoxColliders;
    SpriteRenderer MySpriteRenderer;
    PlayerStatus PlayerStatus;
    PlayerManager PlayerManager;
    public bool IsWalkingAllowed = true;
    public LayerMask targetLayerMask;

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
        MyBoxColliders = GetComponents<BoxCollider2D>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerStatus = GetComponent<PlayerStatus>();
        PlayerManager = GetComponent<PlayerManager>();
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
        if (!PlayerManager.CanInput) {
            return;
        }
        MoveInput = Value.Get<Vector2>();
    }

    void Walk() { //플레이어 좌우이동
        if (IsAlive == false) {
            return;
        }

        bool PlayerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon;
        Vector2 PlayerVelocity = new Vector2 (MoveInput.x * RunSpeed, MyRigidbody.velocity.y); // 현재의 속도인 y가 무엇이든 동일한 속도를 유지하라는 의미
        MyRigidbody.velocity = PlayerVelocity;
        
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (IsOnLadder) {
            MyAnimator.SetBool("IsWalking", false); // 사다리에 있다면 무조건 걷기 애니메이션 X
        }
        else {
            // 레이캐스트를 통해 플레이어가 바라보고 있는 방향으로 조금 앞에 Ground 레이어 확인
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, 0.3f, LayerMask.GetMask("Ground"));
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right * transform.localScale.x, 0.3f, LayerMask.GetMask("Ground"));
            
            // 만약 양쪽 모두가 Ground 레이어와 충돌하지 않고 있다면 Walk 애니메이션 실행
            if (hitRight.collider == null && hitLeft.collider == null) {
                MyAnimator.SetBool("IsWalking", PlayerHasHorizontalSpeed);
            } else {
                MyAnimator.SetBool("IsWalking", false);
            }
        }
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

    // 사다리에서 점프를 가능하게 하기 위한 코루틴. 1. 현재 플레이어의 y 위치를 눈에 띄지 않을 만큼 올리고(사다리 rigidbody에 걸리는 현상 해결) 2. 원래대로 velocity에 벡터값 추가. 
    IEnumerator MagicJump(float modified_jump_speed)
    {
        transform.position = new Vector2(transform.position.x, transform.position.y+0.15f);
        yield return new WaitForSeconds(0.001f); 
        MyRigidbody.velocity += new Vector2 (0f, modified_jump_speed);
    }

    void OnJump(InputValue Value) { //플레이어 점프
        if (IsAlive == false || !PlayerManager.CanInput) {
            return;
        }
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (!IsOnGround & !IsSteppingLadder) {
            return;
        }
        if (Value.isPressed) {
            StartCoroutine(MagicJump(JumpSpeed));
        }

    }

    void ClimbLadder() { //플레이어 사다리 타기
        if (IsAlive == false) {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (!IsOnLadder) {
            MyAnimator.SetBool("IsClimbing", false);
            MyAnimator.SetBool("IsClimbingIdle", false);
            MyRigidbody.gravityScale = StartGravity;
            return;
        }
        
        // 플레이어가 사다리를 터치하고 있되, 밟고 있다면 사다리 애니메이션 끔. 허벅지 부위에 BoxCollider 추가.
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if(IsSteppingLadder){
            float Modified_y;
            if (MoveInput.y > 0){
                Modified_y = 0;
            } else {
                Modified_y = MoveInput.y;
            }
            float LadderSpeed = Modified_y * ClimbSpeed;
            MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, LadderSpeed);
            MyRigidbody.gravityScale = 0f;

            MyAnimator.SetBool("IsClimbing", false);
            MyAnimator.SetBool("IsClimbingIdle", false);

            
        }
        else {
            float LadderSpeed = MoveInput.y * ClimbSpeed;
            MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, LadderSpeed);
            MyRigidbody.gravityScale = 0f;

            bool playerHasVerticalSpeed = Mathf.Abs(MyRigidbody.velocity.y) > Mathf.Epsilon;
            MyAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
            MyAnimator.SetBool("IsClimbingIdle", !playerHasVerticalSpeed);
            MyAnimator.SetFloat("LadderSpeed", Mathf.Abs(LadderSpeed)); // 사다리 속도를 Animator에 전달
        }
    }



    public void TakeDamage(int Damage) { // 플레이어 피격
        if (IsAlive == false) {
            return;
        }
        if (IsInvincible) { // 무적상태면 실행안함
            return;
        }
        Damage -= Mathf.CeilToInt(PlayerStatus.PlayerDEF * 1.5f); // 방어력 공식: DEF * 1.5
        if (Damage < 1) {
            Damage = 1;
        }
        PlayerStatus.PlayerCurrentHP -= Damage;
        Color.a = 0.5f;
        MySpriteRenderer.color = Color;
        IsInvincible = true;
        StartCoroutine(InvincibleDelay()); // 무적 활성화 초기화
        StartCoroutine(MoveHurtPosition(0.2f)); // 플레이어 넉백
        if (PlayerStatus.PlayerCurrentHP <= 0) {
            PlayerStatus.PlayerCurrentHP = 0;
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

        // Ground레이어에 해당하는 레이어 마스크 가져오기
        LayerMask layerMask = LayerMask.GetMask("Ground");

        while (Time.time < StartTime + Duration) {
            float t = (Time.time - StartTime) / Duration;
            Vector3 NewPosition = Vector3.Lerp(StartPosition, EndPosition, t);
            
            // 레이캐스트를 사용해 Ground 레이어와의 충돌 검사
            RaycastHit2D Hit = Physics2D.Raycast(transform.position, EndPosition - transform.position, Vector3.Distance(transform.position, EndPosition), layerMask);
            
            if (Hit.collider != null) {
                yield break; // 충돌시 Coroutine 중단
            }
            
            transform.position = NewPosition;
            yield return null;
        }
    }
}


