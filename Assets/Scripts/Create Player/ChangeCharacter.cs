using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public Sprite PlayerSprite; // 변경할 플레이어 스프라이트를 에디터에서 할당
    public SpriteRenderer[] ActiveRenderers; // 활성화할 스프라이트 렌더러 배열
    public SpriteRenderer[] InactiveRenderers; // 비활성화할 스프라이트 렌더러 배열
    public RuntimeAnimatorController PlayerAnimatorController; // 변경할 애니메이터 컨트롤러
    public GameObject UICanvas;
    Character Character;

    void Start() {
        Character = UICanvas.GetComponent<Character>();
    }

    public void SpriteChange() { // 버튼에 할당하여 클릭 시 해당 캐릭터 스프라이트로 변경
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        if (PlayerObject != null)
        {
            SpriteRenderer PlayerSpriteRenderer = PlayerObject.GetComponent<SpriteRenderer>();
            if (PlayerSpriteRenderer != null)
            {
                PlayerSpriteRenderer.sprite = PlayerSprite;
            }
        }
    }

    public void SwitchSprites() { // 스프라이트 렌더러 활성화 및 비활성화
        foreach (SpriteRenderer Renderer in ActiveRenderers)
        {
            Renderer.enabled = true;
        }

        foreach (SpriteRenderer Renderer in InactiveRenderers)
        {
            Renderer.enabled = false;
        }
    }
    
    public void ChangeAnimatorController() { // 버튼에 할당하여 클릭 시 해당 캐릭터 애니메이터로 변경
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        if (PlayerObject != null)
        {
            Animator Animator = PlayerObject.GetComponent<Animator>();
            if (Animator != null)
            {
                Animator.runtimeAnimatorController = PlayerAnimatorController;
                Character.SetPortrait();
            }
        }
    }
}
