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
        if (GameManager.Instance.currentLevelData is not StoryLevelSO) return;

        currentImageIndex = 0;
        vælgBilledText.text = BilledtextFraIndex(currentImageIndex);

        storyData = GameManager.Instance.currentLevelData as StoryLevelSO;
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
            Image[] imageComponents = buttons[i].GetComponentsInChildren<Image>();
            imageComponents[0].color = Color.white;
            imageComponents[1].color = Color.white;
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

        Image[] imageComponents = buttons[selectedIndex].GetComponentsInChildren<Image>();

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.red;
        Color endColor = Color.white;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponents[0].color = Color.Lerp(startColor, endColor, t);
            imageComponents[1].color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponents[0].color = endColor;
        imageComponents[1].color = endColor;

        if (GameManager.Instance.stars <= 0){
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        }
    }

    private IEnumerator RightAnswer(int selectedIndex){
        Image[] imageComponents = buttons[selectedIndex].GetComponentsInChildren<Image>();
        buttons[selectedIndex].interactable = false;

        float elapsedTime = 0f;
        float duration = 0.5f;

        Color startColor = Color.green;
        Color endColor = Color.gray;

        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            imageComponents[0].color = Color.Lerp(startColor, endColor, t);
            imageComponents[1].color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        imageComponents[0].color = endColor;
        imageComponents[1].color = endColor;

        currentImageIndex++;
        vælgBilledText.text = BilledtextFraIndex(currentImageIndex);
        
        if (currentImageIndex >= currentQuestion.answers.Length){
            if (currentQuestionIndex < storyData.questions.Count - 1){
                currentQuestionIndex++;
                currentImageIndex = 0;
                vælgBilledText.text = BilledtextFraIndex(currentImageIndex);
                LoadQuestion(currentQuestionIndex);
            } else {
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            }
        }
    }

    private string BilledtextFraIndex(int index) {
        switch (index) {
            case 0:
                return "Mål tråden op";
            case 1:
                return "Før nålen gannem stoffets bagside";
            case 2:
                return "Før nålen gennem stoffet, så det danner et 'x'";
            case 3:
                return "Placer knappen på stoffet, og før nålen gennem et hul";
            case 4:
                return "Før nålen tilbage gennem et modstående hul";
            case 5:
                return "Syg gennem alle huller flere gange, så det danner et 'x'";
            case 6:
                return "Lav en stilk ved at dreje tråden rundt under knappen et par gange";
            case 7:
                return "Før tråden på den anden side af stoffet, under knappen, og bind en knude på bagsiden";
            default:
                return "Vælg det korrekte billede";
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
