using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemToolTip : MonoBehaviour
{
    PlayerStatus PlayerStatus;
    [SerializeField] GameObject ItemToolTipBase;
    [SerializeField] TMP_Text ItemNameText;
    [SerializeField] TMP_Text ItemInfoText;
    [SerializeField] TMP_Text ItemCostText;
    [SerializeField] TMP_Text ItemRequireLVText;
    [SerializeField] TMP_Text ItemGradeText;
    [SerializeField] TMP_Text ItemDetailTypeText;
    [SerializeField] Image ItemImage;

    void Start() {
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }
    public void ShowToolTip(Item Item) { // 아이템 툴팁 전시 기능
        ItemToolTipBase.SetActive(true);

        ItemNameText.text = Item.ItemName;
        ItemInfoText.text = Item.ItemInfo;
        ItemCostText.text = Item.ItemCost.ToString();
        ItemDetailTypeText.text = "아이템 유형: " + Item.ItemDetailType;
        ItemImage.sprite = Item.ItemImage;

        if (Item.Type == Item.ItemType.Equipment) { // 장비 아이템일 경우에만 장비 아이템 정보 보이기
            DisplayRequireLevel(Item);
            DisplayEquipmentGrade(Item);
        }
        else {
            HideEquipmentInfoText();
            ItemNameText.color = Color.white;
        }
    }

    public void HideToolTip() { // 아이템 툴팁 숨김 기능
        ItemToolTipBase.SetActive(false);
    }

    void DisplayRequireLevel(Item Item) { // 필요 아이템 레벨 전시 기능
        ItemRequireLVText.gameObject.SetActive(true);
        int RequireLevel = Item.ItemPrefab.GetComponent<EquipmentItem>().RequireLevel;
        ItemRequireLVText.text = "필요 코딩력: " + RequireLevel + " LV";

        Color color;
        if (RequireLevel > PlayerStatus.PlayerLevel) { // 레벨 미달이면 필요 착용 레벨을 붉은색으로 표시
            ColorUtility.TryParseHtmlString("#FF0000", out color); // Red
            ItemRequireLVText.color = color;
        } else {
            ColorUtility.TryParseHtmlString("#FFFFFF", out color); // White
            ItemRequireLVText.color = color;
        }
    }
    void DisplayEquipmentGrade(Item Item) { // 아이템 등급 전시 기능
        ItemGradeText.gameObject.SetActive(true);
        string EquipmentItemGrade = Item.ItemPrefab.GetComponent<EquipmentItem>().EquipmentItemGrade;
        ItemGradeText.text = "아이템 등급: " + EquipmentItemGrade;

        Color color;
        switch (EquipmentItemGrade) { // 등급에 따라 색상 부여
            case "일반":
                ColorUtility.TryParseHtmlString("#FFFFFF", out color); // White
                ItemGradeText.color = color;
                ItemNameText.color = color;
                break;
            case "고급":
                ColorUtility.TryParseHtmlString("#32CD32", out color); // Limegreen
                ItemGradeText.color = color;
                ItemNameText.color = color;
                break;
            case "희귀":
                ColorUtility.TryParseHtmlString("#87CEFA", out color); // Lightskyblue
                ItemGradeText.color = color;
                ItemNameText.color = color;
                break;
            case "영웅":
                ColorUtility.TryParseHtmlString("#BA55D3", out color); // Mediumorchid
                ItemGradeText.color = color;
                ItemNameText.color = color;
                break;
            case "전설":
                ColorUtility.TryParseHtmlString("#FF8C00", out color); // Darkorange
                ItemGradeText.color = color;
                ItemNameText.color = color;
                break;
            default:
                break;
        }
    }

    void HideEquipmentInfoText() { // 장비아이템 전용 텍스트 숨김 
        ItemRequireLVText.gameObject.SetActive(false);
        ItemGradeText.gameObject.SetActive(false);
    }
}
