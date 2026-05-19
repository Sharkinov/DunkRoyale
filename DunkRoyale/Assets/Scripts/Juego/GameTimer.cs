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

    void Start()
    {
        timeRemaining = matchDuration;
        if (finalGamePanel != null) finalGamePanel.SetActive(false);
        if (victoryPanel != null)   victoryPanel.SetActive(false);
        if (loserPanel != null)     loserPanel.SetActive(false);
    }

    void Update()
    {
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

        // Parar spawner
        if (npcSpawner != null)
            npcSpawner.enabled = false;

        // Destruir todos los personajes en cancha
        var allObjects = FindObjectsOfType<PlayerCombat>();
        foreach (var obj in allObjects)
            Destroy(obj.gameObject);

        // Guardar partida
        ScoreManager.Instance.OnGameEnd();

        // Mostrar panel final
        StartCoroutine(ShowEndPanel());
    }

    IEnumerator ShowEndPanel()
    {
        yield return new WaitForSeconds(0.5f);

        if (finalGamePanel != null)
            finalGamePanel.SetActive(true);

        int lakersScore = ScoreManager.Instance.GetLakersScore();
        int npcScore = ScoreManager.Instance.GetNpcScore();

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
    }
}