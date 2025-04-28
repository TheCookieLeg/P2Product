using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI buttonText;

    public String[] quizText = new String[3] {"Quiz level","Vælg den korrekte svarmulighed","Start quiz"};
    public String[] matchText = new String[3] {"Match level","Tryk på den tekst der matcher til billedet","Fortsæt"};
    public String[] storyText = new String[3] {"Story level","Tryk på billederne i den korrekte rækkefølge","Fortsæt"};
    public String[] gameText = new String[3] {"Game level","Lær at sy bagsting gennem et spil.","Fortsæt"};
    public String[] bossText = new String[3] {"Boss level","test","Fortsæt"};


	private void GameManager_OnEnterTurorial(object sender, EventArgs e){
        String[] tutorialText = new String[3];

        switch (GameManager.Instance.currentLevelData) {
            case QuizLevelSO:
                tutorialText = quizText;
                break;
            case MatchLevelSO:
                tutorialText = matchText;
                break;
            case StoryLevelSO:
                tutorialText = storyText;
                break;
            case GameLevelSO:
                tutorialText = gameText;
                break;
            case BossLevelSO:
                tutorialText = bossText;
                break;
            default:
                tutorialText = quizText;
                break;
        }

        titleText.SetText(tutorialText[0]);
        descriptionText.SetText(tutorialText[1]);
        buttonText.SetText(tutorialText[2]);

        Show();
    }
    public void ContinueToLevel() {
        GameManager.Instance.EnterLevel();
        Hide();
    } 

    private void Show() {
        gameObject.SetActive(true);        
    }
    private void Hide(){
        gameObject.SetActive(false);
    }

    private void Start(){
        GameManager.Instance.OnEnterTurorial += GameManager_OnEnterTurorial;
        Hide();
    }
    private void OnDestroy(){
        GameManager.Instance.OnEnterTurorial -= GameManager_OnEnterTurorial;
    }
    
}
