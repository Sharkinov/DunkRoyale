using UnityEditor.Build.Content;
using UnityEngine;

public class SupabaseConfig : MonoBehaviour
{
    public static SupabaseConfig Instance {get; private set;}
    [SerializeField]
    private SupabaseSettings supabaseSettings;
    public string UserJwtToken {get; private set;}
    public string UserId {get; private set;}
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSession(string jwt, string userId)
    {
        UserJwtToken = jwt;
        UserId = userId;
    }
}
