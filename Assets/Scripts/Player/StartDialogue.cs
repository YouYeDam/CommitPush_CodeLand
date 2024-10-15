using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public float DetectionRadius = 5f;  // 감지 반경
    public LayerMask npcLayerMask; // NPC가 포함된 레이어 마스크
    PlayerManager PlayerManager;
    PlayerUI PlayerUI;
    GameObject DialogueNPC;
    DialogueController DialogueController;

    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        PlayerUI = GetComponent<PlayerUI>();
        DialogueController = FindObjectOfType<DialogueController>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) { // 마우스 클릭 감지
            CheckMouseClick();
        }
        if (!DialogueController.DialogueBase.activeSelf) { // 대화중인 NPC 감지
            ResetDialogueNPC();  // 대화창이 활성화 상태가 아니라면 대화중인 NPC 초기화
        }
        else { //대화창이 활성화 상태라면 일정거리에서 떨어질 시 대화창 닫기
            if (DialogueNPC != null && Vector3.Distance(transform.position, DialogueNPC.transform.position) > DetectionRadius + 15f) {
            DialogueController.DialogueBase.SetActive(false);
            ResetDialogueNPC();
            }
        }
    }

    void CheckMouseClick() { // 마우스 클릭 시 NPC 대화 가능한지 체크
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D Hit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, npcLayerMask);

        if (Hit.collider != null && Hit.collider.CompareTag("NPC")) {
            DialogueNPC = Hit.collider.gameObject;
            StartDialogueIfPossible(DialogueNPC);
        }
    }

    void OnInteraction() { // NPC 대화 단축키 행동(NPC가 반경 내에 있을 경우 Space Bar 키)
        if (!PlayerManager.CanInput) {
            return;
        }
        if (!DialogueController.DialogueBase.activeSelf) {
            DialogueNPC = DetectNearestNPC();
            if (DialogueNPC != null) {
                StartDialogueIfPossible(DialogueNPC);
            }
        }
        else {
            DialogueController.OnNextButtonClicked();
        }
    }

    GameObject DetectNearestNPC() { // 가장 가까운 NPC 탐지(여러 NPC 대화를 한꺼번에 시도하는 것을 방지)
        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, DetectionRadius, npcLayerMask);
        GameObject NearestNPC = null;
        float MinDistance = float.MaxValue;
        foreach (var Hit in Hits) {
            if (Hit.CompareTag("NPC")) {
                float Distance = Vector3.Distance(transform.position, Hit.transform.position);
                if (Distance < MinDistance) {
                    MinDistance = Distance;
                    NearestNPC = Hit.gameObject;
                }
            }
        }
        return NearestNPC;
    }

    void StartDialogueIfPossible(GameObject npc) { // 대화가 가능하다면 대화 시작
        if (npc.GetComponent<NPC>().IsShop) { // 상점 NPC라면 상점 열기
            PlayerUI.OpenShop();
            AddShopItem addShopItem = npc.GetComponent<AddShopItem>();
            addShopItem.SetContent();
            addShopItem.AddEachItem();
        }

        NPC NpcComponent = npc.GetComponent<NPC>();
        if (NpcComponent != null && DialogueController != null) {
            Dialogue Dialogue = NpcComponent.GetCurrentDialogue();

            if (Dialogue != null) {
                DialogueController.StartDialogue(Dialogue, NpcComponent); // NPC와 함께 대화 시작
                if (PlayerUI.Shop.activeSelf) { // 상점 창 열려있다면 닫기
                    PlayerUI.CloseShop();
                }

                if (PlayerUI.DialogueButtonObject == null) {
                    PlayerUI.SetDialogueButton();
                }
            }
        }
    }

    void ResetDialogueNPC() { // 대화중인 NPC 초기화
        DialogueNPC = null;
    }
}
