using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour {

    [SerializeField] private Button restartButton;

    private void Start(){
        // Subscriber til events
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitLevel += GameManager_OnExitLevel;

        // Initialiserer knappen
        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartProgress();
        });
    }

    private void OnDestroy(){
        // Unsubscriber til events når GameObjectet bliver ødelagt (Sker aldrig, medmindre vi skifter scene/selv sletter dette GameObject)
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitLevel -= GameManager_OnExitLevel;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        // Slår denne side fra når vi vælger et level
        Hide();
    }

    private void GameManager_OnExitLevel(object sender, EventArgs e){
        // Viser denne side når vi vinder/taber
        Show();
    }

    private void Hide(){
        // Slår siden fra
        gameObject.SetActive(false);
    }

    private void Show(){
        // Slår siden til
        gameObject.SetActive(true);
    }
}
