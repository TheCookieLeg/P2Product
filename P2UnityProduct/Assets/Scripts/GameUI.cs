using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private Animator starAnim;
    [SerializeField] private Button backButton;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject fabric;
    [SerializeField] private GameObject Scorer;
    [SerializeField] private GameObject[] fabrics;
    [SerializeField] private GameObject[] scorers;
    private int score = 0;
    private bool gameStarted = false;

    private Animator anim;
    private List<float> realScorerXs;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("Needle End");
            Invoke("Hide", 0.5f);
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        Hide();
    }

    private void FixedUpdate(){
        if (!gameStarted) return;

        if (scorers[score].transform.position.x < -2.7f){
            int incrementedValue = 1;
            if (score == 2 || score == 6 || score == 10 || score == 14 || score == 18 || score == 22 || score == 26 || score == 30 || score == 34){
                incrementedValue = 4;
            }

            if (score == 3 || score == 7 || score == 11 || score == 15 || score == 19 || score == 23 || score == 27 || score == 31 || score == 35){
                incrementedValue = 5;
            }

            if (score + incrementedValue >= scorers.Length){
                scoreText.text = scorers.Length + "/" + scorers.Length;
                scoreText.GetComponent<Animator>().SetTrigger("Pulse");
                gameStarted = false;
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            } else {
                score += incrementedValue;
                scoreText.text = score + "/" + scorers.Length;
                scoreText.GetComponent<Animator>().SetTrigger("Pulse");
                scorers[score - incrementedValue].SetActive(false);
                scorers[score].SetActive(true);
            }

            GameManager.Instance.stars--;
            starsText.text = GameManager.Instance.stars.ToString();
            starAnim.SetTrigger("Pulse");

            if (GameManager.Instance.stars <= 0 && gameStarted){
                gameStarted = false;
                GameManager.Instance.BackToGameScene();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            }
        }
    }

    public void HitScorer(){
        if (score + 1 >= scorers.Length){
            scoreText.text = scorers.Length + "/" + scorers.Length;
            scoreText.GetComponent<Animator>().SetTrigger("Pulse");
            gameStarted = false;
            GameManager.Instance.CompleteLevel();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        } else {
            score++;
            scoreText.text = score + "/" + scorers.Length;
            scoreText.GetComponent<Animator>().SetTrigger("Pulse");
            scorers[score].SetActive(true);
            scorers[score - 1].SetActive(false);
        }
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        if (GameManager.Instance.currentLevelData is not GameLevelSO) return;

        score = 0;
        scoreText.text = score + "/" + scorers.Length;
        GameManager.Instance.stars = 3;
        starsText.text = GameManager.Instance.stars.ToString();

        Show();

        for (int i = 0; i < fabrics.Length; i++){
            Vector3 position = new Vector3((i * 2) - 2, 0, 0);
            fabrics[i].transform.position = position;
        }

        float startX = -1f;
        float forwardStep = 4f;  // big step forward
        float backStep = 2f;     // step back
        float ySpacing = 1.5f;     // mid scorer vertical spacing

        realScorerXs = new List<float>();

        float currentX = startX;
        bool moveForward = true;

        // First, calculate all real scorer X positions
        for (int i = 0; i < scorers.Length; i += 2){
            realScorerXs.Add(currentX);

            if (moveForward){
                currentX += forwardStep;
            } else {
                currentX -= backStep;
            }

            moveForward = !moveForward;
        }
        
        for (int i = 0; i < scorers.Length; i++){
            float positionX = 0;
            float positionY = 0;

            if (i % 2 == 0){
                // Real scorers
                positionX = realScorerXs[i / 2];
                positionY = 0;
            } else {
                // Mid scorers between real scorers
                int midIndex = (i - 1) / 2;

                if (midIndex + 1 < realScorerXs.Count){
                    float leftRealX = realScorerXs[midIndex];
                    float rightRealX = realScorerXs[midIndex + 1];

                    positionX = (leftRealX + rightRealX) / 2f;
                    positionY = (midIndex % 2 == 0) ? ySpacing : -ySpacing;
                } else {
                    positionX = realScorerXs[midIndex];
                    positionY = 0;
                }
            }

            Vector3 position = new Vector3(positionX, positionY, 0);
            scorers[i].transform.localPosition = position;
            scorers[i].SetActive(i < 1);
        }

        gameStarted = true;
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
