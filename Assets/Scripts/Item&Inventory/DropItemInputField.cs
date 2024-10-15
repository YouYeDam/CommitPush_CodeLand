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
        if (InputFieldText.text.Length > 0) { // 드랍 개수를 입력해야만 확인 버튼 누를 수 있도록
            OKButton.interactable = true;
        }
        else {
            OKButton.interactable = false;
        }
    }
    public void OpenInputField() { // 입력창 팝업
        InputFieldBase.SetActive(true);
        InputFieldText.text = ItemDrag.Instance.DragSlot.ItemCount.ToString();
        TextInput.text = ItemDrag.Instance.DragSlot.ItemCount.ToString();
    }

    public void Cancel() { // 취소 버튼 클릭 시
        InputFieldBase.SetActive(false);
        ItemDrag.Instance.SetColor(0);
        ItemDrag.Instance.DragSlot.SetColor(1);
        ItemDrag.Instance.DragSlot = null;
    }

    public void OK() { // 확인 버튼 클릭 시
        ItemDrag.Instance.SetColor(0);
        int.TryParse(InputFieldText.text, out int ParsedCount); // 텍스트 -> 정수
        int DropCount = ParsedCount;

        if (DropCount > ItemDrag.Instance.DragSlot.ItemCount) { // 아이템 개수보다 많이 버리려 시도할 경우 아이템 개수만큼 버리도록 조정
            DropCount = ItemDrag.Instance.DragSlot.ItemCount;
        } 
        else if (DropCount <= 0 || ItemDrag.Instance.DragSlot.Item.Type == Item.ItemType.Quest) { // 드랍 개수가 0개 이하거나 퀘스트 아이템이면 드랍 불가능
            Cancel();
            return;
        }
        ItemDrag.Instance.DragSlot.SetColor(1);
        DropItem(DropCount);
    }

    void DropItem(int DropCount) { // 아이템 드랍 기능
        Item Item = ItemDrag.Instance.DragSlot.Item;
        ItemDrag.Instance.DragSlot.SetSlotCount(-DropCount); // 드랍 카운트만큼 아이템 개수 차감
        ItemDrag.Instance.DragSlot = null;
        InputFieldBase.SetActive(false);

        QuestManager.UpdateRemoveObjective(Item.ItemName, DropCount); // QuestManager에서 아이템 제거 업데이트
    }
}