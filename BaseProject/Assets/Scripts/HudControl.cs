using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudControl : MonoBehaviour {

    public Transform pauseMenu;

	void Start ()
    {
        pauseMenu.gameObject.SetActive(false);
    }
	
	void Update ()
    {
        if(Input.GetKeyDown (KeyCode.Escape))
        {
            if(pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }



        //Take highest point of tower, take the grid value and apply it to Current height
		//if current height exceeds highscore, update highscore
        //If button is pressed, toggle visual on corrosponding HUD button
        //If scrap collides with HUD recycler, increase value of the scrap count
	}
}
