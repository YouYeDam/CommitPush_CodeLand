using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public Sprite NewSprite; // 변경할 플레이어 스프라이트를 에디터에서 할당
    public SpriteRenderer[] RenderersToActivate; // 활성화할 스프라이트 렌더러 배열
    public SpriteRenderer[] RenderersToDeactivate; // 비활성화할 스프라이트 렌더러 배열
    public RuntimeAnimatorController NewAnimatorController; // 변경할 애니메이터 컨트롤러
    public GameObject UICanvas;
    Character Character;
    void Start() {
        Character = UICanvas.GetComponent<Character>();
    }
    public void SpriteChange()
    {
        // 태그로 대상 오브젝트를 찾음
        GameObject TargetObject = GameObject.FindGameObjectWithTag("Player");
        if (TargetObject != null)
        {
            SpriteRenderer SpriteRenderer = TargetObject.GetComponent<SpriteRenderer>();
            if (SpriteRenderer != null)
            {
                SpriteRenderer.sprite = NewSprite; // 스프라이트 변경
            }
        }
    }

    public void SwitchSprites()
    {
        // 활성화할 스프라이트 렌더러를 활성화
        foreach (SpriteRenderer Renderer in RenderersToActivate)
        {
            Renderer.enabled = true;
        }

        // 비활성화할 스프라이트 렌더러를 비활성화
        foreach (SpriteRenderer Renderer in RenderersToDeactivate)
        {
            Renderer.enabled = false;
        }
    }
    
    public void ChangeAnimatorController()
    {
        // 태그로 대상 오브젝트를 찾음
        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");
        if (targetObject != null)
        {
            Animator Animator = targetObject.GetComponent<Animator>();
            if (Animator != null)
            {
                Animator.runtimeAnimatorController = NewAnimatorController; // 애니메이터 컨트롤러 변경
                Character.SetPortrait();
            }
        }
    }
}
