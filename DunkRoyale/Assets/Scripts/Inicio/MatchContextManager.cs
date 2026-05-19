using UnityEngine;

public class MatchContextManager : MonoBehaviour
{
    public GameObject practiceCourtObject;  // arrastra LakersCourtBG_0
    public GameObject realMatchCourtObject; // arrastra Court_BG_0

    void Start()
    {
        int teamId = PlayerPrefs.GetInt("OpposingTeamId", 0);
        bool isRealMatch = teamId > 0;

        if (practiceCourtObject != null)
            practiceCourtObject.SetActive(!isRealMatch);

        if (realMatchCourtObject != null)
            realMatchCourtObject.SetActive(isRealMatch);
    }
}