using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour {

    [SerializeField] private Button restartButton;
    [SerializeField] private Button backButton;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartProgress();
        });

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("Close");
            Invoke("Hide", 0.5f);
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterProfile += GameManager_OnEnterProfile;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterProfile -= GameManager_OnEnterProfile;
    }

  private void GameManager_OnEnterProfile(object sender, EventArgs e){
        Show();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
