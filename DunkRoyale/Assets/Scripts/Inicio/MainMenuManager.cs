using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameButton;
    [SerializeField] private GameObject otherButton;
    [SerializeField] private GameDataService gameDataService;
    [SerializeField] private Text partidoContra;
    //Si no encuentra juego desactiva el boton y solamente deja el practice game
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
    //Si encuentra un juego lo pone en el boton 
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
    //Agregar más lógica para identificar si viene del match o del practice!
    public void PasarAJuego()
    {
        SceneManager.LoadScene("Game");
    }
}
