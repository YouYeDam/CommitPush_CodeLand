using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image SkillImage;  // 스킬의 이미지
    public GameObject SkillPrefab;
    public TMP_Text SkillNameText;
    public string SkillName;
    SkillToolTip SkillToolTip;
    public SkillQuickSlot QuickSlotReference;

    void Start() {
        GameObject ToolTipObject = GameObject.Find("SkillToolTip");
        if (ToolTipObject != null) {
        SkillToolTip = ToolTipObject.GetComponent<SkillToolTip>();
        }
    }
    public void SetColor(float Alpha){ // 스킬 이미지의 투명도 조절
        Color Color = SkillImage.color;
        Color.a = Alpha;
        SkillImage.color = Color;
    }

    public void AddItem(GameObject SkillPrefab) { // 스킬창에 새로운 스킬 추가
        this.SkillPrefab = SkillPrefab;
        SpriteRenderer SkillSpriteRenderer = this.SkillPrefab.GetComponent<SpriteRenderer>();
        SkillName = SkillPrefab.GetComponent<SkillInfo>().SkillName;
        SkillNameText.text = SkillName;
        SkillImage.sprite = SkillSpriteRenderer.sprite;
        SetColor(1);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (SkillPrefab != null) {
            SkillToolTip.ShowToolTip(SkillPrefab);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        SkillToolTip.HideToolTip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(SkillPrefab != null)
        {
            SkillDrag.Instance.SkillDragSlot = this;
            SkillDrag.Instance.DragSetImage(SkillImage);
            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            SkillDrag.Instance.transform.position = this.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SkillPrefab != null) 
        {
            Vector3 GlobalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(SkillDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out GlobalMousePos))
            {
                SkillDrag.Instance.MyRectTransform.position = GlobalMousePos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SkillDrag.Instance.SetColor(0);
        SkillDrag.Instance.SkillDragSlot = null;
    }
}
