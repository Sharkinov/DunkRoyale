using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject audioSettingsPanel;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Mute Buttons")]
    public GameObject audio1;  // icono normal music
    public GameObject mute1;   // icono mute music
    public GameObject audio2;  // icono normal sfx
    public GameObject mute2;   // icono mute sfx

    private bool musicMuted = false;
    private bool sfxMuted = false;
    private float lastMusicVolume;
    private float lastSFXVolume;

    void Start()
    {
        Debug.Log($"[AudioSettings] Init — musicMuted={musicMuted}");
        // Cargar valores guardados
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        lastMusicVolume = savedMusic;
        lastSFXVolume = savedSFX;

        ApplyMusicVolume(savedMusic);
        ApplySFXVolume(savedSFX);

        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        UpdateMuteIcons();

        if (audioSettingsPanel != null)
            audioSettingsPanel.SetActive(false);
    }

    // Botón audio (abrir panel)
    public void OpenPanel()
    {
        if (audioSettingsPanel != null)
            audioSettingsPanel.SetActive(true);
    }

    // Botón close
    public void ClosePanel()
    {
        if (audioSettingsPanel != null)
            audioSettingsPanel.SetActive(false);
    }

    // Slider Music
    void OnMusicSliderChanged(float value)
    {
        lastMusicVolume = value;
        musicMuted = false;
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
        UpdateMuteIcons();
    }

    // Slider SFX
    void OnSFXSliderChanged(float value)
    {
        lastSFXVolume = value;
        sfxMuted = false;
        ApplySFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
        UpdateMuteIcons();
    }

    // Botón mute Music (audio1)
    public void ToggleMuteMusic()
{
    musicMuted = !musicMuted;
    musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged); // 👈 remove first

    if (musicMuted)
    {
        lastMusicVolume = musicSlider.value;
        musicSlider.value = 0f;
        ApplyMusicVolume(0f);
    }
    else
    {
        musicSlider.value = lastMusicVolume;
        ApplyMusicVolume(lastMusicVolume);
    }

    musicSlider.onValueChanged.AddListener(OnMusicSliderChanged); // 👈 re-add after
    UpdateMuteIcons();
}

public void ToggleMuteSFX()
{
    sfxMuted = !sfxMuted;
    sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged); // 👈 remove first

    if (sfxMuted)
    {
        lastSFXVolume = sfxSlider.value;
        sfxSlider.value = 0f;
        ApplySFXVolume(0f);
    }
    else
    {
        sfxSlider.value = lastSFXVolume;
        ApplySFXVolume(lastSFXVolume);
    }

    sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged); // 👈 re-add after
    UpdateMuteIcons();
}

    void ApplyMusicVolume(float value)
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.SetMusicVolume(value);
    }

    void ApplySFXVolume(float value)
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.SetSFXVolume(value);
    }

    void UpdateMuteIcons()
    {
        if (audio1 != null) audio1.SetActive(!musicMuted);
        if (mute1 != null)  mute1.SetActive(musicMuted);
        if (audio2 != null) audio2.SetActive(!sfxMuted);
        if (mute2 != null)  mute2.SetActive(sfxMuted);
    }
}