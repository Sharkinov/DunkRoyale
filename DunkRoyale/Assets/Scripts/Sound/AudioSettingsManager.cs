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
        // 1. Load saved values
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuted   = PlayerPrefs.GetInt("SFXMuted",   0) == 1;

        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float savedSFX   = PlayerPrefs.GetFloat("SFXVolume",   0.5f);

        lastMusicVolume = savedMusic;
        lastSFXVolume   = savedSFX;

        // 2. Add listeners BEFORE setting slider values
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        // 3. Set sliders — muted = 0, unmuted = saved value
        musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);

        musicSlider.value = musicMuted ? 0f : savedMusic;
        sfxSlider.value   = sfxMuted   ? 0f : savedSFX;

        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        // 4. Apply to SFXManager
        ApplyMusicVolume(musicMuted ? 0f : savedMusic);
        ApplySFXVolume(sfxMuted     ? 0f : savedSFX);

        // 5. Update UI
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
    PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0); 

    musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged); 

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

    musicSlider.onValueChanged.AddListener(OnMusicSliderChanged); 
    UpdateMuteIcons();
}

public void ToggleMuteSFX()
{
    sfxMuted = !sfxMuted;
    PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
    sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged); 

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

    sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged); 
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