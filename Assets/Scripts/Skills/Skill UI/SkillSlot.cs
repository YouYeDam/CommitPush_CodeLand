using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image SkillImage;
    public GameObject SkillPrefab;
    public TMP_Text SkillNameText;
    public string SkillName;
    SkillToolTip SkillToolTip;
    public SkillQuickSlot QuickSlotReference;
    public bool CantMove = false;
    public bool CanDelete = true;

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

    public void AddSkill(GameObject SkillPrefab) { // 스킬창에 새로운 스킬 추가
        this.SkillPrefab = SkillPrefab;
        SpriteRenderer SkillSpriteRenderer = this.SkillPrefab.GetComponent<SpriteRenderer>();
        SkillName = SkillPrefab.GetComponent<SkillInfo>().SkillName;
        SkillNameText.text = SkillName;
        SkillImage.sprite = SkillSpriteRenderer.sprite;
        SetColor(1);
    }

    public void OnPointerEnter(PointerEventData eventData) { // 마우스가 스킬 위라면 툴팁 전시
        if (SkillPrefab != null) {
            SkillToolTip.ShowToolTip(SkillPrefab);
        }
    }

    public void OnPointerExit(PointerEventData eventData) { // 마우스가 스킬을 벗어나면 툴팁 숨김
        SkillToolTip.HideToolTip();
    }

    public void OnBeginDrag(PointerEventData eventData) { // 스킬 드래그 시작 시 
        if (QuickSlotReference != null) {
            QuickSlotReference.CheckCoolDown();
        }

        if(SkillPrefab != null && !CantMove && CanDelete) {
            SkillDrag.Instance.SkillDragSlot = this;
            SkillDrag.Instance.DragSetImage(SkillImage);

            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            SkillDrag.Instance.transform.position = this.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData) { // 드래그 중일 때 드래그 된 아이템이 마우스를 따라가도록
        if (SkillPrefab != null && !CantMove) 
        {
            Vector3 GlobalMousePos;

            //마우스의 스크린 좌표를 월드 좌표로 변환
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(SkillDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out GlobalMousePos))
            {
                SkillDrag.Instance.MyRectTransform.position = GlobalMousePos; // // 드래그 중인 스킬의 RectTransform 위치를 마우스가 위치한 월드 좌표로 업데이트하여, 스킬이 마우스를 따라가도록
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) { // 스킬 드래그 종료 시
        SkillDrag.Instance.SetColor(0);
        SkillDrag.Instance.SkillDragSlot = null;
    }
}
