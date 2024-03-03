using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } //Use to call this class from another script and this script can not be modify out side this class.
    public static AudioSource audioSource;
    private AudioSource source;

    [Header("Sound part")]
    [SerializeField] private AudioClip baseBGM;
    [SerializeField] private AudioClip bossBGM;
    
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = EconomyManager.soundVolume;
        audioSource.clip = baseBGM;
        audioSource.Play();
    }

    //Play one time.
    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    public void SetSoundVolume()
    {
        audioSource.volume = EconomyManager.soundVolume;
    }

    public void SetBossBGM(bool _b)
    {
        audioSource.clip = baseBGM;
        if (_b)
        {
            audioSource.clip = bossBGM;
        }
        audioSource.Play();
    }
}