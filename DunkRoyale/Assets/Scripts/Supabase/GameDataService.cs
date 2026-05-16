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
    //Obtener el juego de vista marcador activo
    public IEnumerator GetActiveGame(Action<ActiveGameData> onSuccess, Action onEmpty)
    {
        //solamente se acepta 1
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
            //si se acaba la sesion o no tiene permisos marca error
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
}
