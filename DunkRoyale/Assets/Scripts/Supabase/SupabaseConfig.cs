using System;
using System.Collections.Generic;
using UnityEngine;

public class SupabaseConfig : MonoBehaviour
{
    public static SupabaseConfig Instance {get; private set;}
    [SerializeField]
    private SupabaseSettings supabaseSettings;
    // Se obtiene el user token (session) y user id
    public string UserJwtToken { get; private set; }
    public string UserId { get; private set; }
    // Llamado desde React via sendMessage
    public void ReceiveSessionFromReact(string jsonData)
    {
        var parsed = JsonUtility.FromJson<SessionData>(jsonData);
        SetSession(parsed.jwt, parsed.userId);
    }

    [System.Serializable]
    private class SessionData
    {
        public string jwt;
        public string userId;
    }
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
        //Esto es para las pruebas dentro de unity
        #if UNITY_EDITOR
        SetSession( "eyJhbGciOiJFUzI1NiIsImtpZCI6IjAyYzcyMmY1LTNkOGEtNGI5Ni1hY2FmLTQ4MWVmMTVlYjMxMSIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL3B0YmNveGFndXZid3ByeGR1bmR6LnN1cGFiYXNlLmNvL2F1dGgvdjEiLCJzdWIiOiJhOGMzYjAxZS1mZWJjLTQ3MGMtYWFjYS0zN2E2NzlmZWUyZGIiLCJhdWQiOiJhdXRoZW50aWNhdGVkIiwiZXhwIjoxNzc5MzExMjQwLCJpYXQiOjE3NzkzMDc2NDAsImVtYWlsIjoibGFrZXJmYW5AbGFrZXJzY291cnQuY29tIiwicGhvbmUiOiIiLCJhcHBfbWV0YWRhdGEiOnsicHJvdmlkZXIiOiJlbWFpbCIsInByb3ZpZGVycyI6WyJlbWFpbCJdfSwidXNlcl9tZXRhZGF0YSI6eyJlbWFpbF92ZXJpZmllZCI6dHJ1ZX0sInJvbGUiOiJhdXRoZW50aWNhdGVkIiwiYWFsIjoiYWFsMSIsImFtciI6W3sibWV0aG9kIjoicGFzc3dvcmQiLCJ0aW1lc3RhbXAiOjE3NzkzMDc2NDB9XSwic2Vzc2lvbl9pZCI6ImNlZmViODRlLWNmOTQtNDg4MS04ZjFhLWExZDQyYTk3YTEyMSIsImlzX2Fub255bW91cyI6ZmFsc2V9.p65l8yegL5iQLc41DGldbyOtdXaXqgwUVWtvelv6anBOthsho39E0IF-7_H1nLUW8tJx9euuqJzL7VSaFsMijw", 
                    "a8c3b01e-febc-470c-aaca-37a679fee2db");
        #endif
    }
    //Setear la session
    public void SetSession(string jwt, string userId)
    {
        UserJwtToken = jwt;
        UserId = userId;
    }
    //Para saber si esta loggeado
    public bool IsLoggedIn => !string.IsNullOrEmpty(UserJwtToken);
    //Conexion con supabase
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