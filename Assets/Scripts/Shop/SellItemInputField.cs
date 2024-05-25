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
    public void OpenInputField(Slot SellSlot)
    {
        InputFieldBase.SetActive(true);
        SellItemSlot = SellSlot;
        InputFieldText.text = SellItemSlot.ItemCount.ToString();
        TextInput.text = SellItemSlot.ItemCount.ToString();

        if (BuyItemInputField.InputFieldBase.activeSelf) {
            BuyItemInputField.Cancel();
        }
    }

    public void Cancel()
    {
        InputFieldBase.SetActive(false);
    }

    public void OK()
    {
        int.TryParse(InputFieldText.text, out int ParsedCount);
        int SellItemCount = ParsedCount;

        if (SellItemCount > SellItemSlot.ItemCount) {
            SellItemCount = SellItemSlot.ItemCount;
        } 

        if (SellItemCount <= 0) {
            Cancel();
            return;
        }
        SellItemSlot.SellManyItem(SellItemCount);
        InputFieldBase.SetActive(false);
    }
}
