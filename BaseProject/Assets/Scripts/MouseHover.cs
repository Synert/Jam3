using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    public AudioSource ButtonPress;
    public AudioClip Button;

    public Color hoverColour;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
        ButtonPress.clip = Button;
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = new Color(1.0f,0.5f,0.0f);
        ButtonPress.Play();
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
