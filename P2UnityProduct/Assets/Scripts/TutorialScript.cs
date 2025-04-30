using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] Button backButton;

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI buttonText;
    private Animator anim;

    private String[] quizText = new String[3] {$"Quiz level","Vælg den korrekte svarmulighed","Start quiz"};
    private String[] matchText = new String[3] {$"Match level","Tryk på den tekst der matcher til billedet","Fortsæt"};
    private String[] storyText = new String[3] {$"Story level","Tryk på billederne i den korrekte rækkefølge","Fortsæt"};
    private String[] gameText = new String[3] {$"Game level","Lær at sy bagsting gennem et spil.","Fortsæt"};
    private String[] bossText = new String[3] {$"Boss level","Som den sidste opgave i dette forløb, skal du bruge hvad du har lært, og lappe 2 stykker tøj sammen, samt sy en knap på stoffet efter. \n\n Hvad du skal bruge: \n- Nål \n- passende længde tråd der skal lægges dobbelt \n- 2 stykker stof \n- en knap  \n\nNår du har færdiggjort opgaven, kan du gå videre til næste side!","Fortsæt"};

	private void Awake()
	{
        anim = GetComponent<Animator>();

		backButton.onClick.AddListener(() => {
            GameManager.Instance.BackToGameScene();
            anim.SetTrigger("End");
            Invoke("Hide", 0.5f);
        });
	}
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
        anim.SetTrigger("Start");
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
        GameManager.Instance.OnEnterTutorial += GameManager_OnEnterTurorial;
        Hide();
    }
    private void OnDestroy(){
        GameManager.Instance.OnEnterTutorial -= GameManager_OnEnterTurorial;
    }
    
}
