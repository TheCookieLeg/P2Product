using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour {

    [SerializeField] private Button profileButton;

    private void Awake(){
        profileButton.onClick.AddListener(() => {
            GameManager.Instance.EnterProfile();
        });
    }
}
