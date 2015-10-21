using UnityEngine;

namespace Soomla.Store.IAP
{
public class AudioManager : MonoBehaviour {
	
	public AudioSource audioSource;
	GameManager gameManager;
    public AudioClip[] audioClips;
	
	public AudioSource levelPassAudio;
	
	Animator anim;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("game manager").GetComponent<GameManager>();
		audioSource = this.GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();
		audioSource.clip = audioClips[0];
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.isReady)
		{
			//  levelPassAudio.PlayOneShot()
			anim.SetTrigger("levelpass");
		}
	}
	
	public void levelPass(){
			audioSource.PlayOneShot(audioClips[3],0.5f);	
	}
	
}
}
