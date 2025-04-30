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
    [SerializeField] private RawImage picture1;
    [SerializeField] private RawImage picture2;
    [SerializeField] private RawImage picture3;
    [SerializeField] private Button picture1Button;
    [SerializeField] private Button picture2Button;
    [SerializeField] private Button picture3Button;
    [SerializeField] private Button overlayBackButton;
    [SerializeField] private Button completeEverythingButton;
    [SerializeField] private GameObject overlay;
    [SerializeField] private RawImage cameraOutput;
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
        picture1Button.onClick.AddListener(() => {
            if (GameManager.Instance.picture1 == null) return;
            cameraOutput.texture = GameManager.Instance.picture1;
            overlay.SetActive(true);
        });
        picture2Button.onClick.AddListener(() => {
            if (GameManager.Instance.picture2 == null) return;
            cameraOutput.texture = GameManager.Instance.picture2;
            overlay.SetActive(true);
        });
        picture3Button.onClick.AddListener(() => {
            if (GameManager.Instance.picture3 == null) return;
            cameraOutput.texture = GameManager.Instance.picture3;
            overlay.SetActive(true);
        });
        overlayBackButton.onClick.AddListener(() => {
            overlay.SetActive(false);
        });
        completeEverythingButton.onClick.AddListener(() => {
            GameManager.Instance.CompleteEverything();
        });

        starsText.text = GameManager.Instance.GetTotalStars().ToString();
        streakText.text = "0";
    }

    private void Start(){
        GameManager.Instance.OnEnterProfile += GameManager_OnEnterProfile;
        GameManager.Instance.OnRefreshLevels += GameManger_OnRefreshLevels;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterProfile -= GameManager_OnEnterProfile;
        GameManager.Instance.OnRefreshLevels -= GameManger_OnRefreshLevels;
    }

    private void GameManager_OnEnterProfile(object sender, EventArgs e){
        starsText.text = GameManager.Instance.GetTotalStars().ToString();
        Show();
        overlay.SetActive(false);

        if (GameManager.Instance.picture1 != null){
            picture1.texture = GameManager.Instance.picture1;
            picture1.color = Color.white;
        }

        if (GameManager.Instance.picture2 != null){
            picture2.texture = GameManager.Instance.picture2;
            picture2.color = Color.white;
        }

        if (GameManager.Instance.picture3 != null){
            picture3.texture = GameManager.Instance.picture3;
            picture3.color = Color.white;
        }
    }

    private void GameManger_OnRefreshLevels(object sender, EventArgs e){
        starsText.text = GameManager.Instance.GetTotalStars().ToString();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }
}
