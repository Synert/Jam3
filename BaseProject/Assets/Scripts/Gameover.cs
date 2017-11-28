using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{

    public int highscore;
	public Text highscoreText;
    public int score;

    public void setScore(int _score)
    {
        int m_score = _score;
        PlayerPrefs.SetInt("PreviousScore", m_score);

        if (_score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", m_score);
        }
        loadScene();
    }

	void Update() {
		if (PlayerPrefs.GetInt ("Highscore", highscore) != -1) {
			highscoreText.text = "Highscore: " + highscore.ToString();
		}
	}

    void loadScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
