using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
{
    [SerializeField] GameObject UICanvas;
    Inventory InventoryScript;
    void Start() {
        UICanvas = GameObject.Find("UIManager");
        if (UICanvas != null) {
            InventoryScript = UICanvas.GetComponent<Inventory>();
        }
    }
    void OnTriggerEnter2D(Collider2D other) { // 아이템과 닿을 시 아이템 획득
        if (other.gameObject.tag == "Item" && InventoryScript != null) {
            InventoryScript.AcquireItem(other.gameObject.GetComponent<ItemPickup>().item); // 아이템과 갯수를 전달하여 호출
            Destroy(other.gameObject);
        }    
    }
}
