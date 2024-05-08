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
    // private TMP_Text PlaceHolder;
    // private TMP_Text ButtonText;

    // void Start(){
    //     PlaceHolder = InputField.GetComponentInChildren<TMP_Text>();
    //     ButtonText = Button.GetComponentInChildren<TMP_Text>();

    // }
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
                
                // 엔터 키 인식
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Button.onClick.Invoke();
                }
            }
            else
            {
                Button.interactable = false; // 버튼 비활성화
                Button.image.color = InactivateColor; // 비활성화된 상태의 색상으로 변경
                //비활성화 상태에서 엔터키, 마우스 클릭이 발생하면 place holder를 빨간색으로 바꿔주기
                // if (Input.GetKeyDown(KeyCode.Return) )
                // {
                //     SetColorRed();
                // }
                

            }
        }
        else
        {
            Button.interactable = false; // 입력이 없으면 버튼 비활성화
            Button.image.color = InactivateColor; // 비활성화된 상태의 색상으로 변경
            // if (Input.GetKeyDown(KeyCode.Return) )
            // {
            //     SetColorRed();

            // }
        }
    }
    // void SetColorRed()
    // {
    //     PlaceHolder.color = Color.red;
    // }
}
