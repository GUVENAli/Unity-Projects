using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance { get; private set; }
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private const string MUSIC_KEY = "MusicVol";
    private const string SFX_KEY = "SFXVol";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float savedMusic = PlayerPrefs.GetFloat(MUSIC_KEY, 0.5f);
        float savedSFX = PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicSource.volume = sliderValue;
        PlayerPrefs.SetFloat(MUSIC_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxSource.volume = sliderValue;
        PlayerPrefs.SetFloat(SFX_KEY, sliderValue);
        PlayerPrefs.Save();
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
