using System;
using System.Collections.Generic;
using UnityEngine;

public class SupabaseConfig : MonoBehaviour
{
    public static SupabaseConfig Instance {get; private set;}
    [SerializeField]
    private SupabaseSettings supabaseSettings;

    public string UserJwtToken { get; private set; }
    public string UserId { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //Segun que lo tienes qye cambiar cada hora
        #if UNITY_EDITOR
        SetSession("eyJhbGciOiJFUzI1NiIsImtpZCI6IjAyYzcyMmY1LTNkOGEtNGI5Ni1hY2FmLTQ4MWVmMTVlYjMxMSIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3B0YmNveGFndXZid3ByeGR1bmR6LnN1cGFiYXNlLmNvL2F1dGgvdjEiLCJzdWIiOiJhOGMzYjAxZS1mZWJjLTQ3MGMtYWFjYS0zN2E2NzlmZWUyZGIiLCJhdWQiOiJhdXRoZW50aWNhdGVkIiwiZXhwIjoxNzc4ODc4MzUwLCJpYXQiOjE3Nzg4NzQ3NTAsImVtYWlsIjoibGFrZXJmYW5AbGFrZXJzY291cnQuY29tIiwicGhvbmUiOiIiLCJhcHBfbWV0YWRhdGEiOnsicHJvdmlkZXIiOiJlbWFpbCIsInByb3ZpZGVycyI6WyJlbWFpbCJdfSwidXNlcl9tZXRhZGF0YSI6eyJlbWFpbF92ZXJpZmllZCI6dHJ1ZX0sInJvbGUiOiJhdXRoZW50aWNhdGVkIiwiYWFsIjoiYWFsMSIsImFtciI6W3sibWV0aG9kIjoicGFzc3dvcmQiLCJ0aW1lc3RhbXAiOjE3Nzg4NzQ3NTB9XSwic2Vzc2lvbl9pZCI6ImMwY2NkOTBkLTg1MzktNDk1NC05ZDJmLWZmZjNlZjM1ZWNhZiIsImlzX2Fub255bW91cyI6ZmFsc2V9.RhrwnVH4CNKb4ct6SAT7lwIBOqmoqZtPiLRDMoAyQgP-2erTYA9IAGd-OOH2OkQBYcHJWSdqPjOnacJuT2jqcw", 
                    "a8c3b01e-febc-470c-aaca-37a679fee2db");
        #endif
    }
    
    public void SetSession(string jwt, string userId)
    {
        UserJwtToken = jwt;
        UserId = userId;
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(UserJwtToken);

    public Dictionary<string, string> GetHeaders()
    {
        var headers = new Dictionary<string, string>();
        var anonKey = supabaseSettings != null ? supabaseSettings.supabaseAnonKey : string.Empty;
        headers["apikey"] = anonKey;

        var authorization = !string.IsNullOrEmpty(UserJwtToken) ? "Bearer " + UserJwtToken : "Bearer " + anonKey;
        headers["Authorization"] = authorization;

        headers["Content-Type"] = "application/json";
        return headers;
    }

    public string SupabaseUrl => supabaseSettings != null ? supabaseSettings.supabaseUrl : string.Empty;

}
