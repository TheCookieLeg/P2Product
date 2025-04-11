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
    [SerializeField] private Transform[] hoverLevelStars;

    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene += GameManager_OnExitToGameScene;
        GameManager.Instance.OnHoverLevel += GameManager_OnHoverLevel;
        GameManager.Instance.OnEnterThread += GameManager_OnEnterThread;
        GameManager.Instance.OnLeaveLevel += GameManager_OnLeaveLevel;

        startButton.onClick.AddListener(() => {
            GameManager.Instance.EnterLevel();
            anim.SetTrigger("End");
        });
        backButton.onClick.AddListener(() => {
            GameManager.Instance.LeaveLevel();
        });
        hoverBackButton.onClick.AddListener(() => {
            hoverUI.GetComponent<Animator>().SetTrigger("End");
            Invoke("HideHover", 0.5f);
        });

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene -= GameManager_OnExitToGameScene;
        GameManager.Instance.OnHoverLevel -= GameManager_OnHoverLevel;
        GameManager.Instance.OnEnterThread -= GameManager_OnEnterThread;
        GameManager.Instance.OnLeaveLevel -= GameManager_OnLeaveLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        Invoke("Hide", 0.5f);
    }

    private void GameManager_OnExitToGameScene(object sender, EventArgs e){
        hoverUI.SetActive(false);
        Show();
        anim.SetTrigger("StartFromMinigame");
    }

    private void GameManager_OnEnterThread(object sender, EventArgs e){
        hoverUI.SetActive(false);
        Show();
    }

    private void GameManager_OnLeaveLevel(object sender, EventArgs e){
        Hide();
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

    private void HideHover(){
        hoverUI.SetActive(false);
    }
}
