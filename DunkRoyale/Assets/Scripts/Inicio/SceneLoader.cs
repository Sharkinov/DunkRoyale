using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPracticeMatch()
    {
        SceneManager.LoadScene("Game");
    }
}