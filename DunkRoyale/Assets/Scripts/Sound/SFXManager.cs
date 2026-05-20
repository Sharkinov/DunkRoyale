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
    private AudioSource audioSource;

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

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainSound);
    }

    public void PlayGameMusic()
    {
        PlayMusic(gameSound);
    }

    public void SetMusicVolume(float value)
    {
        audioSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip == null || audioSource == null) return;

        audioSource.PlayOneShot(clip, volumeScale);
    }

    public void PlayClickButton()
    {
        PlaySFX(clickButton, clickButtonVolume);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.Play();
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

    public void PlayMarcadorFinal()
    {
        PlaySFX(marcadorFinal, 2f);
    }

    public void PlayGolpe()
    {
        PlaySFX(Golpe);
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}