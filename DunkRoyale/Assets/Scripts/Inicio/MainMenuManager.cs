using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameButton;
    [SerializeField] private GameObject otherButton;
    [SerializeField] private GameDataService gameDataService;
    [SerializeField] private Text partidoContra;

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

        if (partidoContra != null)
            partidoContra.text = "vs. " + data.opposing_team_name;

        Debug.Log($"Partido activo encontrado");
    }

    private void OnNoGame()
    {
        Debug.Log("No hay partido activo");
    }
}
