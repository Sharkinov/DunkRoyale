using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPracticeMatch()
    {
        PlayerPrefs.SetInt("OpposingTeamId", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("[SceneLoader] ReturnToMainMenu called");
        SceneManager.LoadScene("Inicio");
    }

    public void PlayAgain()
    {
        Debug.Log("[SceneLoader] PlayAgain called");
        SceneManager.LoadScene("Game");
    }
}