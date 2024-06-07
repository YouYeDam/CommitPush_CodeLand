using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropItemInputNumber : MonoBehaviour
{

    [SerializeField] TMP_Text TextInput;  
    [SerializeField] TMP_InputField InputFieldText;
    [SerializeField] GameObject InputFieldBase;
    public Button OKButton;
    QuestManager QuestManager;
    
    void Start() {
        QuestManager = FindObjectOfType<QuestManager>();
    }
    void Update() {
        if (InputFieldText.text.Length > 0) {
            OKButton.interactable = true;
        }
        else {
            OKButton.interactable = false;
        }
    }
    public void OpenInputField()
    {
        InputFieldBase.SetActive(true);
        InputFieldText.text = ItemDrag.Instance.DragSlot.ItemCount.ToString();
        TextInput.text = ItemDrag.Instance.DragSlot.ItemCount.ToString();
    }

    public void Cancel()
    {
        InputFieldBase.SetActive(false);
        ItemDrag.Instance.SetColor(0);
        ItemDrag.Instance.DragSlot.SetColor(1);
        ItemDrag.Instance.DragSlot = null;
    }

    public void OK()
    {
        ItemDrag.Instance.SetColor(0);
        int.TryParse(InputFieldText.text, out int ParsedCount);
        int DropCount = ParsedCount;

        if (DropCount > ItemDrag.Instance.DragSlot.ItemCount) {
            DropCount = ItemDrag.Instance.DragSlot.ItemCount;
        } 
        else if (DropCount <= 0 || ItemDrag.Instance.DragSlot.Item.Type == Item.ItemType.Quest) { // 드랍 개수가 0개 이하거나 퀘스트 아이템이면 드랍 불가능
            Cancel();
            return;
        }
        ItemDrag.Instance.DragSlot.SetColor(1);
        DropItem(DropCount);
    }

    void DropItem(int DropCount)
    {
        Item Item = ItemDrag.Instance.DragSlot.Item;
        ItemDrag.Instance.DragSlot.SetSlotCount(-DropCount);
        ItemDrag.Instance.DragSlot = null;
        InputFieldBase.SetActive(false);

        // QuestManager에서 아이템 제거 목표 업데이트
        QuestManager.UpdateRemoveObjective(Item.ItemName, DropCount);
    }
}