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

    public void DragSetImage(Image SkillImage) { // 스킬 드래그 시 이미지 띄우기
        this.SkillImage.sprite = SkillImage.sprite;
        SetColor(1);
    }

    public void SetColor(float Alpha) { // 알파값 1로 조정
        Color Color = SkillImage.color;
        Color.a = Alpha;
        SkillImage.color = Color;
    }
}
