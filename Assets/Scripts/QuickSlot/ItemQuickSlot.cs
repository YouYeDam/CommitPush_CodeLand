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
    Rect QuickSlotRect;
    PlayerMovement PlayerMovement;
    [SerializeField] TMP_Text TextCount;
    [SerializeField] GameObject CountImage;

    public bool IsSyncing = false; // 동기화 플래그

    void Start() {
        QuickSlotRect = QuickSlotBase.GetComponent<RectTransform>().rect;
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    public void AddItem(Item Item, int Count = 1, Slot Slot = null) { // 퀵슬롯에 새로운 아이템 슬롯 추가
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
    
    public void SetSlotCount(int Count) {
        ItemCount += Count;
        TextCount.text = ItemCount.ToString();

        if (ItemCount <= 0) {
            ClearSlot();
        }

        if (SlotReference != null && !SlotReference.IsSyncing) {
            IsSyncing = true;
            SlotReference.SetSlotCount(Count); // Slot과 동기화
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
        // SlotReference 초기화
        if (SlotReference != null) {
            SlotReference.QuickSlotReference = null;
            SlotReference = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Item != null)
        {
            QuickSlotItemDrag.Instance.DragItemQuickSlot = this;
            QuickSlotItemDrag.Instance.DragSetImage(ItemImage);
            // 현재 슬롯의 월드 좌표를 드래그 객체의 위치로 설정
            QuickSlotItemDrag.Instance.transform.position = this.transform.position;
            SetColor(0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item != null) 
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(QuickSlotItemDrag.Instance.MyRectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                QuickSlotItemDrag.Instance.MyRectTransform.position = globalMousePos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool IsOutsideQuickSlot = QuickSlotItemDrag.Instance.transform.localPosition.x < QuickSlotRect.xMin // 퀵슬롯을 벗어나는지 검사
            || QuickSlotItemDrag.Instance.transform.localPosition.x > QuickSlotRect.xMax
            || QuickSlotItemDrag.Instance.transform.localPosition.y < QuickSlotRect.yMin
            || QuickSlotItemDrag.Instance.transform.localPosition.y > QuickSlotRect.yMax;

        if (IsOutsideQuickSlot)
        {
            ClearSlot();
        }
        QuickSlotItemDrag.Instance.SetColor(0);
        QuickSlotItemDrag.Instance.DragItemQuickSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (ItemDrag.Instance.DragSlot != null) 
        {
            if (ItemDrag.Instance.DragSlot.QuickSlotReference != null) {
                ItemDrag.Instance.DragSlot.QuickSlotReference.ClearSlot();
            }
            AddItem(ItemDrag.Instance.DragSlot.Item, ItemDrag.Instance.DragSlot.ItemCount, ItemDrag.Instance.DragSlot);
            DropItemInputNumber DropItemInputNumber = FindObjectOfType<DropItemInputNumber>();
            DropItemInputNumber.Cancel();
        }

        if (QuickSlotItemDrag.Instance.DragItemQuickSlot != null)
        {
            //ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Item TempItem = Item;
        int TempItemCount = ItemCount;
        Slot TempSlotReference = SlotReference;
        AddItem(QuickSlotItemDrag.Instance.DragItemQuickSlot.Item, QuickSlotItemDrag.Instance.DragItemQuickSlot.ItemCount, QuickSlotItemDrag.Instance.DragItemQuickSlot.SlotReference);

        if (TempItem != null) 
        {
            QuickSlotItemDrag.Instance.DragItemQuickSlot.AddItem(TempItem, TempItemCount, TempSlotReference);
        }
        else 
        {
            QuickSlotItemDrag.Instance.DragItemQuickSlot.ClearSlot();

        }
    }
    public void UseItem() // 슬롯 더블클릭
    {
        if (Item == null || !PlayerMovement.IsAlive) {
            return;
        }
        UsedItem UsedItem = Item.ItemPrefab.GetComponent<UsedItem>();
        UsedItem.PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        if (UsedItem != null)
        {
            UsedItem.EffectItem();
            SetSlotCount(-1);
        }
    }
}