using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    public AudioSource ButtonPress;
    public AudioClip Button;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
        ButtonPress.clip = Button;
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.green;
        ButtonPress.Play();
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
