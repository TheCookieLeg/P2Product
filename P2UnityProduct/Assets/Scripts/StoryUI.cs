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
    [SerializeField] private TextMeshProUGUI vælgBilledText;
    [SerializeField] private Button backButton;
    [SerializeField] private Transform answersParent;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] answerImages;

    private StoryLevelSO.Question currentQuestion;
    private int currentQuestionIndex = 0;
    private StoryLevelSO storyData;
    private Animator anim;

    private int currentImageIndex = 0;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });

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

        currentImageIndex = 0;
        vælgBilledText.text = "Vælg billed " + (currentImageIndex + 1);

        storyData = GameManager.Instance.currentLevelStoryData;
        currentQuestionIndex = 0;

        GameManager.Instance.stars = 3;
        starsText.text = GameManager.Instance.stars.ToString();

        Show();
        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index){
        currentQuestion = storyData.questions[index];

        for (int i = 0; i < answerImages.Length; i++){
            answerImages[i].sprite = currentQuestion.answers[i];
            buttons[i].interactable = true;
            buttons[i].GetComponentInChildren<Image>().color = Color.white;
        }

        List<int> randomIndices = Enumerable.Range(0, buttons.Length).OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < buttons.Length; i++){
            buttons[randomIndices[i]].transform.SetSiblingIndex(i);
        }
    }

    private void OnButtonClicked(int selectedIndex){
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
        buttons[selectedIndex].interactable = false;

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.green;
        Color endColor = Color.gray;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponent.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponent.color = endColor;

        currentImageIndex++;
        vælgBilledText.text = "Vælg billed " + (currentImageIndex + 1);

        if (currentImageIndex >= currentQuestion.answers.Length){
            if (currentQuestionIndex < storyData.questions.Count - 1){
                currentQuestionIndex++;
                currentImageIndex = 0;
                vælgBilledText.text = "Vælg billed " + (currentImageIndex + 1);
                LoadQuestion(currentQuestionIndex);
            } else {
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            }
        }
    }

    private bool IsAnswerCorrect(int selectedIndex){
        return selectedIndex == currentImageIndex;
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
