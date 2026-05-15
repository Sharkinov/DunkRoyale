using UnityEngine;

[CreateAssetMenu(fileName = "SupabaseSettings", menuName = "Config/Supabase Settings")]
public class SupabaseSettings : ScriptableObject
{
	public string supabaseUrl;
	public string supabaseAnonKey;
}
