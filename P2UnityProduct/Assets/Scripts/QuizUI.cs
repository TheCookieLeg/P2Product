using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class QuizUI : MonoBehaviour {

    [Header("References")]
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject video;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;

    private QuizLevelSO.Question currentQuestion;
    private int currentQuestionIndex = 0;
    private QuizLevelSO quizData;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });

        for (int i = 0; i < buttons.Length; i++){
            int index = i;
            buttons[i].onClick.AddListener(() => OnAnswerClicked(index));
        }
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        if (GameManager.Instance.currentLevelQuizData == null) return;

        quizData = GameManager.Instance.currentLevelQuizData;
        currentQuestionIndex = 0;

        GameManager.Instance.stars = 3;
        starsText.text = "Stars: " + GameManager.Instance.stars;

        Show();
        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index){
        currentQuestion = quizData.questions[index];

        questionText.text = currentQuestion.questionText;
        videoPlayer.clip = currentQuestion.videoClip;

        for (int i = 0; i < answerTexts.Length; i++){
            answerTexts[i].text = currentQuestion.answers[i];
        }

        if (quizData.questions[index].videoClip != null){
            video.gameObject.SetActive(true);
            videoPlayer.Play();
        } else {
            video.gameObject.SetActive(false);
            videoPlayer.Stop();
        }
    }

    private void OnAnswerClicked(int selectedIndex){
        if (!IsAnswerCorrect(selectedIndex)){
            GameManager.Instance.stars--;
            starsText.text = "Stars: " + GameManager.Instance.stars;

            if (GameManager.Instance.stars <= 0){
                GameManager.Instance.BackToGameScene();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
                return;
            }

            return;
        }

        currentQuestionIndex++;

        if (currentQuestionIndex < quizData.questions.Count){
            LoadQuestion(currentQuestionIndex);
        } else {
            GameManager.Instance.CompleteLevel();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        }
    }

    private bool IsAnswerCorrect(int selectedIndex){
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
