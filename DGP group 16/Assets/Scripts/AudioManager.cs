using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Source---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---Audio Clip---")]
    public AudioClip background;
    public AudioClip nextlevelIn;
    public AudioClip nextlevelOut;
    public AudioClip pickupItem;
    public AudioClip dropItem;
    public AudioClip placeItem;
    public AudioClip bellSound;
    public AudioClip playerJump;
    public AudioClip error;
    public AudioClip screwdriverUse;
    public AudioClip buttonPress;
    public AudioClip buttonRelease;
    public AudioClip doorClose;
    public AudioClip doorOpen;
    //public AudioClip ;

    private void Awake()
    {
        // Zorg ervoor dat er maar één AudioManager is
        var existingManagers = FindObjectsByType<AudioManager>(FindObjectsSortMode.None);
        if (existingManagers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = background;
            musicSource.Play();
        } 
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
