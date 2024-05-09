using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
{
    [SerializeField] GameObject UICanvas;
    Inventory InventoryScript;
    PlayerMoney PlayerMoney;
    DropMoney DropMoney;
    void Start() {
        UICanvas = GameObject.Find("UIManager");
        if (UICanvas != null) {
            InventoryScript = UICanvas.GetComponent<Inventory>();
        }
        PlayerMoney = GetComponent<PlayerMoney>();

    }
    void OnTriggerEnter2D(Collider2D other) { // 아이템과 닿을 시 아이템 획득
        if (other.gameObject.tag == "Item" && InventoryScript != null) {
            InventoryScript.AcquireItem(other.gameObject.GetComponent<ItemPickup>().item); // 아이템과 갯수를 전달하여 호출
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Money") {
            DropMoney = other.gameObject.GetComponent<DropMoney>();
            if (DropMoney.Bit != 0) {
                PlayerMoney.Bit += DropMoney.Bit;
            }
            if (DropMoney.Snippet != 0) {
                PlayerMoney.Snippet += DropMoney.Snippet;
            }
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) { // 아이템과 충돌 시 아이템 획득
        if (collision.gameObject.tag == "Item" && InventoryScript != null) {
            InventoryScript.AcquireItem(collision.gameObject.GetComponent<ItemPickup>().item); // 아이템과 개수를 전달하여 호출
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Money") {
            DropMoney = collision.gameObject.GetComponent<DropMoney>();
            if (DropMoney.Bit != 0) {
                PlayerMoney.Bit += DropMoney.Bit;
            }
            if (DropMoney.Snippet != 0) {
                PlayerMoney.Snippet += DropMoney.Snippet;
            }
            Destroy(collision.gameObject);
        }
    }
}
