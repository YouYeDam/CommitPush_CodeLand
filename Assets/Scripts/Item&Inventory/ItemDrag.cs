using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour
{
    static public ItemDrag Instance;
    public Slot DragSlot;
    public RectTransform MyRectTransform;
    public Canvas UIManager;

    [SerializeField] private Image ItemImage;

    void Awake()
    {
        MyRectTransform = GetComponent<RectTransform>();
        Instance = this;
    }

    public void DragSetImage(Image ItemImage) { // 아이템 드래그 시 이미지 띄우기
        this.ItemImage.sprite = ItemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float Alpha) { // 알파값 1로 조정
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }
}
