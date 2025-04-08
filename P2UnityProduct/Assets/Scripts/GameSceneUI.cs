using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour {

    private void Start(){
        GameManager.Instance.OnEnterLevel += GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene += GameManager_OnExitToGameScene;
        GameManager.Instance.OnEnterProfile += GameManager_OnEnterProfile;
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterLevel -= GameManager_OnEnterLevel;
        GameManager.Instance.OnExitToGameScene -= GameManager_OnExitToGameScene;
        GameManager.Instance.OnEnterProfile -= GameManager_OnEnterProfile;
    }

    private void GameManager_OnEnterLevel(object sender, EventArgs e){
        Hide();
    }

    private void GameManager_OnExitToGameScene(object sender, EventArgs e){
        Show();
    }

    private void GameManager_OnEnterProfile(object sender, EventArgs e){
        Hide();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
