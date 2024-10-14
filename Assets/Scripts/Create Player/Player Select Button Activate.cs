using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerSelectButtonActivate : MonoBehaviour
{
    public TMP_InputField InputField;
    public SpriteRenderer[] PlayerSelectSignSprites;
    public Button Button;
    public Color ActivateColor;
    public Color InactivateColor;

    void Update()
    {
        if (InputField.text.Length > 0){ // 이름이 한 글자 이상일 때만 버튼 활성화 되도록
            
            if (PlayerSelectSignSprites[0].enabled || PlayerSelectSignSprites[1].enabled || PlayerSelectSignSprites[2].enabled) { // 아무 스프라이트나 활성화 되었다면 버튼 활성화
                Button.interactable = true;
                Button.image.color = ActivateColor;
                
                if (Input.GetKeyDown(KeyCode.Return)) { // 엔터 키 입력 시 버튼 클릭과 동일한 효과
                    Button.onClick.Invoke();
                }
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
