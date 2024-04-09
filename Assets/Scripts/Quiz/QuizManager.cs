using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    // 퀴즈 데이터를 저장할 리스트
    List<string> QuizTextArray = new List<string>();
    List<string> AnswerTextArray = new List<string>();
    List<int> CorrectAnswers = new List<int>();
    string[] answer;
    int currentQuestionIndex = 0;
    int totalScore = 0;
    int currentScore = 100; // 현재 문제의 시작 점수
    [SerializeField] private float delay = 0.05f; // 문제 텍스트 표시 지연 시간
    private bool skipText = false;
    const int penalty = 20;

    void Start()
    {
        skipButton.onClick.AddListener(SkipText); // 스킵 버튼 이벤트 리스너 추가
        // 게임 시작할 때 결과 패널 비활성화
        resultPanel.SetActive(false);
        InitializeQuiz(); // 퀴즈 데이터 초기화
        // 첫번째 문제 표시
        DisplayQuestion(currentQuestionIndex);
    }
    // 퀴즈 데이터 초기화 함수
    void InitializeQuiz()
    {
        QuizTextArray.Add("다음 중 비선형 자료 구조에 해당하는 것은?");
        AnswerTextArray.Add("큐, 그래프, 데크, 스택");
        CorrectAnswers.Add(1);

        QuizTextArray.Add("웹브라우저에서 뒤로가기와 같은 기능을 구현할 때 사용하기 가장 적합한 자료구조는?");
        AnswerTextArray.Add("Hash, Stack, Queue, Tree");
        CorrectAnswers.Add(1);

    }

    // 문제 표시 함수
    void DisplayQuestion(int questionIndex)
    {
        currentScore = 100; // 새 문제에 대한 점수를 100으로 설정
        UpdateScoreText();  // 점수 텍스트 업데이트
        
        SetAnswerButtonsActive(false);  // 시작 시 보기 버튼들 비활성화

        // 문제 텍스트를 한글자씩 표시
        string questionText = QuizTextArray[questionIndex];
        StartCoroutine(ShowTextOneByOne(questionText, QuizText, delay));
    }

    // 문제 텍스트를 한글자씩 화면에 표시하는 코루틴
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
        
        AssignAnswerTexts(currentQuestionIndex); // 선택지 텍스트 할당
        SetAnswerButtonsActive(true); // 텍스트 출력 완료 후 보기 버튼들 활성화
    }

    // 각 선택지 버튼에 텍스트를 할당하는 함수
    void AssignAnswerTexts(int questionIndex)
    {
        string[] answers = AnswerTextArray[questionIndex].Split(',');
        ButtonText1.text = answers[0].Trim();
        ButtonText2.text = answers[1].Trim();
        ButtonText3.text = answers[2].Trim();
        ButtonText4.text = answers[3].Trim();
    }

    // 선택지 버튼의 활성/비활성 상태를 설정하는 함수
    void SetAnswerButtonsActive(bool isActive)
    {
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    // 선택지 버튼 클릭 시 호출되는 함수
    public void AnswerButtonClicked(int buttonIndex)
    {
        resultPanel.SetActive(true); // 결과 패널 활성화

        // 정답 확인
        if (buttonIndex == CorrectAnswers[currentQuestionIndex]) // 정답을 맞췄을 시 
        {
            resultText.text = "200 OK \n\n" + currentScore + "원을 획득하셨습니다!";

            Debug.Log("정답입니다!");
            totalScore += currentScore;
            currentQuestionIndex++;

            if (currentQuestionIndex < QuizTextArray.Count)
            {
                DisplayQuestion(currentQuestionIndex);
            }
            else
            {
                Debug.Log($"퀴즈 끝! 최종 점수: {totalScore}");
            }
        }
        else // 오답 선택 시
        {
            resultText.text = "404 NOT FOUND \n\n 오답입니다.";
            Debug.Log("오답입니다!");
            currentScore = Mathf.Max(0, currentScore - penalty);
            UpdateScoreText(); // 오답을 선택할 때마다 화면에 표시되는 점수 갱신
        }

        // 결과를 표시하고 패널을 자동으로 숨김
        StartCoroutine(HideResultPanel());
    }

    // 결과 패널을 숨기는 코루틴
    IEnumerator HideResultPanel()
    {
        yield return new WaitForSeconds(1); // 2초간 대기
        resultPanel.SetActive(false); // 패널을 비활성화
    }

    // 스킵 버튼 클릭 시 호출되는 함수
    public void SkipText()
    {
        skipText = true;
    }

    // Score를 업데이트하는 함수
    void UpdateScoreText()
    {
        ScoreText.text = currentScore.ToString(); // 현재 문제의 점수를 화면에 표시
    }

    void Update()
    {
        
    }
}


