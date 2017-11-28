using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreText : MonoBehaviour
{
    public Text score;
    public Text highScore;

    void Start()
    {
        score.text = PlayerPrefs.GetInt("PreviousScore", 0).ToString();
        highScore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
    }

}
