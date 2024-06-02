using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public float DetectionRadius = 5f;  // 감지 반경
    public LayerMask npcLayerMask;       // NPC가 포함된 레이어 마스크
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
        // 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0)) {
            CheckMouseClick();
        }
        // 대화중인 NPC 감지
        if (!DialogueController.DialogueBase.activeSelf) { //대화창이 활성화 상태가 아니라면 대화중인 NPC 초기화
            ResetDialogueNPC();
        }
        else { //대화창이 활성화 상태라면 일정거리에서 떨어질 시 대화창 닫기
            if (DialogueNPC != null && Vector3.Distance(transform.position, DialogueNPC.transform.position) > DetectionRadius + 7f) {
            DialogueController.DialogueBase.SetActive(false);
            ResetDialogueNPC();
            }
        }
    }

    void CheckMouseClick() {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D Hit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, npcLayerMask);

        if (Hit.collider != null && Hit.collider.CompareTag("NPC")) {
            StartDialogueIfPossible(Hit.collider.gameObject);
        }
    }

    void OnInteraction() {
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

    GameObject DetectNearestNPC() {
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

    void StartDialogueIfPossible(GameObject npc)
    {
        if (npc.GetComponent<NPC>().IsShop)
        {
            PlayerUI.OpenShop();
            AddShopItem addShopItem = npc.GetComponent<AddShopItem>();
            addShopItem.SetContent();
            addShopItem.AddEachItem();
        }

        NPC npcComponent = npc.GetComponent<NPC>();
        if (npcComponent != null && DialogueController != null)
        {
            Dialogue dialogue = npcComponent.GetCurrentDialogue();
            if (dialogue != null)
            {
                DialogueController.StartDialogue(dialogue, npcComponent); // NPC와 함께 대화 시작
                if (PlayerUI.Shop.activeSelf)
                {
                    PlayerUI.CloseShop();
                }
                if (PlayerUI.DialogueButtonObject == null)
                {
                    PlayerUI.SetDialogueButton();
                }
            }
        }
    }

    void ResetDialogueNPC() {
        DialogueNPC = null;
    }
}
