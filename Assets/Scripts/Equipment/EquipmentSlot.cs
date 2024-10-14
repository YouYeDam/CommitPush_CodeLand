using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item Item; // 장비 아이템
    public Image ItemImage;
    ItemToolTip ItemToolTip;
    EquipmentItem EquipmentItem;
    [SerializeField] GameObject UICanvas;
    Inventory InventoryScript;

    private int ClickCount = 0;
    private float LastClickTime = 0f;
    private const float DoubleClickTime = 0.2f; // 더블 클릭 간격

    void Start() {
        GameObject ToolTipObject = GameObject.Find("EquipmentToolTip");
        if (ToolTipObject != null) {
        ItemToolTip = ToolTipObject.GetComponent<ItemToolTip>();
        }
    }

    public void ConnectUIManager() { // UI 매니저 연결
        UICanvas = GameObject.Find("UIManager");
        if (UICanvas != null) {
            InventoryScript = UICanvas.GetComponent<Inventory>();
        }
    }

    public void InitialEquip() { // 초기 아이템 설정
            if (Item != null) {
            EquipmentItem EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
            EquipmentItem.PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            AddItem(Item);
        }
    }

    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    void ClearSlot() { // 장비 슬롯에 아이템 탈착
        EquipmentItem.DecreaseStat();
        Item = null;
        ItemImage.sprite = null;
        SetColor(0);
    }

    public void OnPointerClick(PointerEventData eventData) { // 더블클릭 기능
        ClickCount++;
        if (ClickCount == 1)
        {
            LastClickTime = Time.time;
        }
        else if (ClickCount == 2 && Time.time - LastClickTime < DoubleClickTime)
        {
            OnDoubleClick(); // 더블 클릭 발생
            ClickCount = 0; // 클릭 카운트 초기화
        }
        else if (Time.time - LastClickTime > DoubleClickTime)
        {
            ClickCount = 1; // 시간 초과로 다시 1부터 카운트
            LastClickTime = Time.time;
        }
    }

    void OnDoubleClick() { // 슬롯 더블클릭시 아이템 탈착 및 인벤토리로 이동
        if (Item != null) {
        InventoryScript.AcquireItem(Item);
        ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) { // 마우스가 장비 아이템 위에 있을 시 툴팁 전시
        if (Item != null) {
            ItemToolTip.ShowToolTip(Item);
        }
    }

    public void OnPointerExit(PointerEventData eventData) { // 마우스가 장비 아이템 위에서 벗어날 시 툴팁 숨김
        ItemToolTip.HideToolTip();
    }

    public void AddItem(Item Item) { //장비 슬롯에 아이템 장착
        this.Item = Item;
        ItemImage.sprite = this.Item.ItemImage;
        SetColor(1);
        EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
        if (EquipmentItem.IsSpecialEquipment) {
            SpecialEquipment SpecialEquipment = GetComponent<SpecialEquipment>();
            SpecialEquipment.SpecialEquipmentEffect(EquipmentItem.EquipmentType, Item.ItemName);
        }
        EquipmentItem.IncreaseStat();
    }

    public void CheckNull(Item NewItem) { // 장비 슬롯이 비었는지 확인
        if (Item == null) { // 비었으면 곧바로 슬롯에 장착
            AddItem(NewItem);
        }
        else { // 안비었다면 아이템을 장착중인 아이템을 인벤토리로 이동시킨 후 슬롯에 장착
            InventoryScript.AcquireItem(Item);
            ClearSlot();
            AddItem(NewItem);
        }
    }
}
