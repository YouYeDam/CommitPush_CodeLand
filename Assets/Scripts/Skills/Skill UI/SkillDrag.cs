using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDrag : MonoBehaviour
{
    static public SkillDrag Instance;
    public SkillSlot SkillDragSlot;
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
