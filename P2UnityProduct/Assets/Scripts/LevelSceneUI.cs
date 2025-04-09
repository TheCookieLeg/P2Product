using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSceneUI : MonoBehaviour {

    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button hoverBackButton;
    [SerializeField] private TextMeshProUGUI hoverLevelText;
    [SerializeField] private GameObject hoverUI;
    [SerializeField] private TextMeshProUGUI threadText;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private Transform[] hoverLevelStars;

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene += GameManager_OnExitToGameScene;
        GameManager.Instance.OnHoverLevel += GameManager_OnHoverLevel;
        GameManager.Instance.OnEnterThread += GameManager_OnEnterThread;

        startButton.onClick.AddListener(() => {
            GameManager.Instance.EnterLevel();
        });
        backButton.onClick.AddListener(() => {
            GameManager.Instance.LeaveLevel();
            Hide();
        });
        hoverBackButton.onClick.AddListener(() => {
            hoverUI.SetActive(false);
        });

        Hide();
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

    private void GameManager_OnEnterThread(object sender, EventArgs e){
        Show();
        UpdateUI();
    }

    private void UpdateUI(){
        switch (GameManager.Instance.threadID){
            case 0:
                threadText.text = "HÅNDSY";
                break;
            case 1:
                threadText.text = "SYMASKINE";
                break;
            case 2:
                threadText.text = "LAPPER";
                break;
        }

        foreach (GameObject level in levels){
            level.SetActive(false);
        }
        levels[GameManager.Instance.threadID].SetActive(true);
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
