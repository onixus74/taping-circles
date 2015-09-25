using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	public AudioSource audioSource;

    public AudioClip[] audioClips;
	// Use this for initialization
	void Start () {
		audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = audioClips[0];
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
