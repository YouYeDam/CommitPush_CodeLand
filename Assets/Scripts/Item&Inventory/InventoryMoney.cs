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
    void SetMoneyText() {
        BitText.text = PlayerMoney.Bit.ToString() + "비트";
        SnippetText.text = PlayerMoney.Snippet.ToString() + "스니펫";
    }
}
