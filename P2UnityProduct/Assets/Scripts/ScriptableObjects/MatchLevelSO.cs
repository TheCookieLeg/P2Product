using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Match Level", menuName = "Levels/Match")]
public class MatchLevelSO : BaseLevelSO {

    [System.Serializable]
    public class MatchPair{
        public int leftIndex;
        public int rightIndex;
    }

    [System.Serializable]
    public class Question{
        public string[] answers = new string[6];
        public List<MatchPair> matchPairs = new List<MatchPair>(3);
    }

    public List<Question> questions = new List<Question>();
}
