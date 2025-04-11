using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour {

    [SerializeField] private Button profileButton;
    [SerializeField] private TextMeshProUGUI starText;

    private void Awake(){
        profileButton.onClick.AddListener(() => {
            GameManager.Instance.EnterProfile();
        });
        UpdateCompletion();
    }

    private void Start(){
        GameManager.Instance.OnRefreshLevels += GameManager_OnRefreshLevels;
        GameManager.Instance.OnResetSave += GameManager_OnResetSave;
    }

    private void GameManager_OnRefreshLevels(object sender, EventArgs e){
        UpdateCompletion();
    }

    private void GameManager_OnResetSave(object sender, EventArgs e){
        UpdateCompletion();
    }

    private void UpdateCompletion(){
        int håndsyTotalStars = 0;

        int amountOfLevels = 8;

        float totalAmountOfStars = amountOfLevels * 3;

        for (int i = 1; i <= amountOfLevels; i++){
            håndsyTotalStars += PlayerPrefs.GetInt("Level" + i + "Stars", 0);
        }

        starText.text = håndsyTotalStars + "/" + totalAmountOfStars;
    }
}
