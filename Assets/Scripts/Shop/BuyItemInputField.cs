using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyItemInputField : MonoBehaviour
{
    [SerializeField] TMP_Text TextInput;  
    [SerializeField] TMP_InputField InputFieldText;
    [SerializeField] public GameObject InputFieldBase;
    public SellItemInputField SellItemInputField;

    ShopSlot ShopSlot;
    public Button OKButton;

    void Start() {
        SellItemInputField = FindObjectOfType<SellItemInputField>();
    }
    void Update() {
        if (InputFieldText.text.Length > 0) {
            OKButton.interactable = true;
        }
        else {
            OKButton.interactable = false;
        }
    }

    public void OpenInputField(ShopSlot BuyShopSlot) { // 구매할 아이템 숫자 입력창 팝업
        InputFieldBase.SetActive(true);
        ShopSlot = BuyShopSlot;

        if (SellItemInputField.InputFieldBase.activeSelf) {
            SellItemInputField.Cancel();
        }
    }

    public void Cancel() { // 거래 취소
        InputFieldBase.SetActive(false);
    }

    public void OK() { // 거래 확인
        int.TryParse(InputFieldText.text, out int ParsedCount); // 텍스트를 int로 변환
        int BuyItemCount = ParsedCount;

        if (BuyItemCount <= 0) { // 구매할 아이템이 0개 이하면 거래 취소처리
            Cancel();
            return;
        }
        ShopSlot.BuyManyItem(BuyItemCount);
        InputFieldBase.SetActive(false);
    }
}
