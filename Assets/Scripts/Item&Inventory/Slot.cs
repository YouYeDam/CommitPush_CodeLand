using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item Item;
    public int ItemCount;
    public Image ItemImage;
    Rect InventoryRect;
    public DropItemInputNumber DropItemInputNumber;
    public SellItemInputField SellItemInputField;
    public ItemQuickSlot QuickSlotReference;
    ItemToolTip ItemToolTip;
    PlayerMovement PlayerMovement;
    PlayerStatus PlayerStatus;
    PlayerMoney PlayerMoney;
    [SerializeField] TMP_Text TextCount;
    [SerializeField] GameObject CountImage;
    QuestManager QuestManager;

    // 스킬북 기능 구현 변수
    [SerializeField] private GameObject SkillContent; // 스킬 슬롯의 부모인 Content
    private SkillSlot[] SkillSlots; // 스킬 슬롯들 배열

    // 더블클릭 기능 구현 변수
    private int ClickCount = 0;
    private float LastClickTime = 0f;
    private const float DoubleClickTime = 0.2f; // 더블 클릭 간격

    public bool IsSyncing = false; // 동기화 플래그

    void Start() {
        InventoryRect = transform.parent.parent.parent.GetComponent<RectTransform>().rect;
        DropItemInputNumber = FindObjectOfType<DropItemInputNumber>();
        SellItemInputField = FindObjectOfType<SellItemInputField>();

        GameObject ToolTipObject = GameObject.Find("ItemToolTip");
        if (ToolTipObject != null) {
        ItemToolTip = ToolTipObject.GetComponent<ItemToolTip>();
        }
        
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        PlayerMoney  = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>();

        SkillSlots = SkillContent.GetComponentsInChildren<SkillSlot>();
        QuestManager = FindObjectOfType<QuestManager>();
    }

    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    public void AddItem(Item Item, int Count = 1, ItemQuickSlot QuickSlot = null) { // 인벤토리 슬롯에 새로운 아이템 추가
        this.Item = Item;
        ItemCount = Count;
        ItemImage.sprite = this.Item.ItemImage;
        QuickSlotReference = QuickSlot;

        if (this.Item.Type == Item.ItemType.Used || this.Item.Type == Item.ItemType.ETC) { // 소비 혹은 기타 아이템일 경우에만 수량 표시
            CountImage.SetActive(true);
            TextCount.text = ItemCount.ToString();
        }
        else {
            TextCount.text = "0";
            CountImage.SetActive(false);
        }
        SetColor(1);
    }

    public void SetSlotCount(int Count) { // 슬롯의 아이템 수량 설정 기능
        ItemCount += Count;
        TextCount.text = ItemCount.ToString();

        if (ItemCount <= 0) { // 아이템을 모두 사용하면(수량이 0이되면) 슬롯 비우기
            if (QuickSlotReference != null && !QuickSlotReference.IsSyncing) { // 퀵슬롯에 연동되어 있다면 연동 해제
                IsSyncing = true;
                QuickSlotReference.ClearSlot(); // 퀵슬롯도 초기화
                IsSyncing = false;
            }
            ClearSlot();
        } 
        else if (QuickSlotReference != null && !QuickSlotReference.IsSyncing) { // 퀵슬롯도 아이템 수량 조정
            IsSyncing = true;
            QuickSlotReference.SetSlotCount(Count); // 퀵슬롯과 동기화
            IsSyncing = false;
        }
    }

    void ClearSlot() { // 해당 슬롯 하나 삭제
        Item = null;
        ItemCount = 0;
        ItemImage.sprite = null;
        SetColor(0);

        TextCount.text = "0";
        CountImage.SetActive(false);
        if (ItemToolTip != null) {
            ItemToolTip.HideToolTip();
        }
        if (QuickSlotReference != null) { // QuickSlotReference 초기화
            QuickSlotReference.SlotReference = null; // 연동 초기화
            QuickSlotReference = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData) { // 더블 클릭 기능
        ClickCount++;
        if (ClickCount == 1)
        {
            LastClickTime = Time.time;
        }
        else if (ClickCount == 2 && Time.time - LastClickTime < DoubleClickTime)
        {
            PlayerUI PlayerUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
            if (PlayerUI.Shop.activeSelf) { // 상점을 이용중이면 아이템 더블 클릭시 아이템 판매
                OnDoubleClickForSell();
            }
            else { // 이외에는 아이템 사용(착용)
                OnDoubleClickForUse(); 
                ClickCount = 0; // 클릭 카운트 초기화
            }
        }
        else if (Time.time - LastClickTime > DoubleClickTime)
        {
            ClickCount = 1; // 시간 초과로 다시 카운트
            LastClickTime = Time.time;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { // 마우스가 아이템 위라면 툴팁 전시
        if (Item != null) {
            ItemToolTip.ShowToolTip(Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData) { // 마우스가 아이템을 벗어나면 툴팁 숨김
        ItemToolTip.HideToolTip();
    }

    public void OnBeginDrag(PointerEventData eventData) { // 아이템 드래그 시작 시 
        if(Item != null)
        {
            ItemDrag.Instance.DragSlot = this;
            ItemDrag.Instance.DragSetImage(ItemImage);

            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            ItemDrag.Instance.transform.position = this.transform.position;
            SetColor(0.5f); // 반투명하게 설정
        }
    }

    public void OnDrag(PointerEventData eventData) { // 드래그 중일 때 드래그 된 아이템이 마우스를 따라가도록
        if (Item != null) 
        {
            Vector3 GlobalMousePos;

            //마우스의 스크린 좌표를 월드 좌표로 변환
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(ItemDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out GlobalMousePos))
            {
                ItemDrag.Instance.MyRectTransform.position = GlobalMousePos; // 드래그 중인 아이템의 RectTransform 위치를 마우스가 위치한 월드 좌표로 업데이트하여, 아이템이 마우스를 따라가도록
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) { // 아이템 드래그 종료 시
        bool IsOutsideInventory = ItemDrag.Instance.transform.localPosition.x < InventoryRect.xMin  // 인벤토리를 벗어나는지 검사
            || ItemDrag.Instance.transform.localPosition.x > InventoryRect.xMax
            || ItemDrag.Instance.transform.localPosition.y < InventoryRect.yMin
            || ItemDrag.Instance.transform.localPosition.y > InventoryRect.yMax;

        if (IsOutsideInventory) { // 인벤토리를 벗어나서 드래그 종료 시 드랍 입력 필드 팝업
            if (ItemDrag.Instance.DragSlot != null) {
                DropItemInputNumber.OpenInputField();
                ItemDrag.Instance.SetColor(0);
            }
        }
        else { // 인벤토리 안이라면 인스턴스 초기화
            ItemDrag.Instance.SetColor(0);
            ItemDrag.Instance.DragSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData) { // 아이템 슬롯에 드랍 시
        if (ItemDrag.Instance.DragSlot != null) { // 드랍 시 슬롯 간 아이템 교환
            ChangeSlot(); 
        }
    }

    void ChangeSlot() { // 슬롯 간 아이템 교환 기능
        Item TempItem = Item;
        int TempItemCount = ItemCount;
        ItemQuickSlot TempQuickSlotReference = QuickSlotReference;

        AddItem(ItemDrag.Instance.DragSlot.Item, ItemDrag.Instance.DragSlot.ItemCount, ItemDrag.Instance.DragSlot.QuickSlotReference); // 드랍하는 슬롯에 드래그 한 아이템 넣기

        if (TempItem != null) { // 드랍하는 슬롯에 아이템이 존재한다면 원래 드래그 한 슬롯에 아이템 넣기
            ItemDrag.Instance.DragSlot.AddItem(TempItem, TempItemCount, TempQuickSlotReference);
        }
        else {
            ItemDrag.Instance.DragSlot.ClearSlot();
        }

        // 퀵슬롯 참조 동기화
        if (QuickSlotReference != null) {
            QuickSlotReference.SlotReference = this;
        }

        if (ItemDrag.Instance.DragSlot.QuickSlotReference != null) {
            ItemDrag.Instance.DragSlot.QuickSlotReference.SlotReference = ItemDrag.Instance.DragSlot;
        }
    }

    void OnDoubleClickForUse() // 슬롯 더블클릭
    {   
        if (Item == null || !PlayerMovement.IsAlive) {
            return;
        }

        if (Item.Type == Item.ItemType.Used) { // 소비 아이템시 실행
            UsedItem UsedItem = Item.ItemPrefab.GetComponent<UsedItem>();
            UsedItem.PlayerStatus = PlayerStatus;

            if (UsedItem != null) {
                UsedItem.EffectItem();
                SetSlotCount(-1);
            }
        }
        else if (Item.Type == Item.ItemType.Equipment) { // 장비 아이템시 실행
            EquipmentItem EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
            EquipmentItem.PlayerStatus = PlayerStatus;

            if (EquipmentItem != null) {
                if (PlayerStatus.PlayerLevel >= EquipmentItem.RequireLevel) {
                    EquipmentItem.EquipItem(Item);
                    ClearSlot();
                }
                else { // 요구 레벨 미충족시 장착하지 않고 반환
                    return;
                }
            }
        }
        else if (Item.Type == Item.ItemType.SourceCode) { // 스킬북 아이템시 실행
            SourceCodeItem SourceCodeItem = Item.ItemPrefab.GetComponent<SourceCodeItem>();

            if (SourceCodeItem != null) {
                for (int i = 0; i < SkillSlots.Length; i++) {
                    if (SourceCodeItem.SkillName == SkillSlots[i].SkillName) { // 이름이 같으면 스킬을 추가하지 않고 반환
                        return;
                    }
                }
            }

            if (SourceCodeItem != null) {
                for (int i = 0; i < SkillSlots.Length; i++) {
                    if (SkillSlots[i].SkillPrefab == null) {
                        SkillSlots[i].AddSkill(SourceCodeItem.SkillPrefab); // 스킬 추가
                        ClearSlot();
                        return;
                    }
                }
            }
        }
    }

    void OnDoubleClickForSell() { // 아이템 판매 더블클릭
        if (Item == null || !PlayerMovement.IsAlive) {
            return;
        }

        if (Item.Type == Item.ItemType.Quest) { // 퀘스트 아이템시 판매불가
            return;
        }
        else if (Item.Type == Item.ItemType.Used || Item.Type == Item.ItemType.ETC) { // 소비, 기타 아이템시 실행
            SellItemInputField.OpenInputField(this); // 여러 수량을 판매할 수 있도록 입력창 팝업
        }
        else { // 그 외 아이템 시 실행
            PlayerMoney.Bit += Mathf.RoundToInt(Item.ItemCost * 0.7f); // 판매할 때 아이템 가격 70%로 조정
            QuestManager.UpdateRemoveObjective(Item.ItemName, 1); // QuestManager에서 아이템 제거 목표 업데이트
            ClearSlot();
        }
    }

    public void SellManyItem(int SellItemCount) { // 다수의 수량을 판매할 경우 실행
        int TotalItemCost = Item.ItemCost * SellItemCount;
        PlayerMoney.Bit += Mathf.RoundToInt(TotalItemCost * 0.7f); // 판매할 때 아이템 가격 70%로 조정
        QuestManager.UpdateRemoveObjective(Item.ItemName, SellItemCount); // QuestManager에서 아이템 제거 목표 업데이트
        
        SetSlotCount(-SellItemCount);
    }
}