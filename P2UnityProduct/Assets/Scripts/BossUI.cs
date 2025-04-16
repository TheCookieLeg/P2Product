
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour {

    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmButton;

    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });
            confirmButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        if (GameManager.Instance.currentLevelBossData == null) return;

        Show();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
