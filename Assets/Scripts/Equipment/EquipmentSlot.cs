using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item Item; // 장비 아이템
    public Image ItemImage;  // 아이템의 이미지
    ItemToolTip ItemToolTip;
    EquipmentItem EquipmentItem;

    [SerializeField] GameObject UICanvas;
    public Inventory InventoryScript;

    private int ClickCount = 0;
    private float LastClickTime = 0f;
    private const float DoubleClickTime = 0.2f; // 더블 클릭 간격

    void Start() {
        GameObject ToolTipObject = GameObject.Find("EquipmentToolTip");
        if (ToolTipObject != null) {
        ItemToolTip = ToolTipObject.GetComponent<ItemToolTip>();
        }
    }

    public void ConnectUIManager() {
        UICanvas = GameObject.Find("UIManager");
        if (UICanvas != null) {
            InventoryScript = UICanvas.GetComponent<Inventory>();
        }
    }
    public void InitialEquip(bool IsLoaded) {
            if (Item != null) { // 초기 아이템 설정
            EquipmentItem EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
            EquipmentItem.PlayerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            // if (!IsLoaded) { 
                AddItem(Item, IsLoaded); // 이게 하나의 일만 하는게 아니라서 문제를 일으킨다...
            // }
        }
    }
    public void SetColor(float Alpha){ // 아이템 이미지의 투명도 조절
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }

    public void ClearSlot() { // 장비 슬롯에 아이템 탈착
        Debug.Log("log112: 1111 "+EquipmentItem);
        EquipmentItem.DecreaseStat();
        Debug.Log("log112: 2222");
        Item = null;
        Debug.Log("log112: 3333");
        ItemImage.sprite = null;
        SetColor(0);
    }

    public void OnPointerClick(PointerEventData eventData) {
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

    void OnDoubleClick() // 슬롯 더블클릭
    {
        if (Item != null) {
        InventoryScript.AcquireItem(Item);
        ClearSlot();
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

    public void AddItem(Item Item, bool IsLoaded) { //장비 슬롯에 아이템 장착
        EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
        if(!IsLoaded){
            this.Item = Item;
            EquipmentItem.IncreaseStat();
            ItemImage.sprite = this.Item.ItemImage;
            SetColor(1);
        }
    }

    public void AddItem(Item Item) { // 다형성
        this.Item = Item;
        EquipmentItem = Item.ItemPrefab.GetComponent<EquipmentItem>();
        EquipmentItem.IncreaseStat();
        ItemImage.sprite = this.Item.ItemImage;
        SetColor(1);
    }

    public void CheckNull(Item NewItem) {
        if (Item == null) {
            AddItem(NewItem);
        }
        else { 
            //InventoryScript.AcquireItem(Item); //-> 얘가 문제였음. 중복 작동하는 거 같음. 문제 되면 여기 우선적으로 확인
            ClearSlot();
            AddItem(NewItem);
        }
    }
}
