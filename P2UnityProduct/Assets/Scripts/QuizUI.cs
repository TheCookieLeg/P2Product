using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class QuizUI : MonoBehaviour {

    [Header("Quiz Data")]
    [SerializeField] private QuizLevelSO quizData;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject video;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;

    private int currentQuestionIndex = 0;

    private void Awake(){
        for (int i = 0; i < answerButtons.Length; i++){
            int index = i;

            answerButtons[i].onClick.AddListener(() =>{
                if (OnAnswerSelected(index)){
                    currentQuestionIndex++;

                    if (currentQuestionIndex < quizData.questions.Count){
                        DisplayCurrentQuestion();
                    } else {
                        GameManager.Instance.CompleteLevel();
                    }
                } else {
                    GameManager.Instance.stars--;
                    starsText.text = "Stars: " + GameManager.Instance.stars;

                    if (GameManager.Instance.stars <= 0){
                        GameManager.Instance.FailLevel();
                    }
                }
            });

            backButton.onClick.AddListener(() => {
                GameManager.Instance.FailLevel();
            });
        }
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitLevel += GameManager_OnExitLevel;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitLevel -= GameManager_OnExitLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        if (GameManager.Instance.currentLevelQuizData == null) return;

        quizData = GameManager.Instance.currentLevelQuizData;

        Show();

        currentQuestionIndex = 0;
        GameManager.Instance.stars = 3;
        starsText.text = "Stars: " + GameManager.Instance.stars;
        DisplayCurrentQuestion();
    }

    private void GameManager_OnExitLevel(object sender, EventArgs e){
        Hide();
    }

    private void DisplayCurrentQuestion(){
        var question = quizData.questions[currentQuestionIndex];

        questionText.text = question.questionText;
        videoPlayer.clip = question.videoClip;

        for (int i = 0; i < answerTexts.Length; i++){
            answerTexts[i].text = question.answers[i];
        }

        if (quizData.questions[currentQuestionIndex].videoClip != null){
            video.gameObject.SetActive(true);
            videoPlayer.Play();
        } else {
            video.gameObject.SetActive(false);
            videoPlayer.Stop();
        }
    }

    private bool OnAnswerSelected(int selectedIndex){
        if (currentQuestionIndex >= quizData.questions.Count){
            Debug.LogWarning("No more questions available.");
            return false;
        }

        var question = quizData.questions[currentQuestionIndex];
        return selectedIndex == (question.correctAnswerIndex - 1);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
