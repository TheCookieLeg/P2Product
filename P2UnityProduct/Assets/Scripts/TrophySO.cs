using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trophy", menuName = "Trophy")]
public class TrophySO : ScriptableObject {

    [Header("Question Info")]
    public int trophyID;
    public string trophyName;
    public Sprite trophyImage;
    public string trophyCondition;

    [Header("Progress")]
    public bool progress;
    public enum TrackedStat {
        None,
        Stjerner,
    }
    public TrackedStat trackedStat;
    public int statGoal;
}
