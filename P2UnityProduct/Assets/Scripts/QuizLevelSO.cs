using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Quiz Question", menuName = "Levels/Quiz")]
public class QuizLevelSO : BaseLevelSO {

    [System.Serializable]
    public class Question{
        [Header("Question Info")]
        public string questionText;
        public VideoClip videoClip;

        [Header("Answer Info")]
        public string[] answers = new string[4];

        [Range(1, 4)]
        public int correctAnswerIndex;
    }

    public List<Question> questions = new List<Question>();
}
