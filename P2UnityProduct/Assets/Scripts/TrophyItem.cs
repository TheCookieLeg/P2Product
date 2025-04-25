using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyItem : MonoBehaviour {

    [SerializeField] private TrophySO trophyData;
    [SerializeField] private TextMeshProUGUI trophyNameText;
    [SerializeField] private Image trophyImage;

    private void Awake(){
        if (trophyData == null || trophyData.trophyID == 0){
            Debug.LogWarning("Trophy not properly set-up");
            return;
        }

        UpdateUI();

        GetComponent<Button>().onClick.AddListener(() => {
            GameManager.Instance.OpenTrophy(trophyData);
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterTrophies += GameManager_OnEnterTrophies;
    }

    private void GameManager_OnEnterTrophies(object sender, EventArgs e){
        UpdateUI();
    }

    private void UpdateUI(){
        trophyImage.sprite = trophyData.trophyImage;
        trophyNameText.text = trophyData.trophyName;
        if (PlayerPrefs.GetInt("Trophy" + trophyData.trophyID) == 0){
            trophyImage.color = new Color32(79, 69, 59, 255);
        } else {
            trophyImage.color = Color.white;
        }
    }
}
