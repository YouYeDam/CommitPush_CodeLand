using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite newSprite; // 변경할 새 스프라이트를 에디터에서 할당
    [SerializeField] public string TargetTag;
    public void SpriteChange()
    {
        // 태그로 대상 오브젝트를 찾음
        GameObject targetObject = GameObject.FindGameObjectWithTag(TargetTag);
        if (targetObject != null)
        {
            SpriteRenderer spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = newSprite; // 스프라이트 변경
            }
            else
            {
                Debug.LogError("SpriteRenderer not found on the target object.");
            }
        }
        else
        {
            Debug.LogError("Target object not found with tag: " + TargetTag);
        }
    }
}
