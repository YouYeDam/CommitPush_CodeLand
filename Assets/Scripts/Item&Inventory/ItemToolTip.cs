using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemToolTip : MonoBehaviour
{
    [SerializeField] GameObject ItemToolTipBase;
    [SerializeField] TMP_Text ItemNameText;
    [SerializeField] TMP_Text ItemInfoText;
    [SerializeField] Image ItemImage;

    public void ShowToolTip(Item Item) {
        ItemToolTipBase.SetActive(true);

        ItemNameText.text = Item.ItemName;
        ItemInfoText.text = Item.ItemInfo;
        ItemImage.sprite = Item.ItemImage;
    }

    public void HideToolTip() {
        ItemToolTipBase.SetActive(false);
    }

}
