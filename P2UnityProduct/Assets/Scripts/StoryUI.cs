using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class StoryUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Transform answersParent;
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
        starsText.text = "Stars: " + GameManager.Instance.stars;

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
            buttons[i].GetComponent<Image>().color = Color.white;
        }

        firstSelectedIndex = -1;
    }

    private void OnButtonClicked(int index){
        if (firstSelectedIndex == -1){
            firstSelectedIndex = index;
            buttons[index].GetComponent<Image>().color = Color.green;
        } else if (firstSelectedIndex == index){
            buttons[index].GetComponent<Image>().color = Color.white;
            firstSelectedIndex = -1;
        } else {
            Transform first = buttons[firstSelectedIndex].transform;
            Transform second = buttons[index].transform;

            int firstSibling = first.GetSiblingIndex();
            int secondSibling = second.GetSiblingIndex();

            first.SetSiblingIndex(secondSibling);
            second.SetSiblingIndex(firstSibling);

            buttons[firstSelectedIndex].GetComponent<Image>().color = Color.white;
            firstSelectedIndex = -1;
        }
    }

    private void OnConfirmClicked(){
        bool isCorrect = true;

        for (int i = 0; i < buttons.Length; i++){
            int buttonIndex = buttons[i].transform.GetSiblingIndex();

            if (answerImages[buttonIndex].sprite != currentQuestion.answers[i]){
                Debug.Log("Answer " + (i + 1) + " is wrong");
                isCorrect = false;
            } else {
                Debug.Log("Answer " + (i + 1) + " is correct");
                buttons[i].interactable = false;
                buttons[i].GetComponent<Image>().color = Color.black;
            }
        }

        if (isCorrect){
            Debug.Log("Correct order!");
            currentQuestionIndex++;

            if (currentQuestionIndex < storyData.questions.Count){
                LoadQuestion(currentQuestionIndex);
            } else {
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            }
        } else {
            Debug.Log("Incorrect order!");
            GameManager.Instance.stars--;
            starsText.text = "Stars: " + GameManager.Instance.stars;

            if (GameManager.Instance.stars <= 0){
                GameManager.Instance.BackToGameScene();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
                return;
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
