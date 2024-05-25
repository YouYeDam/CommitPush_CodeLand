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

    public void OpenInputField(ShopSlot BuyShopSlot)
    {
        InputFieldBase.SetActive(true);
        ShopSlot = BuyShopSlot;

        if (SellItemInputField.InputFieldBase.activeSelf) {
            SellItemInputField.Cancel();
        }
    }

    public void Cancel()
    {
        InputFieldBase.SetActive(false);
    }

    public void OK()
    {
        int.TryParse(InputFieldText.text, out int ParsedCount);
        int BuyItemCount = ParsedCount;

        if (BuyItemCount <= 0) {
            Cancel();
            return;
        }
        ShopSlot.BuyManyItem(BuyItemCount);
        InputFieldBase.SetActive(false);
    }
}
