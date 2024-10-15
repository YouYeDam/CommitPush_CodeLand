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

    public void OnDrag(PointerEventData eventData) { // 마우스 드래그로 UI를 이동시킬 수 있도록
        MyRectTransform.anchoredPosition += eventData.delta/ UIManager.scaleFactor; // UI의 스케일에 맞춰서 UI 위치 조정
    }
}
