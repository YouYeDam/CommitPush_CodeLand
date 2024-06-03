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

    // 이미 획득한 아이템을 추적하기 위한 Set
    HashSet<GameObject> acquiredItems = new HashSet<GameObject>();
    
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

    void AcquireItem(GameObject itemObject) {
        if (acquiredItems.Contains(itemObject)) {
            return; // 이미 획득한 아이템이면 반환
        }

        if (itemObject.tag == "Item" && InventoryScript != null) {
            Item item = itemObject.GetComponent<ItemPickup>().item;
            InventoryScript.AcquireItem(item); // 아이템과 갯수를 전달하여 호출
            QuestManager.UpdateObjective(item.ItemName, 1, true);
            acquiredItems.Add(itemObject);
            StartCoroutine(DestroyAfterDelay(itemObject));
        }

        if (itemObject.tag == "Money") {
            DropMoney = itemObject.GetComponent<DropMoney>();
            if (DropMoney.Bit != 0) {
                PlayerMoney.Bit += DropMoney.Bit;
            }
            if (DropMoney.Snippet != 0) {
                PlayerMoney.Snippet += DropMoney.Snippet;
            }
            acquiredItems.Add(itemObject);
            StartCoroutine(DestroyAfterDelay(itemObject));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject itemObject) {
        yield return new WaitForEndOfFrame(); // 한 프레임 기다린 후
        Destroy(itemObject);
        acquiredItems.Remove(itemObject); // Destroy한 후 Set에서 제거
    }
}
