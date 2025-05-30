using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour {

    [Header("Level Info")]
    [SerializeField] private int levelID = 0;
    [SerializeField] private BaseLevelSO levelData;

    [Header("References")]
    [SerializeField] private GameObject lockedButton;
    [SerializeField] private GameObject unlockedButton;
    [SerializeField] private TextMeshProUGUI levelTypeText;
    [SerializeField] private GameObject[] starsIcons;

    private int stars;
    private Button button;
    private bool unlocked = false;
    [SerializeField] private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();

        if (levelData is QuizLevelSO){
            levelTypeText.text = "QUIZ";
        } else if (levelData is MatchLevelSO){
            levelTypeText.text = "MATCH";
        } else if (levelData is StoryLevelSO){
            levelTypeText.text = "STORY";
        } else if (levelData is GameLevelSO){
            levelTypeText.text = "GAME";
        } else if (levelData is BossLevelSO){
            levelTypeText.text = "BOSS";
        }
    }

    private void Start(){
        GameManager.Instance.OnRefreshLevels += GameManager_OnRefreshLevels;

        UpdateVisuals();
    }

    private void OnDestroy(){
        GameManager.Instance.OnRefreshLevels -= GameManager_OnRefreshLevels;
    }

    private void UpdateVisuals(){
        stars = PlayerPrefs.GetInt("Level" + levelID + "Stars", 0);
        if (GameManager.Instance.levelsCompleted >= levelID - 1 || SceneManager.GetActiveScene().name != "Game +Levels"){
            unlocked = true;
            button = unlockedButton.GetComponent<Button>();
            for (int i = 0; i < 3; i++){
                if (stars >= i + 1){
                    starsIcons[i].transform.GetChild(0).gameObject.SetActive(false);
                    starsIcons[i].transform.GetChild(1).gameObject.SetActive(true);
                } else {
                    starsIcons[i].transform.GetChild(0).gameObject.SetActive(true);
                    starsIcons[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        } else {
            unlocked = false;
            button = lockedButton.GetComponent<Button>();
        }

        lockedButton.SetActive(!unlocked);
        unlockedButton.SetActive(unlocked);

        button.onClick.AddListener(() => {
            if (GameManager.Instance.canClickTimer > 0) return;
            GameManager.Instance.canClickTimer = 0.25f;

            if (levelID == 0 || levelData == null){
                Debug.LogWarning("Levelet er ikke sat op endnu");
                return;
            }
            
            if (unlocked){
                GameManager.Instance.HoverLevel(levelID, levelData);
            }
        });
    }

    private void GameManager_OnRefreshLevels(object sender, EventArgs e){
        UpdateVisuals();
    }

    private void FixedUpdate(){
        if (SceneManager.GetActiveScene().name != "Game +Levels") return;

        if (PlayerPrefs.GetInt("LevelsCompleted") == levelID - 1){
            anim.SetBool("Current", true);
        } else {
            anim.SetBool("Current", false);
        }
    }
}
