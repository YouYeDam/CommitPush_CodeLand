using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemInputField : MonoBehaviour
{
    [SerializeField] TMP_Text TextInput;  
    [SerializeField] TMP_InputField InputFieldText;
    [SerializeField] public GameObject InputFieldBase;
    BuyItemInputField BuyItemInputField;
    Slot SellItemSlot;

    public Button OKButton;

    void Start() {
        BuyItemInputField = FindObjectOfType<BuyItemInputField>();
    }
    void Update() {
        if (InputFieldText.text.Length > 0) {
            OKButton.interactable = true;
        }
        else {
            OKButton.interactable = false;
        }
    }
    public void OpenInputField(Slot SellSlot) { // 판매할 아이템 숫자 입력창 팝업
        InputFieldBase.SetActive(true);
        SellItemSlot = SellSlot;
        InputFieldText.text = SellItemSlot.ItemCount.ToString();
        TextInput.text = SellItemSlot.ItemCount.ToString();

        if (BuyItemInputField != null && BuyItemInputField.InputFieldBase.activeSelf) {
            BuyItemInputField.Cancel();
        }
    }

    public void Cancel() { // 거래 취소
        InputFieldBase.SetActive(false);
    }

    public void OK() { // 거래 확인
        int.TryParse(InputFieldText.text, out int ParsedCount); // 텍스트를 int로 변환
        int SellItemCount = ParsedCount;

        if (SellItemCount > SellItemSlot.ItemCount) { // 가지고 있는 아이템보다 많은 수량을 등록할 경우 가지고 있는 아이템 최대 수량만큼만 판매
            SellItemCount = SellItemSlot.ItemCount;
        } 

        if (SellItemCount <= 0) { // 판매할 아이템이 0개 이하면 거래 취소처리
            Cancel();
            return;
        }
        SellItemSlot.SellManyItem(SellItemCount);
        InputFieldBase.SetActive(false);
    }
}
