using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class StoryUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Animator starAnim;
    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Transform answersParent;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] answerImages;

    private StoryLevelSO.Question currentQuestion;
    private int currentQuestionIndex = 0;
    private StoryLevelSO storyData;
    private Animator anim;

    private int firstSelectedIndex = -1;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });

        confirmButton.onClick.AddListener(() => OnConfirmClicked());

        for (int i = 0; i < buttons.Length; i++){
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
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
        if (GameManager.Instance.currentLevelStoryData == null) return;

        storyData = GameManager.Instance.currentLevelStoryData;
        currentQuestionIndex = 0;

        GameManager.Instance.stars = 3;
        starsText.text = GameManager.Instance.stars.ToString();

        Show();
        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index){
        currentQuestion = storyData.questions[index];

        List<int> randomIndices = Enumerable.Range(0, buttons.Length).OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < buttons.Length; i++){
            buttons[randomIndices[i]].transform.SetSiblingIndex(i);
        }

        for (int i = 0; i < answerImages.Length; i++){
            answerImages[i].sprite = currentQuestion.answers[i]; // Keep correct order here
            answerImages[i].gameObject.name = i.ToString(); // Restore original index name if needed
            buttons[i].interactable = true;
            buttons[i].GetComponentInChildren<Image>().color = Color.white;
        }

        firstSelectedIndex = -1;
    }

    private void OnButtonClicked(int index){
        if (GameManager.Instance.canClickTimer > 0) return;

        if (firstSelectedIndex == -1){
            firstSelectedIndex = index;
            buttons[index].GetComponent<Animator>().SetBool("Active", true);
        } else if (firstSelectedIndex == index){
            firstSelectedIndex = -1;
            buttons[index].GetComponent<Animator>().SetBool("Active", false);
            buttons[index].GetComponent<Animator>().ResetTrigger("ButtonDown");
        } else {
            Transform first = buttons[firstSelectedIndex].transform;
            Transform second = buttons[index].transform;

            int firstSibling = first.GetSiblingIndex();
            int secondSibling = second.GetSiblingIndex();

            buttons[firstSelectedIndex].GetComponent<Animator>().SetBool("Active", false);
            buttons[firstSelectedIndex].GetComponent<Animator>().ResetTrigger("ButtonDown");
            buttons[index].GetComponent<Animator>().SetBool("Active", false);
            buttons[index].GetComponent<Animator>().ResetTrigger("ButtonDown");

            first.SetSiblingIndex(secondSibling);
            second.SetSiblingIndex(firstSibling);

            Invoke("FixBug", 0.08f); // Fix spacing bug - så vi resetter spacing her og det fikser buggen indtil videre

            buttons[firstSelectedIndex].GetComponentInChildren<Image>().color = Color.white;
            firstSelectedIndex = -1;
        }
    }

    private void FixBug(){
        verticalLayoutGroup.spacing = 20.1f;
        verticalLayoutGroup.spacing = 20;
    }

    private void OnConfirmClicked(){
        if (GameManager.Instance.canClickTimer > 0) return;

        bool isCorrect = true;

        List<int> rightAnswers = new List<int>(6);
        for (int i = 0; i < buttons.Length; i++){
            int buttonIndex = buttons[i].transform.GetSiblingIndex();

            if (answerImages[buttonIndex].sprite != currentQuestion.answers[i]){
                isCorrect = false;
            } else {
                rightAnswers.Add(i);
            }
        }

        GameManager.Instance.canClickTimer = 0.5f;

        StartCoroutine(Answer(rightAnswers, isCorrect));
    }

    private IEnumerator Answer(List<int> rightAnswers, bool isCorrect){
        if (!isCorrect){
            GameManager.Instance.stars--;
            starsText.text = GameManager.Instance.stars.ToString();
            starAnim.SetTrigger("Pulse");
        }

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color correctStartColor = Color.green;
        Color wrongStartColor = Color.red;
        Color correctEndColor = Color.gray;
        Color wrongEndColor = Color.white;

        for (int i = 0; i < buttons.Length; i++){
            if (rightAnswers.Contains(i)){
                buttons[i].GetComponentInChildren<Image>().color = correctEndColor;
            } else {
                buttons[i].GetComponentInChildren<Image>().color = wrongStartColor;
            }
        }

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            for (int i = 0; i < buttons.Length; i++){
                if (rightAnswers.Contains(i)){
                    buttons[i].GetComponentInChildren<Image>().color = Color.Lerp(correctStartColor, correctEndColor, t);
                } else {
                    buttons[i].GetComponentInChildren<Image>().color = Color.Lerp(wrongStartColor, wrongEndColor, t);
                }
            }

            yield return null;
        }

        for (int i = 0; i < buttons.Length; i++){
            if (rightAnswers.Contains(i)){
                buttons[i].GetComponentInChildren<Image>().color = correctEndColor;
                buttons[i].interactable = false;
            } else {
                buttons[i].GetComponentInChildren<Image>().color = wrongEndColor;
            }
        }

        if (isCorrect){
            currentQuestionIndex++;
            if (currentQuestionIndex < storyData.questions.Count){
                LoadQuestion(currentQuestionIndex);
            } else {
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            }
        } else {
            if (GameManager.Instance.stars <= 0){
                GameManager.Instance.BackToGameScene();
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
