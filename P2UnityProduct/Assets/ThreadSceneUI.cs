using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThreadSceneUI : MonoBehaviour {

    [SerializeField] private Button håndsyButton;
    [SerializeField] private Button symaskineButton;
    [SerializeField] private Button lapperButton;
    [SerializeField] private TextMeshProUGUI håndsyText;
    [SerializeField] private TextMeshProUGUI symaskineText;
    [SerializeField] private TextMeshProUGUI lapperText;

    private void Awake(){
        håndsyButton.onClick.AddListener(() => {
            GameManager.Instance.EnterThread(0);
            Hide();
        });
        symaskineButton.onClick.AddListener(() => {
            GameManager.Instance.EnterThread(1);
            Hide();
        });
        lapperButton.onClick.AddListener(() => {
            GameManager.Instance.EnterThread(2);
            Hide();
        });
        UpdateCompletion();
    }

    private void Start(){
        GameManager.Instance.OnLeaveLevel += GameManager_OnLeaveLevel;
    }

    private void GameManager_OnLeaveLevel(object sender, EventArgs e){
        UpdateCompletion();
        Show();
    }

    private void UpdateCompletion(){
        int håndsyTotalStars = 0;
        int symaskineTotalStars = 0;
        int lapperTotalStars = 0;

        int amountOfLevels = 8;

        float totalAmountOfStars = amountOfLevels * 3;

        for (int i = 1; i <= amountOfLevels; i++){
            håndsyTotalStars += PlayerPrefs.GetInt("HåndsyLevel" + i + "Stars", 0);
        }

        for (int i = 1; i <= amountOfLevels; i++){
            symaskineTotalStars += PlayerPrefs.GetInt("SymaskineLevel" + i + "Stars", 0);
        }

        for (int i = 1; i <= amountOfLevels; i++){
            lapperTotalStars += PlayerPrefs.GetInt("LapperLevel" + i + "Stars", 0);
        }

        håndsyText.text = (håndsyTotalStars / totalAmountOfStars * 100) + "%";
        symaskineText.text = (symaskineTotalStars / totalAmountOfStars * 100) + "%";
        lapperText.text = (lapperTotalStars / totalAmountOfStars * 100) + "%";
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
