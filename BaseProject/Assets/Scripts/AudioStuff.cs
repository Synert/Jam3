using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStuff : MonoBehaviour {
    public AudioClip[] AudioArray;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void playSound(int _clip, float defaultVal = 1, float defaultSound = 0.1f)
    {
        GetComponent<AudioSource>().volume = defaultSound;
        GetComponent<AudioSource>().pitch = defaultVal;
        GetComponent<AudioSource>().clip = AudioArray[_clip];
        GetComponent<AudioSource>().Play();
    }
}
