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
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;

    private int currentQuestionIndex = 0;

    private void Awake(){
        // Initialiserer knapper
        for (int i = 0; i < answerButtons.Length; i++){
            int index = i; // Sådan alle knapper ikke bruger det højeste i

            // Tilføjer en klik funktion til hver knap
            answerButtons[i].onClick.AddListener(() =>{
                if (OnAnswerSelected(index)){ // Hvis man svarer korrekt
                    Debug.Log("Correct");

                    currentQuestionIndex++;

                    if (currentQuestionIndex < quizData.questions.Count){ // Hvis der er flere spørgsmål
                        DisplayCurrentQuestion();
                    } else { // Hvis der ikke er flere spørgsmål
                        GameManager.Instance.CompleteLevel();
                    }
                } else { // Hvis man svarer forkert
                    Debug.Log("Wrong");
                    GameManager.Instance.stars--;
                    starsText.text = "Stars: " + GameManager.Instance.stars;

                    if (GameManager.Instance.stars <= 0){ // Hvis man løber tør for stjerner (liv)
                        GameManager.Instance.FailLevel();
                    }
                }
            });
        }
    }

    private void Start(){
        // Subscriber til events
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitLevel += GameManager_OnExitLevel;

        // Slår denne UI fra til at starte med
        Hide();
    }

    private void OnDestroy(){
        // Unsubscriber til events når GameObjectet bliver ødelagt (Sker aldrig, medmindre vi skifter scene/selv sletter dette GameObject)
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitLevel -= GameManager_OnExitLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        // Når vi går fra GameScene til Quiz scenen

        // Viser denne side når vi vælger et level
        Show();

        // Nulstiller variabler
        currentQuestionIndex = 0;
        GameManager.Instance.stars = 3;
        starsText.text = "Stars: " + GameManager.Instance.stars;

        // Viser første spørgsmål da currentQuestionIndex er 0
        DisplayCurrentQuestion();
    }

    private void GameManager_OnExitLevel(object sender, EventArgs e){
        // Slår denne side fra når vi vinder/taber
        Hide();
    }

    private void DisplayCurrentQuestion(){
        // Opdatere tekst + video
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

    private bool OnAnswerSelected(int selectedIndex){ // Return funktion til at finde det rigtige svar
        if (currentQuestionIndex >= quizData.questions.Count){
            Debug.LogWarning("No more questions available.");
            return false;
        }

        var question = quizData.questions[currentQuestionIndex];
        return selectedIndex == (question.correctAnswerIndex - 1);
    }

    private void Hide(){
        // Slår siden fra
        gameObject.SetActive(false);
    }

    private void Show(){
        // Slår siden til
        gameObject.SetActive(true);
    }
}
