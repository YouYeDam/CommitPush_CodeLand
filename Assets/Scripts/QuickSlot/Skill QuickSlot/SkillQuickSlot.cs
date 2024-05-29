using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillQuickSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public GameObject SkillPrefab;
    public Image SkillImage;  // 스킬의 이미지
    public SkillSlot SlotReference;
    [SerializeField] GameObject QuickSlotBase;
    [SerializeField] string ButtonKey;
    PlayerSkills PlayerSkills;

    void Start() {
        PlayerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
    }
    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = SkillImage.color;
        Color.a = Alpha;
        SkillImage.color = Color;
    }

    public void AddItem(GameObject SkillPrefab, SkillSlot Slot = null) { // 퀵슬롯에 새로운 스킬 추가
        this.SkillPrefab = SkillPrefab;
        SpriteRenderer SkillSpriteRenderer = this.SkillPrefab.GetComponent<SpriteRenderer>();
        SkillImage.sprite = SkillSpriteRenderer.sprite;
        SlotReference = Slot; // SlotReference 설정
        SetColor(1);

        if (SlotReference != null) // Slot과 동기화
        {
            SlotReference.QuickSlotReference = this;
        }

        SetButtonKey();
    }

    void ClearSlot() { // 해당 슬롯 하나 삭제
        SkillPrefab = null;
        SetColor(0);
        // SlotReference 초기화
        if (SlotReference != null) {
            SlotReference.QuickSlotReference = null;
            SlotReference = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(SkillPrefab != null)
        {
            SkillQuickSlotSkillDrag.Instance.DragSkillQuickSlot = this;
            SkillQuickSlotSkillDrag.Instance.DragSetImage(SkillImage);
            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            SkillQuickSlotSkillDrag.Instance.transform.position = this.transform.position;
            SetColor(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SkillPrefab != null) 
        {
            Vector3 GlobalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(SkillQuickSlotSkillDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out GlobalMousePos))
            {
                SkillQuickSlotSkillDrag.Instance.MyRectTransform.position = GlobalMousePos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ClearSlot();
        SkillQuickSlotSkillDrag.Instance.SetColor(0);
        SkillQuickSlotSkillDrag.Instance.DragSkillQuickSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (SkillDrag.Instance == null) {
            return;
        }
        if (SkillDrag.Instance.SkillDragSlot != null) 
        {
            if (SkillDrag.Instance.SkillDragSlot.QuickSlotReference != null) {
                SkillDrag.Instance.SkillDragSlot.QuickSlotReference.ClearSlot();
            }
            AddItem(SkillDrag.Instance.SkillDragSlot.SkillPrefab, SkillDrag.Instance.SkillDragSlot);
        }
    }
    void SetButtonKey() {
        switch (ButtonKey) {
            case "Q":
                PlayerSkills.QSkill = SkillPrefab;
                PlayerSkills.SetSkillsCoolTime(ButtonKey);
                break;
            case "W":
                PlayerSkills.WSkill = SkillPrefab;
                PlayerSkills.SetSkillsCoolTime(ButtonKey);
                break;
            case "E":
                PlayerSkills.ESkill = SkillPrefab;
                PlayerSkills.SetSkillsCoolTime(ButtonKey);
                break;
            case "R":
                PlayerSkills.RSkill = SkillPrefab;
                PlayerSkills.SetSkillsCoolTime(ButtonKey);
                break;
            case "S":
                PlayerSkills.SSkill = SkillPrefab;
                PlayerSkills.SetSkillsCoolTime(ButtonKey);
                break;
            case "D":
                PlayerSkills.DSkill = SkillPrefab;
                PlayerSkills.SetSkillsCoolTime(ButtonKey);
                break;
            default:
                break;
        }
    }

}