using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Zona para dropear al personaje")]
    public GameObject[] zones;

    [Header("Prefabs de personajes")]
    public GameObject[] characterPrefabs;

    [Header("Zona donde NO se dropea")]
    public GameObject zonanodrop;

    void Awake()
    {
        Instance=this;
        HideZones();
    }

    public void ShowZones()
    {
        foreach(var zone in zones)
            zone.SetActive(true);

        zonanodrop.SetActive(true);
    }

    public void HideZones()
    {
        foreach (var zone in zones)
            zone.SetActive(false);

        zonanodrop.SetActive(false);
    }

    GameObject GetPrefabForCharacter(string spriteName)
    {
        foreach (var prefab in characterPrefabs)
        {
            if (prefab.name == spriteName)
                return prefab;
        }
        
        Debug.LogWarning($"Prefab not found for: {spriteName}");
        return characterPrefabs[0];
    }

    public void OnZoneClicked(Vector3 worldPosition)
    {
        var cardManager = Object.FindAnyObjectByType<CardManager>();
        if (cardManager.selectedCardIndex == -1) return;

        var selectedCard = cardManager.loadedCards[cardManager.selectedCardIndex].card;
        string spriteName = selectedCard.sprite?.name.Replace(" ", "");
        Debug.Log($"Looking for prefab: '{spriteName}'");

        GameObject prefabToSpawn = GetPrefabForCharacter(spriteName);
        Debug.Log($"Found prefab: {prefabToSpawn?.name}");
        Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);

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
