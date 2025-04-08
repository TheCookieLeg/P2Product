using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour {

    [Header("Level Info")]
    [SerializeField] private int levelID = 0;
    [SerializeField] private QuizLevelSO quizData;

    [Header("References")]
    [SerializeField] private GameObject lockedButton;
    [SerializeField] private GameObject unlockedButton;
    [SerializeField] private GameObject[] starsIcons;

    private int stars;
    private Button button;
    private bool unlocked = false;

    private void Start(){
        if (levelID == 0){ // Hvis man ikke har sat ID'et op endnu
            Debug.LogWarning("Level ID'et er ikke sat op endnu");
            return;
        }

        // Subscriber til events
        GameManager.Instance.OnRefreshLevels += GameManager_OnRefreshLevels;

        // Opdaterer knappen til at starte med
        UpdateVisuals();
    }

    private void UpdateVisuals(){
        stars = PlayerPrefs.GetInt("Level" + levelID + "Stars", 0);

        if (GameManager.Instance.levelsCompleted >= levelID - 1){
            // Vis unlocked knap
            unlocked = true;
            button = unlockedButton.GetComponent<Button>();
            for (int i = 0; i < 3; i++){
                if (stars >= i + 1){
                    starsIcons[i].transform.GetChild(0).gameObject.SetActive(false); // Deaktiver Not Filled Star
                    starsIcons[i].transform.GetChild(1).gameObject.SetActive(true); // Aktiver Filled Star
                } else {
                    starsIcons[i].transform.GetChild(0).gameObject.SetActive(true); // Aktiver Not Filled Star
                    starsIcons[i].transform.GetChild(1).gameObject.SetActive(false); // Deaktiver Filled Star
                }
            }
        } else {
            // Vis locked knap
            unlocked = false;
            button = lockedButton.GetComponent<Button>();
        }
        lockedButton.SetActive(!unlocked);
        unlockedButton.SetActive(unlocked);

        // Klik event på knappen
        button.onClick.AddListener(() => {
            if (unlocked){
                GameManager.Instance.EnterLevel(levelID, quizData);
            } else {
                Debug.Log("Level er ikke låst op");
            }
        });
    }

    private void OnDestroy(){
        // Unsubscriber til events når GameObjectet bliver ødelagt (Sker aldrig, medmindre vi skifter scene/selv sletter dette GameObject)
        GameManager.Instance.OnRefreshLevels -= GameManager_OnRefreshLevels;
    }

    private void GameManager_OnRefreshLevels(object sender, EventArgs e){
        // Opdaterer knappen når vi refresher siden (Restart knap/Vinder et level)
        UpdateVisuals();
    }
}
