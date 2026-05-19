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
}