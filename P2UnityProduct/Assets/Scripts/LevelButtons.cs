using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private void Awake(){
        if (levelID == 0 || levelData == null) return;

        if (levelData is QuizLevelSO){
            levelTypeText.text = "QUIZ";
        } else if (levelData is MatchLevelSO){
            levelTypeText.text = "MATCH";
        } else if (levelData is StoryLevelSO){
            levelTypeText.text = "STORY";
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
        switch (GameManager.Instance.threadID){
            case 0:
                stars = PlayerPrefs.GetInt("HåndsyLevel" + levelID + "Stars", 0);
                if (GameManager.Instance.levelsCompletedHåndsy >= levelID - 1){
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
                break;
            case 1:
                stars = PlayerPrefs.GetInt("SymaskineLevel" + levelID + "Stars", 0);
                if (GameManager.Instance.levelsCompletedSymaskine >= levelID - 1){
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
                break;
            case 2:
                stars = PlayerPrefs.GetInt("LapperLevel" + levelID + "Stars", 0);
                if (GameManager.Instance.levelsCompletedLapper >= levelID - 1){
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
                break;
        }

        lockedButton.SetActive(!unlocked);
        unlockedButton.SetActive(unlocked);

        button.onClick.AddListener(() => {
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
}
