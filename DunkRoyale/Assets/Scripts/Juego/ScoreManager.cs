using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public Text lakersScoreText;
    public Text npcScoreText;

    private int lakersScore = 0;
    private int npcScore = 0;

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
}