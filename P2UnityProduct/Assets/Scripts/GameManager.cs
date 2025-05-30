using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnEnterLevel;
    public event EventHandler OnExitToGameScene;
    public event EventHandler OnRefreshLevels;
    public event EventHandler OnEnterProfile;
    public event EventHandler OnHoverLevel;
    public event EventHandler OnEnterThread;
    public event EventHandler OnLeaveLevel;
    public event EventHandler OnLeaveThread;
    public event EventHandler OnResetSave;
    public event EventHandler OnEnterTutorial;

    [SerializeField] [Range(0.5f, 1.5f)] private float transmissionTid = 1f;
    [SerializeField] private Animator transmissionAnim;

    [HideInInspector] public BaseLevelSO currentLevelData;

    private int currentLevelID;
    public int levelsCompleted;
    public int stars;

    [HideInInspector] public int hoverLevelID;
    [HideInInspector] public int hoverStars;
    private BaseLevelSO hoverLevelData;

    [HideInInspector] public float canClickTimer;

    public Texture2D picture1; // display area for picture taken
    public Texture2D picture2; // display area for picture taken
    public Texture2D picture3; // display area for picture taken

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");

        Application.targetFrameRate = 120;
    }

    private void Update(){
        if (canClickTimer > 0) canClickTimer -= Time.deltaTime;
    }
    public void EnterTutorial(){
        currentLevelID = hoverLevelID;

        if (hoverLevelData is QuizLevelSO quizData){
            currentLevelData = quizData;
        } else if (hoverLevelData is MatchLevelSO matchData){
            currentLevelData = matchData;
        } else if (hoverLevelData is StoryLevelSO storyData){
            currentLevelData = storyData;
        } else if (hoverLevelData is GameLevelSO gameData){
            currentLevelData = gameData;
        } else if (hoverLevelData is BossLevelSO bossData){
            currentLevelData = bossData;
        } else {
            currentLevelData = null;
            Debug.LogWarning("Level type not found");
        }

        OnEnterTutorial?.Invoke(this, EventArgs.Empty);
        // OnEnterLevel?.Invoke(this, EventArgs.Empty);
    }
    public void EnterLevel(){
        OnEnterLevel?.Invoke(this, EventArgs.Empty);
    }

    public void CompleteLevel(){
        OnExitToGameScene?.Invoke(this, EventArgs.Empty);

        if (currentLevelID - 1 == levelsCompleted){ // Klaret den bane man var kommet
            levelsCompleted++;

            PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        } else if (PlayerPrefs.GetInt("Level" + currentLevelID + "Stars") < stars){ // Har ny rekord på en bane
            PlayerPrefs.SetInt("Level" + currentLevelID + "Stars", stars);
        }

        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void BackToGameScene(){
        OnExitToGameScene?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void ExitProfile(){
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void EnterProfile(){
        OnEnterProfile?.Invoke(this, EventArgs.Empty);
    }

    public void RestartProgress(){
        levelsCompleted = 0;
        PlayerPrefs.DeleteAll();
        
        OnResetSave?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void CompleteEverything(){
        levelsCompleted = 8;
        PlayerPrefs.SetInt("LevelsCompleted", levelsCompleted);
        PlayerPrefs.SetInt("Level" + 1 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 2 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 3 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 4 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 5 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 6 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 7 + "Stars", 1);
        PlayerPrefs.SetInt("Level" + 8 + "Stars", 1);

        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void HoverLevel(int levelID, BaseLevelSO levelData){
        hoverLevelID = levelID;
        hoverLevelData = levelData;

        hoverStars = PlayerPrefs.GetInt("Level" + levelID + "Stars");

        OnHoverLevel?.Invoke(this, EventArgs.Empty);
    }

    public void EnterThread(){
        StartCoroutine(EnterThreadDelay());
    }

    private IEnumerator EnterThreadDelay(){
        transmissionAnim.SetTrigger("Close");

        yield return new WaitForSeconds(transmissionTid);

        transmissionAnim.SetTrigger("Open");
        OnLeaveThread?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
        OnEnterThread?.Invoke(this, EventArgs.Empty);
    }

    public void LeaveLevel(){
        StartCoroutine(LeaveLevelDelay());
    }

    private IEnumerator LeaveLevelDelay(){
        transmissionAnim.SetTrigger("Close");

        yield return new WaitForSeconds(transmissionTid);

        transmissionAnim.SetTrigger("Open");
        OnLeaveLevel?.Invoke(this, EventArgs.Empty);
    }

    public int GetTotalStars(){
        int totalStars = 0;

        int amountOfLevels = 8;

        for (int i = 1; i <= amountOfLevels; i++){
            totalStars += PlayerPrefs.GetInt("Level" + i + "Stars", 0);
        }

        return totalStars;
    }
}
