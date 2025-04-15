using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [SerializeField] private Button backButton;
    [SerializeField] private Button beatLevelButton;

    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("Needle End");
            Invoke("Hide", 0.5f);
        });

        beatLevelButton.onClick.AddListener(() => {
            GameManager.Instance.CompleteLevel();
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
        if (GameManager.Instance.currentLevelGameData == null) return;

        GameManager.Instance.stars = 3;

        Show();
        anim.SetTrigger("Needle Start");
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    public void Exit(){
        GameManager.Instance.CompleteLevel();
        anim.SetTrigger("End");
        Invoke("Hide", 0.5f);
    }
}
