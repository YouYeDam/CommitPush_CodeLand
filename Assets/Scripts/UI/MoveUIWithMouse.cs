using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveUIElementWithMouse : MonoBehaviour, IDragHandler
{
    private RectTransform MyRectTransform;
    [SerializeField] Canvas UIManager;
    
    void Start()
    {
        MyRectTransform = GetComponent<RectTransform>();
    }

    // 드래그 이벤트가 발생할 때 호출
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 입력에 따라 UI 요소의 위치를 업데이트
        MyRectTransform.anchoredPosition += eventData.delta/ UIManager.scaleFactor;
    }
}
