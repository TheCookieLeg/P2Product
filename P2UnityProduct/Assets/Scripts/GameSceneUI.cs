using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour {

    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI hoverLevelText;
    [SerializeField] private Transform[] hoverLevelStars;
    [SerializeField] private GameObject hoverUI;

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene += GameManager_OnExitToGameScene;
        GameManager.Instance.OnHoverLevel += GameManager_OnHoverLevel;

        startButton.onClick.AddListener(() => {
            GameManager.Instance.EnterLevel();
        });
        backButton.onClick.AddListener(() => {
            hoverUI.SetActive(false);
        });
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene -= GameManager_OnExitToGameScene;
        GameManager.Instance.OnHoverLevel -= GameManager_OnHoverLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        hoverUI.SetActive(false);
        Hide();
    }

    private void GameManager_OnExitToGameScene(object sender, EventArgs e){
        Show();
    }

    private void GameManager_OnHoverLevel(object sender, EventArgs e){
        hoverLevelText.text = "Level " + GameManager.Instance.hoverLevelID;
        foreach (Transform hoverLevelStar in hoverLevelStars){
            hoverLevelStar.GetChild(0).gameObject.SetActive(true);
            hoverLevelStar.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.Instance.hoverStars; i++){
            hoverLevelStars[i].GetChild(1).gameObject.SetActive(true);
        }
        hoverUI.SetActive(true);
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
