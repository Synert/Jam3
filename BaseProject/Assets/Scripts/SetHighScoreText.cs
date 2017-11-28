using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHighScoreText : MonoBehaviour {

	// Use this for initialization

    public Text highScore;

    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
    }
}
	


