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

    void Start()
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
