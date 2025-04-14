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
    }

    private void Start(){
        GameManager.Instance.OnRefreshLevels += GameManager_OnRefreshLevels;
        GameManager.Instance.OnResetSave += GameManager_OnResetSave;

        starText.text = GameManager.Instance.GetTotalStars() + "/24";
    }

    private void GameManager_OnRefreshLevels(object sender, EventArgs e){
        starText.text = GameManager.Instance.GetTotalStars() + "/24";
    }

    private void GameManager_OnResetSave(object sender, EventArgs e){
        starText.text = GameManager.Instance.GetTotalStars() + "/24";
    }
}
