using System.Collections;
using UnityEngine;
using TMPro;
public class DisplaySentenceDelay : MonoBehaviour
{
    public TextMeshProUGUI TextDisplay;
    [SerializeField] public float typingSpeed = 0.1f; // 글자가 출력되는 속도
    private void Start()
    {
        StartCoroutine(TypeText(TextDisplay.text));
    }

    IEnumerator TypeText(string textToType) {
        TextDisplay.text = ""; // 텍스트 출력 전 초기화
        foreach (char letter in textToType.ToCharArray())
        {
            TextDisplay.text += letter; // 한 글자씩 텍스트에 추가
            yield return new WaitForSeconds(typingSpeed); // 다음 글자 출력 전 대기
        }
    }
}
