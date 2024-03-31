using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerSelectButtonActivate : MonoBehaviour
{
    public TMP_InputField InputField; // 입력 필드
    public SpriteRenderer[] Sprites; // 스프라이트 배열
    public Button Button; // 버튼
    public Color ActivateColor; // 활성화된 상태의 색상
    public Color InactivateColor; // 비활성화된 상태의 색상

    void Update()
    {
        // 입력 필드에 한 글자 이상이 입력되었을 때
        if (InputField.text.Length > 0)
        {
            // 버튼 활성화 여부를 스프라이트 상태에 따라 결정
            if (Sprites[0].enabled || Sprites[1].enabled || Sprites[2].enabled)
            {
                Button.interactable = true; // 버튼 활성화
                Button.image.color = ActivateColor; // 활성화된 상태의 색상으로 변경
            }
            else
            {
                Button.interactable = false; // 버튼 비활성화
                Button.image.color = InactivateColor; // 비활성화된 상태의 색상으로 변경
            }
        }
        else
        {
            Button.interactable = false; // 입력이 없으면 버튼 비활성화
            Button.image.color = InactivateColor; // 비활성화된 상태의 색상으로 변경
        }
    }
}
