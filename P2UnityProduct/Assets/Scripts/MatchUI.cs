using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Animator starAnim;
    [SerializeField] private Button backButton;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TextMeshProUGUI[] answerTexts;
    [SerializeField] private Image[] answerImages;

    private MatchLevelSO.Question currentQuestion;
    private int currentQuestionIndex = 0;
    private MatchLevelSO matchData;

    private int firstSelectedButtonIndex = -1;
    private Dictionary<int, int> correctMatches = new Dictionary<int, int>();
    private int matchCount = 0;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        for (int i = 0; i < buttons.Length; i++){
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
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
        if (GameManager.Instance.currentLevelData is not MatchLevelSO) return;

        matchData = GameManager.Instance.currentLevelData as MatchLevelSO;
        currentQuestionIndex = 0;

        GameManager.Instance.stars = 3;
        starsText.text = GameManager.Instance.stars.ToString();

        Show();
        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index){
        matchCount = 0;
        firstSelectedButtonIndex = -1;
        correctMatches.Clear();

        currentQuestion = matchData.questions[index];

        for (int i = 0; i < answerTexts.Length; i++){
            if (currentQuestion.answerTexts[i] != ""){
                answerTexts[i].gameObject.SetActive(true);
                answerTexts[i].text = currentQuestion.answerTexts[i];
                answerImages[i].gameObject.SetActive(false);
            } else if (currentQuestion.answerImages[i] != null){
                answerImages[i].gameObject.SetActive(true);
                answerImages[i].sprite = currentQuestion.answerImages[i];
                answerTexts[i].gameObject.SetActive(false);
            } else {
                Debug.LogError("Question not set-up");
                return;
            }
            buttons[i].interactable = true;
            buttons[i].GetComponentInChildren<Image>().color = Color.white;
        }

        foreach (var pair in currentQuestion.matchPairs){
            correctMatches[pair.leftIndex] = pair.rightIndex;
            correctMatches[pair.rightIndex] = pair.leftIndex;
        }
    }

    private void OnButtonClicked(int index){
        if (GameManager.Instance.canClickTimer > 0) return;

        if (firstSelectedButtonIndex == -1){
            firstSelectedButtonIndex = index;
            buttons[index].GetComponent<Animator>().SetBool("Active", true);
        } else if (firstSelectedButtonIndex == index){
            firstSelectedButtonIndex = -1;
            buttons[index].GetComponent<Animator>().SetBool("Active", false);
            buttons[index].GetComponent<Animator>().ResetTrigger("ButtonDown");
        } else {
            buttons[index].GetComponent<Animator>().SetBool("Active", true);

            int matchA = firstSelectedButtonIndex;
            int matchB = index;

            bool isCorrect = (correctMatches.ContainsKey(matchA) && correctMatches[matchA] == matchB) || (correctMatches.ContainsKey(matchB) && correctMatches[matchB] == matchA);

            if (isCorrect){
                StartCoroutine(RightAnswer(matchA, matchB));
            } else {
                StartCoroutine(WrongAnswer(matchA, matchB));
            }

            firstSelectedButtonIndex = -1;
        }
    }

    private IEnumerator WrongAnswer(int matchA, int matchB){
        GameManager.Instance.stars--;
        starsText.text = GameManager.Instance.stars.ToString();
        starAnim.SetTrigger("Pulse");

        Image imageComponentA = buttons[matchA].GetComponentInChildren<Image>();
        Image imageComponentB = buttons[matchB].GetComponentInChildren<Image>();

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.red;
        Color endColor = Color.white;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponentA.color = Color.Lerp(startColor, endColor, t);
            imageComponentB.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponentA.color = endColor;
        imageComponentB.color = endColor;

        buttons[matchA].GetComponent<Animator>().SetBool("Active", false);
        buttons[matchA].GetComponent<Animator>().ResetTrigger("ButtonDown");
        buttons[matchB].GetComponent<Animator>().SetBool("Active", false);
        buttons[matchB].GetComponent<Animator>().ResetTrigger("ButtonDown");

        if (GameManager.Instance.stars <= 0){
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        }
    }

    private IEnumerator RightAnswer(int matchA, int matchB){
        Image imageComponentA = buttons[matchA].GetComponentInChildren<Image>();
        Image imageComponentB = buttons[matchB].GetComponentInChildren<Image>();

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.green;
        Color endColor = Color.gray;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponentA.color = Color.Lerp(startColor, endColor, t);
            imageComponentB.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponentA.color = endColor;
        imageComponentB.color = endColor;

        buttons[matchA].GetComponent<Animator>().SetBool("Active", false);
        buttons[matchA].GetComponent<Animator>().ResetTrigger("ButtonDown");
        buttons[matchB].GetComponent<Animator>().SetBool("Active", false);
        buttons[matchB].GetComponent<Animator>().ResetTrigger("ButtonDown");

        buttons[matchA].interactable = false;
        buttons[matchB].interactable = false;

        matchCount++;
        if (matchCount >= currentQuestion.matchPairs.Count){
            currentQuestionIndex++;
            if (currentQuestionIndex < matchData.questions.Count){
                LoadQuestion(currentQuestionIndex);
            } else {
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            }
        }
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
