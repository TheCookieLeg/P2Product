using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtons : MonoBehaviour {

    [SerializeField] private int levelID = 0;
    private int stars;

    [SerializeField] private GameObject lockedButton;
    [SerializeField] private GameObject unlockedButton;
    [SerializeField] private GameObject[] starsIcons;

    private void Awake(){
        if (levelID == 0){
            Debug.LogWarning("Level not set-up yet!");
        }

        //stars = COMPLETEDLEVELSTARS SOMETHING;

        if (1 >= levelID){ // 1 Skal være det level man er kommet til
            Debug.Log("Beat level: " + levelID);
            lockedButton.SetActive(false);
            unlockedButton.SetActive(true);
            for (int i = 0; i < 3; i++){
                if (stars >= i + 1){
                    starsIcons[i].transform.GetChild(0).gameObject.SetActive(true);
                    starsIcons[i].transform.GetChild(1).gameObject.SetActive(false);
                } else {
                    starsIcons[i].transform.GetChild(0).gameObject.SetActive(false);
                    starsIcons[i].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        } else {
            Debug.Log("Haven't beaten level: " + levelID);
            lockedButton.SetActive(true);
            unlockedButton.SetActive(false);
        }
    }
}
