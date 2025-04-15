using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThreadSceneUI : MonoBehaviour {

    [SerializeField] private Button håndsyButton;
    [SerializeField] private TextMeshProUGUI håndsyText;

    private void Awake(){
        håndsyButton.onClick.AddListener(() => {
            if (GameManager.Instance.canClickTimer > 0) return;
            GameManager.Instance.canClickTimer = 1f;
            GameManager.Instance.EnterThread();
        });
        UpdateCompletion();
    }

    private void Start(){
        GameManager.Instance.OnLeaveLevel += GameManager_OnLeaveLevel;
        GameManager.Instance.OnLeaveThread += GameManager_OnLeaveThread;
        GameManager.Instance.OnResetSave += GameManager_OnResetSave;
    }

    private void OnDestroy(){
        GameManager.Instance.OnLeaveLevel -= GameManager_OnLeaveLevel;
        GameManager.Instance.OnLeaveThread -= GameManager_OnLeaveThread;
        GameManager.Instance.OnResetSave -= GameManager_OnResetSave;
    }

    private void GameManager_OnLeaveLevel(object sender, EventArgs e){
        UpdateCompletion();
        Show();
    }

    private void GameManager_OnLeaveThread(object sender, EventArgs e){
        Hide();
    }

    private void GameManager_OnResetSave(object sender, EventArgs e){
        UpdateCompletion();
    }

    private void UpdateCompletion(){
        int totalStars = 0;

        int amountOfLevels = 8;

        float totalAmountOfStars = amountOfLevels * 3;

        for (int i = 1; i <= amountOfLevels; i++){
            totalStars += PlayerPrefs.GetInt("Level" + i + "Stars", 0);
        }

        håndsyText.text = (totalStars / totalAmountOfStars * 100).ToString("0") + "%";
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
