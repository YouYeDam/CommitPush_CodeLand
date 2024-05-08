using UnityEngine;
public class StartDialogue : MonoBehaviour
{
    public float detectionRadius = 5.0f;  // 감지 반경
    public LayerMask npcLayerMask;       // NPC가 포함된 레이어 마스크
    PlayerManager PlayerManager;
    GameObject DialogueNPC;
    DialogueController DialogueController;

    void Start() {
        PlayerManager= GetComponent<PlayerManager>();
        DialogueController = FindObjectOfType<DialogueController>();
    }
    void OnInteraction() {
        if (!PlayerManager.CanInput || DialogueController.DialogueBase.activeSelf) {
            return;
        }
        DialogueNPC = DetectNearestNPC();
        if (DialogueNPC != null) {
        Dialogue Dialogue;
        Dialogue = DialogueNPC.GetComponent<NPC>().Dialogue;
            if (Dialogue != null) {
                DialogueController.StartDialogue(Dialogue);
            }
        }
    }

    GameObject DetectNearestNPC()
    {
        // 플레이어 위치 주변에 있는 모든 콜라이더 검출
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, npcLayerMask);
        GameObject nearestNPC = null;
        float minDistance = float.MaxValue;  // 최소 거리를 저장하기 위한 변수
        foreach (var hit in hits)
        {
            if (hit.CompareTag("NPC"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestNPC = hit.gameObject;
                }
            }
        }
        return nearestNPC;  // 가장 가까운 NPC 반환
    }
}
