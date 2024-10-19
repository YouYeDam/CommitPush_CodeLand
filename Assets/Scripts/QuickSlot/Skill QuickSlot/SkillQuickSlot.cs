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
    public Image CoolDownPanel; // 쿨타임 패널
    public TMP_Text CoolDownText; // 쿨타임 텍스트
    public SkillSlot SlotReference;
    [SerializeField] GameObject QuickSlotBase;
    [SerializeField] string ButtonKey;
    PlayerSkills PlayerSkills;
    bool CanDelete = true;

    void Start() {
        PlayerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
        StartCoroutine(CoolDownCoroutine()); // 쿨타임 코루틴 시작
    }

    public void SetColor(float Alpha) { // 스킬 이미지의 투명도 조절
        Color color = SkillImage.color;
        color.a = Alpha;
        SkillImage.color = color;
    }

    public void SetCoolDownPanel(float Alpha) { // 패널 이미지 투명도 조절
        Color color = CoolDownPanel.color;
        color.a = Alpha;
        CoolDownPanel.color = color;
    }

    public void AddSkill(GameObject SkillPrefab, SkillSlot Slot = null) { // 퀵슬롯에 새로운 스킬 추가
        this.SkillPrefab = SkillPrefab;
        SpriteRenderer SkillSpriteRenderer = this.SkillPrefab.GetComponent<SpriteRenderer>();
        SkillImage.sprite = SkillSpriteRenderer.sprite;
        SlotReference = Slot; // SlotReference 설정
        SetColor(1);

        if (SlotReference != null) { // Slot과 동기화
            SlotReference.QuickSlotReference = this;
        }

        AddButtonKey();
    }

    void ClearSlot() { // 해당 슬롯 하나 삭제
        DeleteButtonKey();
        SkillPrefab = null;
        SetColor(0);
        CanDelete = true;
        SlotReference.CanDelete = true;
        // SlotReference 초기화
        if (SlotReference != null) {
            SlotReference.QuickSlotReference = null;
            SlotReference = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        CheckCoolDown();
        if (SkillPrefab != null && CanDelete) {
            SkillQuickSlotSkillDrag.Instance.DragSkillQuickSlot = this;
            SkillQuickSlotSkillDrag.Instance.DragSetImage(SkillImage);
            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            SkillQuickSlotSkillDrag.Instance.transform.position = this.transform.position;
            SetColor(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (SkillPrefab != null) {
            Vector3 GlobalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(SkillQuickSlotSkillDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out GlobalMousePos)) {
                SkillQuickSlotSkillDrag.Instance.MyRectTransform.position = GlobalMousePos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        CheckCoolDown(); // 쿨타임이 남아있는지 확인
        if (!CanDelete) {
            return;
        }
        ClearSlot();
        SkillQuickSlotSkillDrag.Instance.SetColor(0);
        SkillQuickSlotSkillDrag.Instance.DragSkillQuickSlot = null;
    }

    public void OnDrop(PointerEventData eventData) { // 스킬이 퀵슬롯에 드롭되었을 때 퀵슬롯에 스킬 등록
        if (SlotReference != null) { // 쿨타임이 남아있는지 확인
            CheckCoolDown();
        }
        if (SkillDrag.Instance == null || !CanDelete) { // 쿨타임이 남아있다면 삭제 불가
            return;
        }
        if (SkillDrag.Instance.SkillDragSlot != null) { // 이미 퀵슬롯에 스킬이 있다면 비우기
            if (this.SkillPrefab != null) {
                ClearSlot();
            }

            if (SkillDrag.Instance.SkillDragSlot.QuickSlotReference != null) {
                SkillDrag.Instance.SkillDragSlot.QuickSlotReference.ClearSlot();
            }
            
            AddSkill(SkillDrag.Instance.SkillDragSlot.SkillPrefab, SkillDrag.Instance.SkillDragSlot);
        }
    }

    void AddButtonKey() {
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

    void DeleteButtonKey() {
        CanDelete = true;
        switch (ButtonKey) {
            case "Q":
                PlayerSkills.QSkill = null;
                break;
            case "W":
                PlayerSkills.WSkill = null;
                break;
            case "E":
                PlayerSkills.ESkill = null;
                break;
            case "R":
                PlayerSkills.RSkill = null;
                break;
            case "S":
                PlayerSkills.SSkill = null;
                break;
            case "D":
                PlayerSkills.DSkill = null;
                break;
            default:
                break;
        }
    }

    public void CheckCoolDown() { // 쿨타임 체크
        CanDelete = false;
        SlotReference.CanDelete = false;
        switch (ButtonKey) {
            case "Q":
                if (PlayerSkills.CanQSkill == true) {
                    CanDelete = true;
                    SlotReference.CanDelete = true;
                }
                break;
            case "W":
                if (PlayerSkills.CanWSkill == true) {
                    CanDelete = true;
                    SlotReference.CanDelete = true;
                }
                break;
            case "E":
                if (PlayerSkills.CanESkill == true) {
                    CanDelete = true;
                    SlotReference.CanDelete = true;
                }
                break;
            case "R":
                if (PlayerSkills.CanRSkill == true) {
                    CanDelete = true;
                    SlotReference.CanDelete = true;
                }
                break;
            case "S":
                if (PlayerSkills.CanSSkill == true) {
                    CanDelete = true;
                    SlotReference.CanDelete = true;
                }
                break;
            case "D":
                if (PlayerSkills.CanDSkill == true) {
                    CanDelete = true;
                    SlotReference.CanDelete = true;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator CoolDownCoroutine() {
        while (true) {
            yield return null;
            UpdateCoolDownUI();
        }
    }

    void UpdateCoolDownUI() { // 쿨타임 UI 최신화
        float remainingCoolDown = 0;
        bool CanUse = false;

        switch (ButtonKey) {
            case "Q":
                remainingCoolDown = PlayerSkills.QSkillRemainingCoolDown;
                CanUse = PlayerSkills.CanQSkill;
                break;
            case "W":
                remainingCoolDown = PlayerSkills.WSkillRemainingCoolDown;
                CanUse = PlayerSkills.CanWSkill;
                break;
            case "E":
                remainingCoolDown = PlayerSkills.ESkillRemainingCoolDown;
                CanUse = PlayerSkills.CanESkill;
                break;
            case "R":
                remainingCoolDown = PlayerSkills.RSkillRemainingCoolDown;
                CanUse = PlayerSkills.CanRSkill;
                break;
            case "S":
                remainingCoolDown = PlayerSkills.SSkillRemainingCoolDown;
                CanUse = PlayerSkills.CanSSkill;
                break;
            case "D":
                remainingCoolDown = PlayerSkills.DSkillRemainingCoolDown;
                CanUse = PlayerSkills.CanDSkill;
                break;
            default:
                break;
        }

        if (CanUse) {
            SetCoolDownPanel(0);
            CoolDownText.text = "";
        } else {
            SetCoolDownPanel(0.5f);
            CoolDownText.text = Mathf.Ceil(remainingCoolDown).ToString();
        }
    }
}
