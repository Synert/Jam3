using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool isStart;
    public bool isHighscore;
    public bool isMainMenu;
    public bool isQuit;


    public bool isPressed;

    public Color selectedColour;

    public void ChangeScene()
	{
		Time.timeScale = 1;
		if (isStart) {
			SceneManager.LoadScene ("GameScene");
		}
		if (isHighscore) {
			SceneManager.LoadScene ("Highscore");
		}
		if (isMainMenu) {
			SceneManager.LoadScene ("Main Menu");
		}
		if (isQuit) {
			Application.Quit ();
		}
	}
}
