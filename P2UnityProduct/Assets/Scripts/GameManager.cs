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
    public event EventHandler OnEnterThread;
    public event EventHandler OnLeaveLevel;
    public event EventHandler OnLeaveThread;

    [SerializeField] private Animator transmissionAnim;

    [HideInInspector] public QuizLevelSO currentLevelQuizData;
    [HideInInspector] public MatchLevelSO currentLevelMatchData;
    [HideInInspector] public StoryLevelSO currentLevelStoryData;

    private int currentLevelID;
    [HideInInspector] public int levelsCompletedHåndsy;
    [HideInInspector] public int levelsCompletedSymaskine;
    [HideInInspector] public int levelsCompletedLapper;
    [HideInInspector] public int stars;

    [HideInInspector] public int hoverLevelID;
    [HideInInspector] public int hoverStars;
    private BaseLevelSO hoverLevelData;

    [HideInInspector] public int threadID;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        levelsCompletedHåndsy = PlayerPrefs.GetInt("LevelsCompleted0");
        levelsCompletedSymaskine = PlayerPrefs.GetInt("LevelsCompleted1");
        levelsCompletedLapper = PlayerPrefs.GetInt("LevelsCompleted2");
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

        switch (threadID){
            case 0:
                if (currentLevelID - 1 == levelsCompletedHåndsy){
                    levelsCompletedHåndsy++;

                    PlayerPrefs.SetInt("LevelsCompleted" + threadID, levelsCompletedHåndsy);
                    PlayerPrefs.SetInt("HåndsyLevel" + currentLevelID + "Stars", stars);
                } else if (PlayerPrefs.GetInt("HåndsyLevel" + currentLevelID + "Stars") < stars){
                    PlayerPrefs.SetInt("HåndsyLevel" + currentLevelID + "Stars", stars);
                }
                break;
            case 1:
                if (currentLevelID - 1 == levelsCompletedSymaskine){
                    levelsCompletedSymaskine++;

                    PlayerPrefs.SetInt("LevelsCompleted" + threadID, levelsCompletedSymaskine);
                    PlayerPrefs.SetInt("SymaskineLevel" + currentLevelID + "Stars", stars);
                } else if (PlayerPrefs.GetInt("SymaskineLevel" + currentLevelID + "Stars") < stars){
                    PlayerPrefs.SetInt("SymaskineLevel" + currentLevelID + "Stars", stars);
                }
                break;
            case 2:
                if (currentLevelID - 1 == levelsCompletedLapper){
                    levelsCompletedLapper++;

                    PlayerPrefs.SetInt("LevelsCompleted" + threadID, levelsCompletedLapper);
                    PlayerPrefs.SetInt("LapperLevel" + currentLevelID + "Stars", stars);
                } else if (PlayerPrefs.GetInt("LapperLevel" + currentLevelID + "Stars") < stars){
                    PlayerPrefs.SetInt("LapperLevel" + currentLevelID + "Stars", stars);
                }
                break;
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
        levelsCompletedHåndsy = 0;
        levelsCompletedSymaskine = 0;
        levelsCompletedLapper = 0;
        PlayerPrefs.DeleteAll();
        
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
    }

    public void HoverLevel(int levelID, BaseLevelSO levelData){
        hoverLevelID = levelID;
        hoverLevelData = levelData;
        switch (threadID){
            case 0:
                hoverStars = PlayerPrefs.GetInt("HåndsyLevel" + levelID + "Stars");
                break;
            case 1:
                hoverStars = PlayerPrefs.GetInt("SymaskineLevel" + levelID + "Stars");
                break;
            case 2:
                hoverStars = PlayerPrefs.GetInt("LapperLevel" + levelID + "Stars");
                break;
        }
        OnHoverLevel?.Invoke(this, EventArgs.Empty);
    }

    public void EnterThread(int chosenThreadID){
        StartCoroutine(EnterThreadDelay(chosenThreadID));
    }

    private IEnumerator EnterThreadDelay(int chosenThreadID){
        transmissionAnim.SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        transmissionAnim.SetTrigger("Open");
        threadID = chosenThreadID;
        OnLeaveThread?.Invoke(this, EventArgs.Empty);
        OnRefreshLevels?.Invoke(this, EventArgs.Empty);
        OnEnterThread?.Invoke(this, EventArgs.Empty);
    }

    public void LeaveLevel(){
        StartCoroutine(LeaveLevelDelay());
    }

    private IEnumerator LeaveLevelDelay(){
        transmissionAnim.SetTrigger("Close");

        yield return new WaitForSeconds(1f);

        transmissionAnim.SetTrigger("Open");
        OnLeaveLevel?.Invoke(this, EventArgs.Empty);
    }
}
