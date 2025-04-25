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

            if (score + 2 >= scorers.Length){
                scoreText.text = scorers.Length + "/" + scorers.Length;
                scoreText.GetComponent<Animator>().SetTrigger("Pulse");
                gameStarted = false;
                GameManager.Instance.CompleteLevel();
                anim.SetTrigger("End");
                Invoke("Hide", 0.5f);
            } else {
                score += 2;
                scoreText.text = score + "/" + scorers.Length;
                scoreText.GetComponent<Animator>().SetTrigger("Pulse");
                scorers[score - 2].SetActive(false);
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
        anim.SetTrigger("Needle Start");

        for (int i = 0; i < fabrics.Length; i++){
            Vector3 position = new Vector3((i * 2) - 2, 0, 0);
            fabrics[i].transform.position = position;
        }

        for (int i = 0; i < scorers.Length; i++){
            float positionX = 0;

            if (i % 2 == 0){
                positionX = i - 1;
            } else {
                positionX = i + 2;
            }

            float offset = 0;
            Vector3 position = new Vector3(positionX + offset, 0, 0);
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
