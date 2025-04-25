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
    public event EventHandler OnEnterTrophies;
    public event EventHandler OnOpenTrophy;

    [SerializeField] [Range(0.5f, 1.5f)] private float transmissionTid = 1f;
    [SerializeField] private Animator transmissionAnim;

    [SerializeField] private GameObject trophyPopupPrefab;
    [SerializeField] private Transform trophyPopTransform;
    [SerializeField] private TrophySO[] trophyScriptableObjects;

    [HideInInspector] public QuizLevelSO currentLevelQuizData;
    [HideInInspector] public MatchLevelSO currentLevelMatchData;
    [HideInInspector] public StoryLevelSO currentLevelStoryData;
    [HideInInspector] public GameLevelSO currentLevelGameData;
    [HideInInspector] public BossLevelSO currentLevelBossData;

    private int currentLevelID;
    [HideInInspector] public int levelsCompleted;
    [HideInInspector] public int stars;

    [HideInInspector] public int hoverLevelID;
    [HideInInspector] public int hoverStars;
    private BaseLevelSO hoverLevelData;

    [HideInInspector] public TrophySO currentTrophyData;

    [HideInInspector] public float canClickTimer;

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

  public void EnterLevel(){
        currentLevelID = hoverLevelID;

        if (hoverLevelData is QuizLevelSO quizData){
            currentLevelQuizData = quizData;
            currentLevelMatchData = null;
            currentLevelStoryData = null;
            currentLevelGameData = null;
            currentLevelBossData = null;
        } else if (hoverLevelData is MatchLevelSO matchData){
            currentLevelQuizData = null;
            currentLevelMatchData = matchData;
            currentLevelStoryData = null;
            currentLevelGameData = null;
            currentLevelBossData = null;
        } else if (hoverLevelData is StoryLevelSO storyData){
            currentLevelQuizData = null;
            currentLevelMatchData = null;
            currentLevelStoryData = storyData;
            currentLevelGameData = null;
            currentLevelBossData = null;
        } else if (hoverLevelData is GameLevelSO gameData){
            currentLevelQuizData = null;
            currentLevelMatchData = null;
            currentLevelStoryData = null;
            currentLevelGameData = gameData;
            currentLevelBossData = null;
        } else if (hoverLevelData is BossLevelSO bossData){
            currentLevelQuizData = null;
            currentLevelMatchData = null;
            currentLevelStoryData = null;
            currentLevelGameData = null;
            currentLevelBossData = bossData;
        } else {
            currentLevelQuizData = null;
            currentLevelMatchData = null;
            currentLevelStoryData = null;
            currentLevelGameData = null;
            currentLevelGameData = null;
            Debug.LogWarning("Level type not found");
        }

        // if (PlayerPrefs.GetInt("Trophy1") == 0){
        //     CompleteTrophy(1);
        // }

        OnEnterLevel?.Invoke(this, EventArgs.Empty);
    }

    public void CompleteLevel(){
        OnExitToGameScene?.Invoke(this, EventArgs.Empty);

        // if (PlayerPrefs.GetInt("Trophy2") == 0){
        //     CompleteTrophy(2);
        // }

        // if (stars == 3 && PlayerPrefs.GetInt("Trophy3") == 0){
        //     CompleteTrophy(3);
        // }

        // if (BOSSBANE KLARET NOGET && PlayerPrefs.GetInt("Trophy4") == 0){
        //     CompleteTrophy(4);
        // }

        // if (GetTotalStars() == 24 && PlayerPrefs.GetInt("Trophy5") == 0){
        //     CompleteTrophy(5);
        // }

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

    public void EnterTrophies(){
        OnEnterTrophies?.Invoke(this, EventArgs.Empty);
    }

    public void OpenTrophy(TrophySO trophyData){
        currentTrophyData = trophyData;
        OnOpenTrophy?.Invoke(this, EventArgs.Empty);
    }

    public int GetTotalStars(){
        int totalStars = 0;

        int amountOfLevels = 8;

        for (int i = 1; i <= amountOfLevels; i++){
            totalStars += PlayerPrefs.GetInt("Level" + i + "Stars", 0);
        }

        return totalStars;
    }

    private void CompleteTrophy(int trophyID){
        PlayerPrefs.SetInt("Trophy" + trophyID, 1);
        
        TrophySO currentTrophySO = trophyScriptableObjects[trophyID - 1];

        GameObject trophyPopup = Instantiate(trophyPopupPrefab, trophyPopTransform);
        trophyPopup.transform.Find("Parent/TrophyIcon").GetComponent<Image>().sprite = currentTrophySO.trophyImage;
        trophyPopup.transform.Find("Parent/TrophyName").GetComponent<TextMeshProUGUI>().text = currentTrophySO.trophyName;

        Destroy(trophyPopup, 3);
    }
}
