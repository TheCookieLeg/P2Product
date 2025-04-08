using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnEnterLevel;
    public event EventHandler OnExitLevel;
    public event EventHandler OnRefreshLevels;

    public int currentLevelID;
    public QuizLevelSO currentLevelQuizData;
    public MatchLevelSO currentLevelMatchData;
    public int levelsCompleted;
    public int stars;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
    }

    public void EnterLevel(int levelID, BaseLevelSO levelData){
        if (levelData == null){
            Debug.LogWarning("Dette level mangler data");
            return;
        }

        currentLevelID = levelID;

        if (levelData is QuizLevelSO quizData){
            currentLevelQuizData = quizData;
            currentLevelMatchData = null;
        } else if (levelData is MatchLevelSO matchData){
            currentLevelMatchData = matchData;
            currentLevelQuizData = null;
        }

        OnEnterLevel?.Invoke(this, EventArgs.Empty);
    }

    public void CompleteLevel(){
        OnExitLevel?.Invoke(this, EventArgs.Empty);

        if (currentLevelID - 1 == levelsCompleted){
            levelsCompleted++;

            PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        } else if (PlayerPrefs.GetInt("Level" + currentLevelID + "Stars") < stars){
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        }

        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void FailLevel(){
        OnExitLevel?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void RestartProgress(){
        levelsCompleted = 0;
        PlayerPrefs.DeleteAll();
        
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }
}
