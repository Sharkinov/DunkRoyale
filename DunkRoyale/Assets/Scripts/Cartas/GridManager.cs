using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Zona para dropear al personaje")]
    public GameObject[] zones;

    [Header("Prefab del personaje a instanciar")]
    public GameObject characterPrefab;

    void Awake()
    {
        Instance=this;
        HideZones();
    }

    public void ShowZones()
    {
        foreach(var zone in zones)
            zone.SetActive(true);
    }

    public void HideZones()
    {
        foreach (var zone in zones)
            zone.SetActive(false);
    }
    public void OnZoneClicked(Vector3 worldPosition)
    {
        var cardManager = Object.FindAnyObjectByType<CardManager>();
        if (cardManager.selectedCardIndex == -1) return;

        Instantiate(characterPrefab, worldPosition, Quaternion.identity);

        cardManager.selectedCardIndex = -1;
        HideZones();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
