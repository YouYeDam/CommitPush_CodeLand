using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public float detectionRadius = 5.0f;  // 감지 반경
    public LayerMask npcLayerMask;       // NPC가 포함된 레이어 마스크
    PlayerManager PlayerManager;
    GameObject DialogueNPC;
    DialogueController DialogueController;

    void Start() {
        PlayerManager = GetComponent<PlayerManager>();
        DialogueController = FindObjectOfType<DialogueController>();
    }

    void Update() {
        // 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0)) {
            CheckMouseClick();
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
        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, npcLayerMask);
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

    void StartDialogueIfPossible(GameObject Npc) {
        Dialogue Dialogue = Npc.GetComponent<NPC>().Dialogue;
        if (Dialogue != null && DialogueController != null) {
            DialogueController.StartDialogue(Dialogue);
        }
    }
}
