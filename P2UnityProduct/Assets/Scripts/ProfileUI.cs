using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour {

    [SerializeField] private Button restartButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button trophiesButton;
    [SerializeField] private TextMeshProUGUI starsText;
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

        trophiesButton.onClick.AddListener(() => {
            GameManager.Instance.EnterTrophies();
        });

        starsText.text = GameManager.Instance.GetTotalStars().ToString();
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
