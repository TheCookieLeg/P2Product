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
    [SerializeField] private TextMeshProUGUI streakText;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartProgress();
            starsText.text = "0";
        });

        backButton.onClick.AddListener(() => {
            GameManager.Instance.ExitProfile();
            anim.SetTrigger("Close");
            Invoke("Hide", 0.6f);
        });

        starsText.text = GameManager.Instance.GetTotalStars().ToString();
        streakText.text = "0";
    }

    private void Start(){
        GameManager.Instance.OnEnterProfile += GameManager_OnEnterProfile;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterProfile -= GameManager_OnEnterProfile;
    }

    private void GameManager_OnEnterProfile(object sender, EventArgs e){
        starsText.text = GameManager.Instance.GetTotalStars().ToString();
        Show();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
