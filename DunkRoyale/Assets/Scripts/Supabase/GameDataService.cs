using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameDataService : MonoBehaviour
{
    [Serializable]
    private class ActiveGameResponse
    {
        public ActiveGameData[] items;
    }

    [Serializable]
    private class PlayerStatsResponse
    {
        public UserDeckItem[] items;
    }

    public IEnumerator GetActiveGame(Action<ActiveGameData> onSuccess, Action onEmpty)
    {
        var url = SupabaseConfig.Instance.SupabaseUrl + "/rest/v1/v_marcador_activo?limit=1";
        using (var request = UnityWebRequest.Get(url))
        {
            var headers = SupabaseConfig.Instance.GetHeaders();
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            request.SetRequestHeader("Accept-Profile", "simulacion_juego");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GetActiveGame failed: {request.error}");
                yield break;
            }

            var responseText = request.downloadHandler.text;
            if (string.IsNullOrWhiteSpace(responseText) || responseText.Trim() == "[]")
            {
                onEmpty?.Invoke();
                yield break;
            }

            var wrappedJson = "{\"items\":" + responseText + "}";
            var response = JsonUtility.FromJson<ActiveGameResponse>(wrappedJson);
            if (response == null || response.items == null || response.items.Length == 0)
            {
                onEmpty?.Invoke();
                yield break;
            }

            onSuccess?.Invoke(response.items[0]);
        }
    }
    public IEnumerator GetPlayerStats(Action<UserDeckItem[]> onSuccess, Action onError)
    {
        var userId = SupabaseConfig.Instance.UserId;
        var url = SupabaseConfig.Instance.SupabaseUrl 
            + $"/rest/v1/deck?select=slot,card(*)&user_id=eq.{userId}&order=slot.asc";
        
        using (var request = UnityWebRequest.Get(url))
        {
            var headers = SupabaseConfig.Instance.GetHeaders();
            foreach (var header in headers)
                request.SetRequestHeader(header.Key, header.Value);

            yield return request.SendWebRequest();

            Debug.Log($"GetPlayerStats response: {request.downloadHandler.text}");

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GetPlayerStats failed: {request.error}");
                onError?.Invoke();
                yield break;
            }

            var json = "{\"items\":" + request.downloadHandler.text + "}";
            var response = JsonUtility.FromJson<PlayerStatsResponse>(json);
            onSuccess?.Invoke(response.items);
        }
    }
}
