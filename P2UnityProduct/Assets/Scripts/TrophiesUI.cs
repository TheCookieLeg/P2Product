using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophiesUI : MonoBehaviour {

    [SerializeField] private Button backButton;
    [SerializeField] private Button overlayBackButton;
    [SerializeField] private GameObject trophyOverlay;
    [SerializeField] private Image trophyOverlayImage;
    [SerializeField] private TextMeshProUGUI trophyOverlayName;
    [SerializeField] private TextMeshProUGUI trophyOverlayCondition;
    [SerializeField] private Slider trophyOverlaySlider;
    [SerializeField] private TextMeshProUGUI trophyOverlaySliderText;
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        backButton.onClick.AddListener(() => {
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });

        overlayBackButton.onClick.AddListener(() => {
            trophyOverlay.GetComponent<Animator>().SetTrigger("End");
            Invoke("HideOverlay", 0.5f);
        });
    }

    private void Start(){
        GameManager.Instance.OnEnterTrophies += GameManager_OnEnterTrophies;
        GameManager.Instance.OnOpenTrophy += GameManager_OnOpenTrophy;

        Hide();
    }

    private void OnDestroy(){
        GameManager.Instance.OnEnterTrophies -= GameManager_OnEnterTrophies;
        GameManager.Instance.OnOpenTrophy -= GameManager_OnOpenTrophy;
    }

    private void GameManager_OnEnterTrophies(object sender, EventArgs e){
        Show();
    }

    private void GameManager_OnOpenTrophy(object sender, EventArgs e){
        ShowOverlay();
    }

    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Show(){
        gameObject.SetActive(true);
    }

    private void HideOverlay(){
        trophyOverlay.SetActive(false);
    }

    private void ShowOverlay(){
        trophyOverlay.SetActive(true);

        trophyOverlayImage.sprite = GameManager.Instance.currentTrophyData.trophyImage;
        trophyOverlayCondition.text = GameManager.Instance.currentTrophyData.trophyCondition;
        trophyOverlayName.text = GameManager.Instance.currentTrophyData.trophyName;
        if (PlayerPrefs.GetInt("Trophy" + GameManager.Instance.currentTrophyData.trophyID) == 0){
            trophyOverlayImage.color = new Color32(79, 69, 59, 255);
        } else {
            trophyOverlayImage.color = Color.white;
        }

        if (GameManager.Instance.currentTrophyData.progress){
            trophyOverlaySlider.gameObject.SetActive(true);
            trophyOverlaySliderText.text = GetStat() + "/" + GameManager.Instance.currentTrophyData.statGoal;
            trophyOverlaySlider.maxValue = GameManager.Instance.currentTrophyData.statGoal;
            trophyOverlaySlider.value = GetStat();
        } else {
            trophyOverlaySlider.gameObject.SetActive(false);
        }
    }

    private int GetStat(){
        if (GameManager.Instance.currentTrophyData.trackedStat == TrophySO.TrackedStat.Stjerner){
            return GameManager.Instance.GetTotalStars();
        } else {
            return 0;
        }
    }
}
