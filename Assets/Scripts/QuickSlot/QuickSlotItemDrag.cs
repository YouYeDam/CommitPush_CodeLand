using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotItemDrag : MonoBehaviour
{
    static public QuickSlotItemDrag Instance;
    public ItemQuickSlot DragItemQuickSlot;
    public RectTransform MyRectTransform;
    public Canvas UIManager;

    [SerializeField] private Image ItemImage;

    void Awake()
    {
        MyRectTransform = GetComponent<RectTransform>();
        Instance = this;
    }

    public void DragSetImage(Image ItemImage)
    {
        this.ItemImage.sprite = ItemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float Alpha)
    {
        Color Color = ItemImage.color;
        Color.a = Alpha;
        ItemImage.color = Color;
    }
}
