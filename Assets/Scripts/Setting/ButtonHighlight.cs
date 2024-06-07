using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI ButtonText; // 버튼의 텍스트 컴포넌트
    public Color DefaultColor = Color.white; // 기본 색상
    public Color HighlightColor = Color.yellow; // 하이라이트 색상

    // 마우스가 버튼 위에 올라갔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonText.color = HighlightColor;
    }

    // 마우스가 버튼에서 내려갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonText.color = DefaultColor;
    }
}
