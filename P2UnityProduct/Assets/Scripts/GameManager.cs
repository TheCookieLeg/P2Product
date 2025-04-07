using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    // Events
    public event EventHandler OnEnterLevel;
    public event EventHandler OnExitLevel;
    public event EventHandler OnRefreshLevels;

    public int currentLevelID;
    public QuizLevelSO currentLevelQuizData;
    public int levelsCompleted;
    public int stars;

    private void Awake(){
        // Singelton
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Loader gemt levelsCompleted
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
    }

    public void EnterLevel(int levelID, QuizLevelSO quizData){
        if (quizData == null){
            Debug.LogWarning("Dette level mangler spørgsmål");
            return;
        }

        // Opdateret currentLevel variabler
        currentLevelID = levelID;
        currentLevelQuizData = quizData;

        // Kører event
        OnEnterLevel?.Invoke(this, EventArgs.Empty);
    }

    public void CompleteLevel(){
        // Kører event
        OnExitLevel?.Invoke(this, EventArgs.Empty);

        // Opdaterer hvor langt man er nået (Hvis man klarer den bane man er nået til)
        if (currentLevelID - 1 == levelsCompleted){
            levelsCompleted++;

            // Gemmer
            PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        } else if (PlayerPrefs.GetInt("Level" + currentLevelID + "Stars") < stars){
            // Hvis man har fået flere stjerne end sidst på samme level
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        }

        // Kører event
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void FailLevel(){
        // Kører events
        OnExitLevel?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void RestartProgress(){
        // Nulstiller variabel
        levelsCompleted = 0;

        // Sletter saveData
        PlayerPrefs.DeleteAll();
        
        // Kører event
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }
}
