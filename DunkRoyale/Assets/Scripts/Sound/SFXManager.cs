using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private AudioClip mainSound;
    [SerializeField] private AudioClip gameSound;
    [SerializeField] private AudioClip clickButton;
    [SerializeField, Range(0f, 1f)] private float clickButtonVolume = 1f;
    [SerializeField] private AudioClip Golpe;
    [SerializeField] private AudioClip canasta;
    [SerializeField] private AudioClip readyforthis;
    [SerializeField] private AudioClip onetwo;
    [SerializeField] private AudioClip ponermonoencancha;
    [SerializeField] private AudioClip selectTarjeta;
    [SerializeField] private AudioClip marcadorFinal;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private float sfxVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if (Instance.mainSound == null) Instance.mainSound = mainSound;
            if (Instance.gameSound == null) Instance.gameSound = gameSound;
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 1f;

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainSound);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameSound);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volumeScale * sfxVolume);
    }

    public void PlayClickButton()
    {
        PlaySFX(clickButton, clickButtonVolume);
    }

    public void PlayOnetwo()
    {
        PlaySFX(onetwo);
    }

    public void PlayReadyForThis()
    {
        PlaySFX(readyforthis, 3f);
    }

    public void PlaySelectTarjeta()
    {
        PlaySFX(selectTarjeta, 7f);
    }

    public void PlayPonerMonoEnCancha()
    {
        PlaySFX(ponermonoencancha, 5f);
    }

    public void PlayCanasta()
    {
        PlaySFX(canasta, 2f);
    }

    public void PlayMarcadorFinal()
    {
        PlaySFX(marcadorFinal, 2f);
    }

    public void PlayGolpe()
    {
        PlaySFX(Golpe);
    }
}