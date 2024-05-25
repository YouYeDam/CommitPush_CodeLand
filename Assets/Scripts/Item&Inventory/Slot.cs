using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item Item; // 획득한 아이템
    public int ItemCount; // 획득한 아이템의 개수
    public Image ItemImage;  // 아이템의 이미지
    Rect InventoryRect;
    public DropItemInputNumber DropItemInputNumber;
    public SellItemInputField SellItemInputField;
    public ItemQuickSlot QuickSlotReference;
    ItemToolTip ItemToolTip;
    PlayerMovement PlayerMovement;
    PlayerMoney PlayerMoney;
    [SerializeField] TMP_Text TextCount;
    [SerializeField] GameObject CountImage;

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
        PlayerMoney  = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>();
    }
    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    public void AddItem(Item Item, int Count = 1, ItemQuickSlot QuickSlot = null) { // 인벤토리에 새로운 아이템 슬롯 추가
        this.Item = Item;
        ItemCount = Count;
        ItemImage.sprite = this.Item.ItemImage;
        QuickSlotReference = QuickSlot;

        if (this.Item.Type != Item.ItemType.Equipment)
        {
            CountImage.SetActive(true);
            TextCount.text = ItemCount.ToString();
        }
        else
        {
            TextCount.text = "0";
            CountImage.SetActive(false);
        }
        SetColor(1);
    }

    
    public void SetSlotCount(int Count) {
        ItemCount += Count;
        TextCount.text = ItemCount.ToString();

        if (ItemCount <= 0) {
            ClearSlot();
        }

        if (QuickSlotReference != null && !QuickSlotReference.IsSyncing) {
            IsSyncing = true;
            QuickSlotReference.SetSlotCount(Count); // QuickSlot과 동기화
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

        // QuickSlotReference 초기화
        if (QuickSlotReference != null) {
            QuickSlotReference.SlotReference = null;
            QuickSlotReference = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        ClickCount++;
        if (ClickCount == 1)
        {
            LastClickTime = Time.time;
        }
        else if (ClickCount == 2 && Time.time - LastClickTime < DoubleClickTime)
        {
            PlayerUI PlayerUI = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUI>();
            if (PlayerUI.Shop.activeSelf) {
                OnDoubleClickForSell();
            }
            else {
                OnDoubleClickForUse(); // 더블 클릭 발생
                ClickCount = 0; // 클릭 카운트 초기화
            }
        }
        else if (Time.time - LastClickTime > DoubleClickTime)
        {
            ClickCount = 1; // 시간 초과로 다시 1부터 카운트
            LastClickTime = Time.time;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Item != null) {
            ItemToolTip.ShowToolTip(Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        ItemToolTip.HideToolTip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Item != null)
        {
            ItemDrag.Instance.DragSlot = this;
            ItemDrag.Instance.DragSetImage(ItemImage);
            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            ItemDrag.Instance.transform.position = this.transform.position;
            SetColor(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item != null) 
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(ItemDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                ItemDrag.Instance.MyRectTransform.position = globalMousePos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool IsOutsideInventory = ItemDrag.Instance.transform.localPosition.x < InventoryRect.xMin  // 인벤토리를 벗어나는지 검사
            || ItemDrag.Instance.transform.localPosition.x > InventoryRect.xMax
            || ItemDrag.Instance.transform.localPosition.y < InventoryRect.yMin
            || ItemDrag.Instance.transform.localPosition.y > InventoryRect.yMax;

        if (IsOutsideInventory)
        {
            if (ItemDrag.Instance.DragSlot != null)
            {
                DropItemInputNumber.OpenInputField();
                ItemDrag.Instance.SetColor(0);
            }
        }
        else
        {
            ItemDrag.Instance.SetColor(0);
            ItemDrag.Instance.DragSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDrag.Instance.DragSlot != null) 
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Item TempItem = Item;
        int TempItemCount = ItemCount;

        AddItem(ItemDrag.Instance.DragSlot.Item, ItemDrag.Instance.DragSlot.ItemCount, ItemDrag.Instance.DragSlot.QuickSlotReference);

        if (TempItem != null) 
        {
            ItemDrag.Instance.DragSlot.AddItem(TempItem, TempItemCount);
        }
        else 
        {
            ItemDrag.Instance.DragSlot.ClearSlot();

        }
    }

    void OnDoubleClickForUse() // 슬롯 더블클릭
    {
        if (Item == null || !PlayerMovement.IsAlive) {
            return;
        }
        if (Item.Type == Item.ItemType.Used) // 소비 아이템시 실행
        {
            UsedItem UsedItem = Item.ItemPrefab.GetComponent<UsedItem>();
            UsedItem.PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            if (UsedItem != null)
            {
                UsedItem.EffectItem();
                SetSlotCount(-1);
            }
        }
        else if (Item.Type == Item.ItemType.Equipment) // 장비 아이템시 실행
        {
            EquipmentItem EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
            EquipmentItem.PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            if (EquipmentItem != null)
            {
                EquipmentItem.EquipItem(Item);
                ClearSlot();
            }
        }
    }

    void OnDoubleClickForSell()
    {
        if (Item == null || !PlayerMovement.IsAlive) {
            return;
        }
        if (Item.Type == Item.ItemType.Used) // 소비 아이템시 실행
        {
            SellItemInputField.OpenInputField(this);
        }
        else if (Item.Type == Item.ItemType.Equipment) // 장비 아이템시 실행
        {
            PlayerMoney.Bit += Item.ItemCost;
            ClearSlot();
        }
    }

    public void SellManyItem(int SellItemCount) {
        int TotalItemCost = Item.ItemCost * SellItemCount;
        PlayerMoney.Bit += TotalItemCost;
        SetSlotCount(-SellItemCount);
    }
}