using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemQuickSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item Item; // 획득한 아이템
    public int ItemCount; // 획득한 아이템의 개수
    public Image ItemImage;  // 아이템의 이미지
    public Slot SlotReference;
    [SerializeField] GameObject QuickSlotBase;
    PlayerMovement PlayerMovement;
    [SerializeField] TMP_Text TextCount;
    [SerializeField] GameObject CountImage;
    [SerializeField] GameObject DropInputField;
    [SerializeField] GameObject BuyInputField;
    [SerializeField] GameObject SellInputField;

    public bool IsSyncing = false; // 동기화 플래그

    void Start() {
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    public void AddQuickSlotItem(Item Item, int Count = 1, Slot Slot = null) { // 퀵슬롯에 새로운 아이템 슬롯 추가
        this.Item = Item;
        ItemCount = Count;
        ItemImage.sprite = this.Item.ItemImage;
        SlotReference = Slot; // SlotReference 설정

        CountImage.SetActive(true);
        TextCount.text = ItemCount.ToString();
        SetColor(1);

        if (SlotReference != null) // Slot과 동기화
        {
            SlotReference.QuickSlotReference = this;
        }
    }
    
    public void SetSlotCount(int Count) { // 해당 퀵슬롯에 등록된 아이템 개수 카운트
        ItemCount += Count;
        TextCount.text = ItemCount.ToString();

        if (SlotReference != null && !SlotReference.IsSyncing) {
            IsSyncing = true;
            SlotReference.SetSlotCount(Count); // Slot과 동기화
            IsSyncing = false;
        }
        if (ItemCount <= 0) {
            ClearSlot();
        }
    }

    public void ClearSlot() { // 해당 슬롯 하나 삭제
        Item = null;
        ItemCount = 0;
        ItemImage.sprite = null;
        SetColor(0);

        TextCount.text = "0";
        CountImage.SetActive(false);
        
        if (SlotReference != null) {
            SlotReference.QuickSlotReference = null;
            SlotReference = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData) { // 퀵슬롯에 등록된 아이템 드래그 시작시
        if(Item != null) {
            ItemQuickSlotItemDrag.Instance.DragItemQuickSlot = this;
            ItemQuickSlotItemDrag.Instance.DragSetImage(ItemImage);
            
            ItemQuickSlotItemDrag.Instance.transform.position = this.transform.position; // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            SetColor(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData) { // 드래그 중일 때 드래그 된 아이템이 마우스를 따라가도록
        if (Item != null) {
            Vector3 GlobalMousePos;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(ItemQuickSlotItemDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out GlobalMousePos))
            {
                ItemQuickSlotItemDrag.Instance.MyRectTransform.position = GlobalMousePos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) { // 아이템 드래그 종료 시
        if (Item != null)
        {
            ClearSlot();
        }

        ItemQuickSlotItemDrag.Instance.SetColor(0);
        ItemQuickSlotItemDrag.Instance.DragItemQuickSlot = null;
    }

    public void OnDrop(PointerEventData eventData) {
        if (ItemDrag.Instance.DragSlot != null) {
            if (ItemDrag.Instance.DragSlot.Item.Type != Item.ItemType.Used) { // 소모품 타입이 아니라면 리턴
                return;
            }

            // 현재 퀵슬롯에 아이템이 있으면 해당 아이템과의 연동을 해제
            if (this.Item != null) {
                ClearSlot();
            }
            if (ItemDrag.Instance.DragSlot.QuickSlotReference != null) {
                ItemDrag.Instance.DragSlot.QuickSlotReference.ClearSlot();
            }
            AddQuickSlotItem(ItemDrag.Instance.DragSlot.Item, ItemDrag.Instance.DragSlot.ItemCount, ItemDrag.Instance.DragSlot);
            DropItemInputNumber DropItemInputNumber = FindObjectOfType<DropItemInputNumber>();
            DropItemInputNumber.Cancel();
        }
    }

    public void UseItem() { // 아이템 사용
        if (Item == null || !PlayerMovement.IsAlive) {
            return;
        }
        if (DropInputField.activeSelf || BuyInputField.activeSelf || SellInputField.activeSelf) {
            return;
        }
        
        UsedItem UsedItem = Item.ItemPrefab.GetComponent<UsedItem>();
        UsedItem.PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        if (UsedItem != null) {
            UsedItem.EffectItem();
            SetSlotCount(-1);
        }
    }
}