using UnityEngine;

namespace Soomla.Store.IAP
{
    public class AudioManager : MonoBehaviour
    {

        public AudioSource backgroundMusicAudioSource;
        public AudioSource gameOverAudioSource;
        public AudioSource levelPassAudioSource;
        GameManager gameManager;
        public AudioClip[] audioClips;
        public AudioClip passage;
        public AudioSource levelPassAudio;

        public static AudioManager instance = null;

        Animator anim;
        // Use this for initialization

        void Start()
        {
            gameManager = GameManager.instance;
            backgroundMusicAudioSource = this.GetComponent<AudioSource>();
            anim = this.GetComponent<Animator>();
            backgroundMusicAudioSource.clip = audioClips[0];
            backgroundMusicAudioSource.Play();
        }
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (gameManager.frogSmashed == gameManager.frogCounter)
            {
                //  levelPassAudio.PlayOneShot()
                // anim.SetTrigger("levelpass");
                //  backgroundMusicAudioSource.PlayOneShot(audioClips[3],1.5f);
                // backgroundMusicAudioSource.PlayOneShot(audioClips[3],0.5f);
                levelPass();
                Debug.Log("Pass");
            }

        }

        public void levelPass()
        {
            levelPassAudioSource.PlayOneShot(audioClips[3], 0.5f);

        }
        public void GameOver(){
            backgroundMusicAudioSource.Stop();
            gameOverAudioSource.Play();
        }

    }
}
