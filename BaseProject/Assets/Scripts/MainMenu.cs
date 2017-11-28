using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool isStart;
    public bool isControls;
    public bool isMainMenu;
    public bool isQuit;

    public Color selectedColour;

    void OnMouseUp()
    {
        if (isStart)
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.5f);
            SceneManager.LoadScene("GameScene");
        }
        if (isControls)
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.5f);
            SceneManager.LoadScene("Controls");
        }
        if (isMainMenu)
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.5f);
            SceneManager.LoadScene("Main Menu");
        }
        if (isQuit)
        {
            GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.5f);
            Application.Quit();
        }
    }
}
