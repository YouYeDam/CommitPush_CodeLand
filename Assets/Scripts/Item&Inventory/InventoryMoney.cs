using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InventoryMoney : MonoBehaviour
{
    [SerializeField] TMP_Text BitText;
    [SerializeField] TMP_Text SnippetText;
    PlayerMoney PlayerMoney;

    void Start() {
        PlayerMoney = FindObjectOfType<PlayerMoney>();
    }

    void Update() {
        SetMoneyText();
    }
    void SetMoneyText() { // 인벤토리에 화폐 표시
        BitText.text = PlayerMoney.Bit.ToString() + "비트";
        SnippetText.text = PlayerMoney.Snippet.ToString() + "스니펫";
    }
}
