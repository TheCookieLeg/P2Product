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
    [SerializeField] private Animator starAnim;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject video;
    [SerializeField] private GameObject videoBackground;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject imageBackground;
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
        starsText.text = GameManager.Instance.stars.ToString();

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

        for (int i = 0; i < buttons.Length; i++){
            buttons[i].interactable = true;
            buttons[i].GetComponentInChildren<Image>().color = Color.white;
        }

        if (quizData.questions[index].videoClip != null && quizData.questions[index].image == null){
            image.SetActive(false);
            imageBackground.SetActive(false);
            video.SetActive(true);
            videoBackground.SetActive(true);
            videoPlayer.Play();
        } else if (quizData.questions[index].videoClip == null && quizData.questions[index].image != null){
            image.SetActive(true);
            imageBackground.SetActive(true);
            image.GetComponent<Image>().sprite = quizData.questions[index].image;
            video.SetActive(false);
            videoBackground.SetActive(false);
            videoPlayer.Stop();
        } else {
            Debug.LogWarning("Question set-up incorrectly");
        }
    }

    private void OnAnswerClicked(int selectedIndex){
        if (GameManager.Instance.canClickTimer > 0) return;
        GameManager.Instance.canClickTimer = 0.5f;

        if (!IsAnswerCorrect(selectedIndex)){
            StartCoroutine(WrongAnswer(selectedIndex));
            return;
        } else {
            StartCoroutine(RightAnswer(selectedIndex));
        }
    }

    private IEnumerator WrongAnswer(int selectedIndex){
        GameManager.Instance.stars--;
        starsText.text = GameManager.Instance.stars.ToString();
        starAnim.SetTrigger("Pulse");

        Image imageComponent = buttons[selectedIndex].GetComponentInChildren<Image>();

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.red;
        Color endColor = Color.white;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponent.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponent.color = endColor;

        if (GameManager.Instance.stars <= 0){
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        }
    }

    private IEnumerator RightAnswer(int selectedIndex){
        Image imageComponent = buttons[selectedIndex].GetComponentInChildren<Image>();

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.green;
        Color endColor = Color.white;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponent.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponent.color = endColor;

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
