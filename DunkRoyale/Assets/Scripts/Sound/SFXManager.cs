using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioClip mainSound;
    [SerializeField] private AudioClip gameSound;
    [SerializeField] private AudioClip clickButton;
    [SerializeField] private AudioClip recibirGolpe;

    private AudioSource audioSource;

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

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}