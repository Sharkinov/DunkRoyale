using UnityEngine;

public class JerseyController : MonoBehaviour
{
    [System.Serializable]
    public struct JerseyEntry
    {
        public string jerseySpriteName; // debe coincidir exacto: "GenericPlayers", "WarriorsPlayers", etc.
        public GameObject jerseyObject;
    }

    public JerseyEntry[] jerseys;

    public void ApplyJersey(string spriteName)
    {
        // Si no hay nombre, usar genérico
        if (string.IsNullOrEmpty(spriteName)) spriteName = "GenericPlayers";
        Debug.Log($"[JerseyController] Aplicando jersey: {spriteName}, entries: {jerseys.Length}");

        foreach (var j in jerseys)
            if (j.jerseyObject != null)
                j.jerseyObject.SetActive(false);

        bool found = false;
        foreach (var j in jerseys)
        {
            if (j.jerseySpriteName == spriteName && j.jerseyObject != null)
            {
                j.jerseyObject.SetActive(true);
                found = true;
                break;
            }
        }

        // Fallback a GenericPlayers si no encontró el sprite
        if (!found)
        {
            foreach (var j in jerseys)
            {
                if (j.jerseySpriteName == "GenericPlayers" && j.jerseyObject != null)
                {
                    j.jerseyObject.SetActive(true);
                    break;
                }
            }
        }
    }
}