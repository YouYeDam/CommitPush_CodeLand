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
        else if (DropCount <= 0) {
            Cancel();
            return;
        }
        ItemDrag.Instance.DragSlot.SetColor(1);
        DropItem(DropCount);
    }

    void DropItem(int DropCount)
    {
        ItemDrag.Instance.DragSlot.SetSlotCount(-DropCount);
        ItemDrag.Instance.DragSlot = null;
        InputFieldBase.SetActive(false);
    }
}