using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
{
    [SerializeField] GameObject UICanvas;
    public Inventory InventoryScript;
    PlayerMoney PlayerMoney;
    DropMoney DropMoney;
    QuestManager QuestManager;

    HashSet<GameObject> AcquiredItems = new HashSet<GameObject>(); // 이미 획득한 아이템을 추적하기 위한 Set (중복 습득 버그 방지)
    
    void Start() {
        UICanvas = GameObject.Find("UIManager");
        if (UICanvas != null) {
            InventoryScript = UICanvas.GetComponent<Inventory>();
        }
        PlayerMoney = GetComponent<PlayerMoney>();
        QuestManager = FindObjectOfType<QuestManager>();
    }

    void OnTriggerEnter2D(Collider2D other) { // 아이템과 닿을 시 아이템 획득
        AcquireItem(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) { // 아이템과 충돌 시 아이템 획득
        AcquireItem(collision.gameObject);
    }

    void AcquireItem(GameObject ItemObject) { // 아이템 획득 기능 함수
        if (AcquiredItems.Contains(ItemObject)) {
            return; // 이미 획득한 아이템이면 반환 (중복 습득 버그 방지)
        }

        if (ItemObject.tag == "Item" && InventoryScript != null) {
            Item item = ItemObject.GetComponent<ItemPickup>().item;
            InventoryScript.InventoryAcquireItem(item); // 아이템과 갯수를 전달하여 호출

            QuestManager.UpdateObjective(item.ItemName, 1, true);
            AcquiredItems.Add(ItemObject); // 해시 셋에 아이템 삽입
            StartCoroutine(DestroyAfterDelay(ItemObject));
        }

        if (ItemObject.tag == "Money") {
            DropMoney = ItemObject.GetComponent<DropMoney>();
            if (DropMoney.Bit != 0) {
                PlayerMoney.Bit += DropMoney.Bit;
            }
            if (DropMoney.Snippet != 0) {
                PlayerMoney.Snippet += DropMoney.Snippet;
            }
            AcquiredItems.Add(ItemObject); // 해시 셋에 아이템 삽입
            StartCoroutine(DestroyAfterDelay(ItemObject));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject itemObject) { // 대기시간 후 아이템 삭제 (버그 방지)
        yield return new WaitForEndOfFrame(); // 한 프레임 기다린 후
        Destroy(itemObject);
        AcquiredItems.Remove(itemObject); // 삭제 후 해시셋에서 제거
    }
}
