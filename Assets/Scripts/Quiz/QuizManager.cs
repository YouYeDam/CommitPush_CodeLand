using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI QuizText;
    public TextMeshProUGUI ButtonText1;
    public TextMeshProUGUI ButtonText2;
    public TextMeshProUGUI ButtonText3;
    public TextMeshProUGUI ButtonText4;
    public TextMeshProUGUI ScoreText;
    public GameObject resultPanel; // 정답 결과를 표시할 패널 
    public TextMeshProUGUI resultText; // 결과 패널에 표시할 텍스트
    public Button skipButton; // 스킵 버튼
    public Button[] answerButtons; // 답변 버튼 배열

    // 퀴즈 데이터 저장을 위한 리스트
    List<string> QuizTextArray = new List<string>(); // 퀴즈 문제 텍스트 배열
    List<string> AnswerTextArray = new List<string>(); // 각 문제의 선택지 배열
    List<int> CorrectAnswers = new List<int>(); // 각 문제의 정답 인덱스 배열
    string[] answer;
    int currentQuestionIndex = 0; // 현재 표시되는 문제의 인덱스
    int totalScore = 0; // 사용자의 총 점수
    int currentScore = 100; // 현재 문제에 대한 점수
    [SerializeField] private float delay = 0.05f; // 텍스트 표시 지연 시간
    private bool skipText = false; // 텍스트 스킵 여부 플래그
    const int penalty = 20; // 오답 선택 시 감점 점수

    void Start()
    {
        skipButton.onClick.AddListener(SkipText); 
        resultPanel.SetActive(false); 
        InitializeQuiz(); 
        DisplayQuestion(currentQuestionIndex); 
    }

    // 퀴즈 데이터 초기화
    void InitializeQuiz()
    {
        QuizTextArray.Add("다음 중 비선형 자료 구조에 해당하는 것은?");
        AnswerTextArray.Add("큐, 그래프, 데크, 스택");
        CorrectAnswers.Add(1);

        QuizTextArray.Add("웹브라우저에서 뒤로가기와 같은 기능을 구현할 때 사용하기 가장 적합한 자료구조는?");
        AnswerTextArray.Add("Hash, Stack, Queue, Tree");
        CorrectAnswers.Add(1);
    }

    // 화면에 지정된 문제 표시
    void DisplayQuestion(int questionIndex)
    {
        currentScore = 100; 
        UpdateScoreText(); 
        SetAnswerButtonsActive(false); 

        string questionText = QuizTextArray[questionIndex];
        StartCoroutine(ShowTextOneByOne(questionText, QuizText, delay));
    }

    // 문제 텍스트를 한 글자씩 표시
    IEnumerator ShowTextOneByOne(string fullText, TextMeshProUGUI textComponent, float delay)
    {
        textComponent.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            if (skipText)
            {
                textComponent.text = fullText;
                break;
            }
            textComponent.text += letter;
            yield return new WaitForSeconds(delay);
        }
        skipText = false;
        
        AssignAnswerTexts(currentQuestionIndex);
        SetAnswerButtonsActive(true); 
    }

    // 각 선택지 버튼에 텍스트 할당
    void AssignAnswerTexts(int questionIndex)
    {
        string[] answers = AnswerTextArray[questionIndex].Split(',');
        ButtonText1.text = answers[0].Trim();
        ButtonText2.text = answers[1].Trim();
        ButtonText3.text = answers[2].Trim();
        ButtonText4.text = answers[3].Trim();
    }

    // 선택지 버튼의 활성/비활성 상태 설정
    void SetAnswerButtonsActive(bool isActive)
    {
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    // 선택지 버튼이 클릭되었을 때
    public void AnswerButtonClicked(int buttonIndex)
    {
        resultPanel.SetActive(true); 

        if (buttonIndex == CorrectAnswers[currentQuestionIndex])
        {
            if (currentQuestionIndex >= QuizTextArray.Count - 1) // 마지막 문제인지 확인
            {
                resultText.text = "200 OK \n\n" + currentScore + "원을 획득하셨습니다!";
                StartCoroutine(LoadNewSceneAfterDelay()); 
            }
            else
            {
                // 마지막 문제가 아닐 경우 다음 문제로 진행
                resultText.text = "200 OK \n\n" + currentScore + "원을 획득하셨습니다!";
                totalScore += currentScore;
                currentQuestionIndex++;
                DisplayQuestion(currentQuestionIndex);
            }
        }
        else
        {
            // 오답 처리
            resultText.text = "404 NOT FOUND \n\n 오답입니다.";
            currentScore = Mathf.Max(0, currentScore - penalty);
            UpdateScoreText(); 
        }

        StartCoroutine(HideResultPanel());
    }

    // 새 씬 로드
    IEnumerator LoadNewSceneAfterDelay()
    {
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene("New Scene"); 
    }

    // 결과 패널을 일정 시간 후 숨기는 코루틴
    IEnumerator HideResultPanel()
    {
        yield return new WaitForSeconds(1); 
        resultPanel.SetActive(false); 
    }

    // 문제 텍스트 효과 스킵
    public void SkipText()
    {
        skipText = true;
    }

    // 화면에 점수 표시
    void UpdateScoreText()
    {
        ScoreText.text = currentScore.ToString();
    }

    void Update()
    {

    }
}