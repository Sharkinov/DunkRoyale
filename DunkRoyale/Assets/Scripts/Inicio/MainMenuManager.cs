using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameButton;
    [SerializeField] private GameObject otherButton;
    [SerializeField] private GameDataService gameDataService;

    void Start()
    {
        if (gameButton != null)
        {
            gameButton.SetActive(false);
        }

        if (gameDataService != null)
        {
            StartCoroutine(gameDataService.GetActiveGame(OnGameFound, OnNoGame));
        }
    }

    private void OnGameFound(ActiveGameData data)
    {
        if (gameButton != null)
        {
            gameButton.SetActive(true);
        }

        Debug.Log($"Partido activo encontrado: {JsonUtility.ToJson(data)}");
    }

    private void OnNoGame()
    {
        Debug.Log("No hay partido activo");
    }
}
