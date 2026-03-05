using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;

    [Header("Scene Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip hubMusic;
    [SerializeField] private AudioClip Room1;
    [SerializeField] private AudioClip Room2;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip BadEnding;
    [SerializeField] private AudioClip GoodEnding;
    [SerializeField] private AudioClip BestEnding;


    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip clickSound;

    public void PlayClick()
    {
        if (clickSound != null)
            sfxSource.PlayOneShot(clickSound);
    }

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadVolume();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    private void PlaySceneMusic(string sceneName)
    {
        AudioClip newClip = GetClipForScene(sceneName);

        if (newClip == null)
            return;

        if (musicSource.clip == newClip)
            return;

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    private AudioClip GetClipForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Menu":
                return menuMusic;

            case "Hub":
                return hubMusic;

            case "Room1":
                return Room1;

            case "Room2":
                return Room2;

            case "Boss":
                return bossMusic;

            case "BadEnding":
                return BadEnding;

            case "GoodEnding":
                return GoodEnding;

            case "BestEnding":
                return BestEnding;

            default:
                return null;
        }
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0.0001f)
            audioMixer.SetFloat("Master", -80f);
        else
            audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(MASTER_KEY, value);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0.0001f)
            audioMixer.SetFloat("Music", -80f);
        else
            audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0001f)
            audioMixer.SetFloat("SFX", -80f);
        else
            audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void LoadVolume()
    {
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MASTER_KEY, 1f);
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_KEY, 1f);
}