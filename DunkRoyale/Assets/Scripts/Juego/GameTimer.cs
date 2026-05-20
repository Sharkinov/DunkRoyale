using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Header("Timer")]
    public float matchDuration = 60f; // 1 minuto
    public Text timerText;

    [Header("End Game")]
    public GameObject finalGamePanel;
    public GameObject victoryPanel;
    public GameObject loserPanel;
    public GameObject tiePanel;

    [Header("References")]
    public NPCSpawner npcSpawner;

    private float timeRemaining;
    private bool gameEnded = false;
     private bool gameStarted = false;

    public Text creditsEarnedText;

    void Start()
    {
        timeRemaining = matchDuration;
        if (finalGamePanel != null) finalGamePanel.SetActive(false);
        if (victoryPanel != null)   victoryPanel.SetActive(false);
        if (loserPanel != null)     loserPanel.SetActive(false);
    }

    public void StartGame() // ← CountdownManager llama esto
    {
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted) return;
        if (gameEnded) return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0f);

        // Actualizar texto del timer
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        if (timerText != null)
            timerText.text = $"{minutes}:{seconds:00}";

        if (timeRemaining <= 0f)
            EndGame();
    }

    void EndGame()
    {
        gameEnded = true;

        if (SFXManager.Instance != null)
        SFXManager.Instance.PlayMarcadorFinal();

        var audioSettings = FindObjectOfType<AudioSettingsManager>();
        if (audioSettings != null)
            audioSettings.ClosePanel();

        if (npcSpawner != null)
            npcSpawner.enabled = false;

        var elixirBar = FindObjectOfType<ElixirBar>();
        if (elixirBar != null)
            elixirBar.StopRecharge();

        // Guardar partida
        ScoreManager.Instance.OnGameEnd();

        // Mostrar panel primero
        StartCoroutine(ShowEndPanel());
    }

    IEnumerator ShowEndPanel()
    {
        if (finalGamePanel != null)
            finalGamePanel.SetActive(true);

        int lakersScore = ScoreManager.Instance.GetLakersScore();
        int npcScore = ScoreManager.Instance.GetNpcScore();

        // Mostrar créditos ganados
        if (creditsEarnedText != null)
        {
            int credits = ScoreManager.Instance.CalculateCredits();
            creditsEarnedText.text = $"+{credits}";
        }

        if (lakersScore == npcScore)
        {
            if (tiePanel != null) tiePanel.SetActive(true);
        }
        else if (ScoreManager.Instance.DidPlayerWin())
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
        else
        {
            if (loserPanel != null) loserPanel.SetActive(true);
        }

        yield return new WaitForSeconds(0.3f);
        var allObjects = FindObjectsOfType<PlayerCombat>();
        foreach (var obj in allObjects)
            Destroy(obj.gameObject);
    }
}