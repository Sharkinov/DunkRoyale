using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public Text lakersScoreText;
    public Text npcScoreText;
    [Header("Créditos por resultado")]
    public int creditsOnWin  = 50;
    public int creditsOnLoss = 10;
    private int lakersScore = 0;
    private int npcScore = 0;
    private bool matchSaved  = false;

    [Header("Final Game Panel UI")]
    public Text finalScore1Text;  
    public Text finalScore2Text;  

    public int GetLakersScore() => lakersScore;
    public int GetNpcScore() => npcScore;

    void Awake()
    {
        Instance = this;
    }

    public void AddScore(bool isNPC)
    {
        if (isNPC)
        {
            npcScore++;
            npcScoreText.text = npcScore.ToString();
        }
        else
        {
            lakersScore++;
            lakersScoreText.text = lakersScore.ToString();
        }
    }
    public void OnGameEnd()
    {
        if (matchSaved) return;
        matchSaved = true;

        if (finalScore1Text != null) finalScore1Text.text = lakersScore.ToString();
        if (finalScore2Text != null) finalScore2Text.text = npcScore.ToString();

        StartCoroutine(SaveMatch());
    }

    public bool DidPlayerWin()
    {
        return lakersScore > npcScore;
    }

    IEnumerator SaveMatch()
    {
        bool won          = lakersScore > npcScore;
        int  credits      = won ? creditsOnWin : creditsOnLoss;
        int  opposingTeam = PlayerPrefs.GetInt("OpposingTeamId", 0); // 0 = sin partido activo

        // Construir JSON manualmente (Unity no tiene JsonUtility para serializar anónimos)
        var sb = new StringBuilder("{");
        sb.Append($"\"player_id\":\"{SupabaseConfig.Instance.UserId}\",");
        sb.Append($"\"player_score\":{lakersScore},");
        sb.Append($"\"npc_score\":{npcScore},");
        sb.Append($"\"won\":{(won ? "true" : "false")},");
        sb.Append($"\"credits_earned\":{credits}");
        if (opposingTeam > 0)
            sb.Append($",\"opposing_team_id\":{opposingTeam}");
        sb.Append("}");

        var url  = SupabaseConfig.Instance.SupabaseUrl + "/rest/v1/minigame_match";
        var body = Encoding.UTF8.GetBytes(sb.ToString());

        using var request = new UnityWebRequest(url, "POST");
        request.uploadHandler   = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Prefer", "return=minimal");

        foreach (var h in SupabaseConfig.Instance.GetHeaders())
            request.SetRequestHeader(h.Key, h.Value);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log($"[ScoreManager] Partida guardada — {(won ? "Victoria" : "Derrota")} " +
                      $"{lakersScore}-{npcScore}, +{credits} créditos");
        else
            Debug.LogError($"[ScoreManager] Error guardando partida: {request.error}");
    }
}