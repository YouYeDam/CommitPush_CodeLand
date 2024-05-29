using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillQuickSlotSkillDrag : MonoBehaviour
{
    static public SkillQuickSlotSkillDrag Instance;
    public SkillQuickSlot DragSkillQuickSlot;
    public RectTransform MyRectTransform;
    public Canvas UIManager;

    [SerializeField] private Image SkillImage;

    void Awake()
    {
        MyRectTransform = GetComponent<RectTransform>();
        Instance = this;
    }

    public void DragSetImage(Image SkillImage)
    {
        this.SkillImage.sprite = SkillImage.sprite;
        SetColor(1);
    }

    public void SetColor(float Alpha)
    {
        Color Color = SkillImage.color;
        Color.a = Alpha;
        SkillImage.color = Color;
    }
}
