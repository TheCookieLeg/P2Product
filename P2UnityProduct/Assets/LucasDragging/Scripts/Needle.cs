using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour {

    private GameUI gameUIScript;

    private void Awake(){
        gameUIScript = GetComponentInParent<GameUI>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Score")){
            gameUIScript.HitScorer();
        }
    }
}
