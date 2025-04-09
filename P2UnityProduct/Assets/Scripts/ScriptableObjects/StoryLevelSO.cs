using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quiz Question", menuName = "Levels/Story")]
public class StoryLevelSO : BaseLevelSO {

    [System.Serializable]
    public class Question{
        public Sprite[] answers = new Sprite[6];
    }

    public List<Question> questions = new List<Question>();
}
