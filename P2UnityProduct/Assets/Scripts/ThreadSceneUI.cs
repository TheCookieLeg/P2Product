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
            GameManager.Instance.EnterThread(0);
        });
        UpdateCompletion();
    }

    private void Start(){
        GameManager.Instance.OnLeaveLevel += GameManager_OnLeaveLevel;
        GameManager.Instance.OnLeaveThread += GameManager_OnLeaveThread;
    }

    private void OnDestroy(){
        GameManager.Instance.OnLeaveLevel -= GameManager_OnLeaveLevel;
        GameManager.Instance.OnLeaveThread -= GameManager_OnLeaveThread;
    }

  private void GameManager_OnLeaveLevel(object sender, EventArgs e){
        UpdateCompletion();
        Show();
    }

    private void GameManager_OnLeaveThread(object sender, EventArgs e){
        Hide();
    }

    private void UpdateCompletion(){
        int håndsyTotalStars = 0;

        int amountOfLevels = 8;

        float totalAmountOfStars = amountOfLevels * 3;

        for (int i = 1; i <= amountOfLevels; i++){
            håndsyTotalStars += PlayerPrefs.GetInt("HåndsyLevel" + i + "Stars", 0);
        }

        håndsyText.text = (håndsyTotalStars / totalAmountOfStars * 100).ToString("0") + "%";
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
