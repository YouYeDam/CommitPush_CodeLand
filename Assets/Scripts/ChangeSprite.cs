using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite NewSprite; // 변경할 플레이어 스프라이트를 에디터에서 할당
    [SerializeField] public string TargetTag;
    public SpriteRenderer[] RenderersToActivate; // 활성화할 스프라이트 렌더러 배열
    public SpriteRenderer[] RenderersToDeactivate; // 비활성화할 스프라이트 렌더러 배열

    public void SpriteChange()
    {
        // 태그로 대상 오브젝트를 찾음
        GameObject targetObject = GameObject.FindGameObjectWithTag(TargetTag);
        if (targetObject != null)
        {
            SpriteRenderer spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = NewSprite; // 스프라이트 변경
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
}
