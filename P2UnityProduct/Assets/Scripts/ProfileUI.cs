using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour {

    [SerializeField] private Button restartButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI starsText;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartProgress();
        });

        backButton.onClick.AddListener(() => {
            GameManager.Instance.ExitProfile();
            anim.SetTrigger("Close");
            Invoke("Hide", 0.6f);
        });

        starsText.text = AmountOfStars().ToString();
    }

    private void Start(){
        GameManager.Instance.OnEnterProfile += GameManager_OnEnterProfile;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterProfile -= GameManager_OnEnterProfile;
    }

    private void GameManager_OnEnterProfile(object sender, EventArgs e){
        starsText.text = AmountOfStars().ToString();
        Show();
    }

    private int AmountOfStars(){
        int totalStars = 0;

        int amountOfLevels = 8;

        for (int i = 1; i <= amountOfLevels; i++){
            totalStars += PlayerPrefs.GetInt("HåndsyLevel" + i + "Stars", 0);
        }

        for (int i = 1; i <= amountOfLevels; i++){
            totalStars += PlayerPrefs.GetInt("SymaskineLevel" + i + "Stars", 0);
        }

        for (int i = 1; i <= amountOfLevels; i++){
            totalStars += PlayerPrefs.GetInt("LapperLevel" + i + "Stars", 0);
        }

        return totalStars;
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
