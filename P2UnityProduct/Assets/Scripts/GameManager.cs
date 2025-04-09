using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnEnterLevel;
    public event EventHandler OnExitToGameScene;
    public event EventHandler OnRefreshLevels;
    public event EventHandler OnEnterProfile;
    public event EventHandler OnHoverLevel;

    [HideInInspector] public QuizLevelSO currentLevelQuizData;
    [HideInInspector] public MatchLevelSO currentLevelMatchData;
    [HideInInspector] public StoryLevelSO currentLevelStoryData;

    private int currentLevelID;
    [HideInInspector] public int levelsCompleted;
    [HideInInspector] public int stars;

    [HideInInspector] public int hoverLevelID;
    [HideInInspector] public int hoverStars;
    private BaseLevelSO hoverLevelData;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
    }

    public void EnterLevel(){
        currentLevelID = hoverLevelID;

        if (hoverLevelData is QuizLevelSO quizData){
            currentLevelQuizData = quizData;
            currentLevelMatchData = null;
            currentLevelStoryData = null;
        } else if (hoverLevelData is MatchLevelSO matchData){
            currentLevelMatchData = matchData;
            currentLevelQuizData = null;
            currentLevelStoryData = null;
        } else if (hoverLevelData is StoryLevelSO storyData){
            currentLevelStoryData = storyData;
            currentLevelMatchData = null;
            currentLevelQuizData = null;
        }

        OnEnterLevel?.Invoke(this, EventArgs.Empty);
    }

    public void CompleteLevel(){
        OnExitToGameScene?.Invoke(this, EventArgs.Empty);

        if (currentLevelID - 1 == levelsCompleted){
            levelsCompleted++;

            PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        } else if (PlayerPrefs.GetInt("Level" + currentLevelID + "Stars") < stars){
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        }

        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void BackToGameScene(){
        OnExitToGameScene?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void EnterProfile(){
        OnEnterProfile?.Invoke(this, EventArgs.Empty);
    }

    public void RestartProgress(){
        levelsCompleted = 0;
        PlayerPrefs.DeleteAll();
        
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void HoverLevel(int levelID, BaseLevelSO levelData){
        hoverLevelID = levelID;
        hoverLevelData = levelData;
        hoverStars = PlayerPrefs.GetInt("Level" + levelID + "Stars");
        OnHoverLevel?.Invoke(this, EventArgs.Empty);
    }
}
