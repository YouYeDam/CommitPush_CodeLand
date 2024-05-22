using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
public class PlayerMovement : MonoBehaviour
{
    Vector2 MoveInput;
    Rigidbody2D MyRigidbody;
    CapsuleCollider2D MyCapsuleCollider;
    BoxCollider2D[] MyBoxColliders;
    SpriteRenderer MySpriteRenderer;
    PlayerStatus PlayerStatus;
    PlayerManager PlayerManager;
    PlayerTakeDamageDisplay PlayerTakeDamageDisplay;
    DyingCheck DyingCheck;
    public Animator MyAnimator;
    public bool IsWalkingAllowed = true;
    public LayerMask targetLayerMask;

    public bool IsAlive = true;
    bool IsInvincible = false;
    float StartGravity;
    bool LadderSwitch = false;
    float TimeInState = 0f;
    float RequiredTime = 0.3f;
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
        PlayerTakeDamageDisplay = GetComponent<PlayerTakeDamageDisplay>();
        DyingCheck = FindObjectOfType<DyingCheck>();
        StartGravity = MyRigidbody.gravityScale;
        Color = MySpriteRenderer.color;
        StartCoroutine(AutoHealRoutine());
    }

    void Update()
    {
        UpPosition();
        if (IsAlive == false)
        {
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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            MyRigidbody.AddForce(new Vector2(0, JumpSpeed * 1.2f), ForceMode2D.Impulse);
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D Hit = Physics2D.Raycast(MyCapsuleCollider.bounds.center, Vector2.down, MyCapsuleCollider.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground") | LayerMask.GetMask("LadderToGround"));
        return Hit.collider != null;
    }

    void OnMove(InputValue Value)
    { //키보드로 좌우상하 입력받기
        if (!PlayerManager.CanInput)
        {
            return;
        }
        MoveInput = Value.Get<Vector2>();
    }

    void Walk()
    { //플레이어 좌우이동
        if (IsAlive == false)
        {
            return;
        }

        bool PlayerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon;
                // MoveInput이 OnMove에서 1이거나(위아래 키 안 눌렸을 때) 0.75로(위아래 키 눌렸을 떄) 설정됨. 두 값이 바뀌는 이유로 걷는 속도가 느려짐. -> 조건에 따라 무조건 1로 설정해야함.
        // float modifiedX = 0;
        // if (MoveInput.x > 0){
            // modifiedX = 1f;
        // }
        // else if (MoveInput.x < 0){
            // modifiedX = -1f;
        // }
        // Vector2 PlayerVelocity = new Vector2 (modifiedX * RunSpeed, MyRigidbody.velocity.y); // 현재의 속도인 y가 무엇이든 동일한 속도를 유지하라는 의미 + 이 부분이 작동을 안 함. 아마 on move 자체 함수의 문제인 듯 함.
        
        Vector2 PlayerVelocity = new Vector2(MoveInput.x * RunSpeed, MyRigidbody.velocity.y); // 현재의 속도인 y가 무엇이든 동일한 속도를 유지하라는 의미
        MyRigidbody.velocity = PlayerVelocity;
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (IsOnLadder)
        {
            MyAnimator.SetBool("IsWalking", false); // 사다리에 있다면 무조건 걷기 애니메이션 X
        }
        else
        {
            // 레이캐스트를 통해 플레이어가 바라보고 있는 방향으로 조금 앞에 Ground 레이어 확인
            RaycastHit2D HitRight = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, 0.3f, LayerMask.GetMask("Ground") | LayerMask.GetMask("LadderToGround"));
            RaycastHit2D HitLeft = Physics2D.Raycast(transform.position, -transform.right * transform.localScale.x, 0.3f, LayerMask.GetMask("Ground") | LayerMask.GetMask("LadderToGround"));

            // 만약 양쪽 모두가 Ground 레이어와 충돌하지 않고 있다면 Walk 애니메이션 실행
            if (HitRight.collider == null && HitLeft.collider == null)
            {
                MyAnimator.SetBool("IsWalking", PlayerHasHorizontalSpeed);
            }
            else
            {
                MyAnimator.SetBool("IsWalking", false);
            }
        }
    }

    void CheckWalk()
    { //걷기 애니메이션 체크
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground") | LayerMask.GetMask("LadderToGround"));
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder") | LayerMask.GetMask("LadderLadder"));
        if (!IsOnGround && !IsOnLadder)
        {
            MyAnimator.SetBool("IsWalking", false);
            MyAnimator.SetBool("IsJumping", true);
        }
        else
        {
            MyAnimator.SetBool("IsJumping", false);
        }
    }
    void FlipSprite()
    { //플레이어 좌우반전
        bool PlayerHasHorizontalSpeed = Mathf.Abs(MyRigidbody.velocity.x) > Mathf.Epsilon; // abs: 절대값 반환, Epsilon: 0에 가까운 아주 작은 값 반환

        if (PlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(MyRigidbody.velocity.x), 1f); // 0보다 크면1, 작으면 -1 반환
        }
    }

    // 
    // void OnJump(InputValue Value)
    // { 
    //     if (IsAlive == false || !PlayerManager.CanInput)
    //     {
    //         return;
    //     }
    //     bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    //     bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
    //     if (!IsOnGround & !IsSteppingLadder)
    //     {
    //         return;
    //     }
    //     if (Value.isPressed)
    //     {
    //         StartCoroutine(MagicJump(JumpSpeed));
    //     }

    // }


    void ClimbLadder()
    { //플레이어 사다리 타기

        if (IsAlive == false)
        {
            return;
        }
        bool IsOnLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnLadderToGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("LadderToGround"));
        bool IsSteppingLadder = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && !MyBoxColliders[1].IsTouchingLayers(LayerMask.GetMask("Ladder"));
        bool IsOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        // 사다리에 없는 경우
        if (!IsOnLadder & !IsOnLadderToGround)
        {
            // 애니메이션 설정
            MyAnimator.SetBool("IsClimbing", false);
            MyAnimator.SetBool("IsClimbingIdle", false);

            // 중력 설정
            MyRigidbody.gravityScale = StartGravity;

            // 사다리 활성화
            if (IsOnGround)
            {
                EnableLadderLayerColliders();
                LadderSwitch = false;
            }
            return;
        }
        // 플레이어가 사다리를 터치하고 있되, 밟고 있다면 사다리 애니메이션 끔. 허벅지 부위에 BoxCollider 추가.
        if (IsSteppingLadder | IsOnLadderToGround)
        {
            if (!LadderSwitch)
            {
                DisableLadderLayerColliders();
                LadderSwitch = true;
            }
            if (MoveInput.y >= 0)
            {
                return;
            }
            else if (MoveInput.y < 0)
            {
                LadderSwitch = false;
                float LadderSpeed = MoveInput.y * ClimbSpeed;
                MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, LadderSpeed);
                MyRigidbody.gravityScale = 0f;

                MyAnimator.SetBool("IsClimbing", false);
                MyAnimator.SetBool("IsClimbingIdle", false);
                EnableLadderLayerColliders();
            }

            else
            {
                DisableLadderLayerColliders();
            }


        }
        else
        {
            float LadderSpeed = MoveInput.y * ClimbSpeed;
            MyRigidbody.velocity = new Vector2(MyRigidbody.velocity.x, LadderSpeed);
            MyRigidbody.gravityScale = 0f;

            bool playerHasVerticalSpeed = Mathf.Abs(MyRigidbody.velocity.y) > Mathf.Epsilon;
            MyAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
            MyAnimator.SetBool("IsClimbingIdle", !playerHasVerticalSpeed);
            MyAnimator.SetFloat("LadderSpeed", Mathf.Abs(LadderSpeed)); // 사다리 속도를 Animator에 전달
        }
    }

    void DisableLadderLayerColliders()
    {
        // "Ladder" 태그가 지정된 모든 GameObjects 찾기
        GameObject[] LadderObjects = GameObject.FindGameObjectsWithTag("Ladder");
        foreach (GameObject LadderObject in LadderObjects)
        {
            LadderObject.layer = LayerMask.NameToLayer("LadderToGround");
            TilemapCollider2D TilemapCollider = LadderObject.GetComponent<TilemapCollider2D>();
            if (TilemapCollider != null)
            {
                TilemapCollider.isTrigger = false; // isTrigger 옵션 끄기
            }
        }

    }
    void EnableLadderLayerColliders()
    {
        // "Ladder" 태그가 지정된 모든 GameObjects 찾기
        GameObject[] LadderObjects = GameObject.FindGameObjectsWithTag("Ladder");
        foreach (GameObject LadderObject in LadderObjects)
        {
            LadderObject.layer = LayerMask.NameToLayer("Ladder");
            TilemapCollider2D TilemapCollider = LadderObject.GetComponent<TilemapCollider2D>();
            if (TilemapCollider != null)
            {
                TilemapCollider.isTrigger = true; // isTrigger 옵션 끄기
            }
        }

    }

    public void TakeDamage(int Damage)
    { // 플레이어 피격
        if (IsAlive == false)
        {
            return;
        }
        if (IsInvincible)
        { // 무적상태면 실행안함
            return;
        }
        Damage -= Mathf.CeilToInt(PlayerStatus.PlayerDEF * 1.5f); // 방어력 공식: DEF * 1.5
        Damage = Mathf.FloorToInt(Damage * Random.Range(0.8f, 1.21f)); // 데미지 랜덤값: 계산된 데미지의 0.8 ~ 1.2배로 조정
        if (Damage < 1)
        {
            Damage = 1;
        }
        PlayerTakeDamageDisplay.DisplayDamageBar(Damage);
        PlayerStatus.PlayerCurrentHP -= Damage;
        Color.a = 0.5f;
        MySpriteRenderer.color = Color;
        IsInvincible = true;
        StartCoroutine(InvincibleDelay()); // 무적 활성화 초기화
        if (PlayerStatus.PlayerCurrentHP <= 0)
        {
            PlayerStatus.PlayerCurrentHP = 0;
            Die();
        }
    }

    void Die()
    { // 플레이어 죽음
        IsAlive = false;
        MyAnimator.SetBool("IsDying", true);
        DyingCheck.ActivateDyingCheck();
    }

    IEnumerator WalkWithDelay()
    { // 공격 시 이동 제한
        yield return new WaitForSeconds(WalkDelayTime);
        // 일정 시간이 지난 후에 다시 Walk 함수 호출을 허용
        IsWalkingAllowed = true;
    }

    IEnumerator InvincibleDelay()
    { // 피격 시 잠깐동안 무적
        yield return new WaitForSeconds(InvincibleTime);
        IsInvincible = false;
        Color.a = 1f;
        MySpriteRenderer.color = Color;
    }
    IEnumerator AutoHealRoutine()
    {
        while (true)
        {
            if (IsAlive)
            {
                PlayerStatus.AutoHeal(); // 5초마다 AutoHeal 함수 호출
            }
            yield return new WaitForSeconds(5f);
        }
    }

    void UpPosition()
    {
        bool IsCapsuleOnGround = MyCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool IsBoxOnGround = MyBoxColliders[0].IsTouchingLayers(LayerMask.GetMask("Ground"));


        if (IsCapsuleOnGround && IsBoxOnGround)
        {
            TimeInState += Time.deltaTime;
            if (TimeInState >= RequiredTime)
            {
                RaycastHit2D HitRight = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, 0.3f, LayerMask.GetMask("Ground") | LayerMask.GetMask("LadderToGround"));
                RaycastHit2D HitLeft = Physics2D.Raycast(transform.position, -transform.right * transform.localScale.x, 0.3f, LayerMask.GetMask("Ground") | LayerMask.GetMask("LadderToGround"));
                if (HitRight.collider == null && HitLeft.collider == null) {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                    TimeInState = 0f; 
                }

            }
        }
        else
        {
            TimeInState = 0f; 
        }
    }
}