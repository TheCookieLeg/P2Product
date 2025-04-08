using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchUI : MonoBehaviour {

    [SerializeField] private MatchLevelSO matchData;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;

    private int firstSelectedButtonIndex = -1;
    private Dictionary<int, int> correctMatches = new Dictionary<int, int>();
    private int matchCount = 0;
    private int currentQuestionIndex = 0;
    private MatchLevelSO.MatchQuestion currentQuestion;

    private void Awake(){
        for (int i = 0; i < buttons.Length; i++){
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            Hide();
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        if (GameManager.Instance.currentLevelMatchData == null) return;

        matchData = GameManager.Instance.currentLevelMatchData;

        Show();

        currentQuestionIndex = 0;
        GameManager.Instance.stars = 3;
        starsText.text = "Stars: " + GameManager.Instance.stars;
        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index){
        matchCount = 0;
        firstSelectedButtonIndex = -1;
        correctMatches.Clear();

        currentQuestion = matchData.questions[index];

        for (int i = 0; i < answerTexts.Length; i++){
            answerTexts[i].text = currentQuestion.answers[i];
            buttons[i].interactable = true;
            buttons[i].GetComponent<Image>().color = Color.white;
        }

        foreach (var pair in currentQuestion.matchPairs){
            correctMatches[pair.leftIndex] = pair.rightIndex;
            correctMatches[pair.rightIndex] = pair.leftIndex;
        }
    }

    private void OnButtonClicked(int index){
        if (firstSelectedButtonIndex == -1){
            firstSelectedButtonIndex = index;
            buttons[index].GetComponent<Image>().color = Color.green;
        } else if (firstSelectedButtonIndex == index){
            firstSelectedButtonIndex = -1;
            buttons[index].GetComponent<Image>().color = Color.white;
        } else {
            int matchA = firstSelectedButtonIndex;
            int matchB = index;

            bool isCorrect = (correctMatches.ContainsKey(matchA) && correctMatches[matchA] == matchB) || (correctMatches.ContainsKey(matchB) && correctMatches[matchB] == matchA);

            if (isCorrect){
                buttons[matchA].interactable = false;
                buttons[matchB].interactable = false;
                buttons[matchA].GetComponent<Image>().color = Color.black;
                buttons[matchB].GetComponent<Image>().color = Color.black;

                matchCount++;
                if (matchCount >= currentQuestion.matchPairs.Count){
                    currentQuestionIndex++;
                    if (currentQuestionIndex < matchData.questions.Count){
                        LoadQuestion(currentQuestionIndex);
                    } else {
                        GameManager.Instance.CompleteLevel();
                        Hide();
                    }
                }
            } else {
                buttons[matchA].GetComponent<Image>().color = Color.white;
                buttons[matchB].GetComponent<Image>().color = Color.white;

                GameManager.Instance.stars--;
                starsText.text = "Stars: " + GameManager.Instance.stars;
                if (GameManager.Instance.stars <= 0){
                    GameManager.Instance.BackToGameScene();
                    Hide();
                }
            }

            firstSelectedButtonIndex = -1;
        }
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
