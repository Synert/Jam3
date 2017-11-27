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

    void OnMouseUp()
    {
        if (isStart)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
            SceneManager.LoadScene("GameScene");
        }
        if (isControls)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
            SceneManager.LoadScene("Controls");
        }
        if (isMainMenu)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
            SceneManager.LoadScene("Main Menu");
        }
        if (isQuit)
        {
            GetComponent<Renderer>().material.color = Color.red;
            Application.Quit();
        }
    }
}
