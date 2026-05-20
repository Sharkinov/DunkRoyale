using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPracticeMatch()
    {
        PlayerPrefs.SetInt("OpposingTeamId", 0);
        PlayerPrefs.Save();
        PlayGameMusicAndLoad();
    }

    public void ReturnToMainMenu()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayMainMenuMusic();

        SceneManager.LoadScene("Inicio"); 
    }

    public void PlayAgain()
    {
        // Reutiliza el OpposingTeamId que ya está guardado
        // Si era Play Match tenía un teamId > 0, si era Practice tenía 0
        PlayGameMusicAndLoad();
    }

    private void PlayGameMusicAndLoad()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayGameMusic();

        SceneManager.LoadScene("Game");
    }
}