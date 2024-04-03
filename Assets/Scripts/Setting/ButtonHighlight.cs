using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; // TextMeshPro를 사용하는 경우 추가

public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // 버튼의 텍스트 컴포넌트
    public Color defaultColor = Color.white; // 기본 색상
    public Color highlightColor = Color.yellow; // 하이라이트 색상

    // 마우스가 버튼 위에 올라갔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightColor;
    }

    // 마우스가 버튼에서 내려갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = defaultColor;
    }
}
