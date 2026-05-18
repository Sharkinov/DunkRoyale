using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Prefabs")]
    public GameObject[] npcPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // puntos en el lado del NPC

    [Header("Config")]
    public float minElixir = 2f;   // elixir minimo para spawnear
    public float maxElixir = 10f;  // elixir maximo del NPC
    public float rechargeRate = 0.3f;      // was 0.8f
    public float minSpawnInterval = 5f;   // was 3f
    public float maxSpawnInterval = 8f;   // was 7f

    private float npcElixir;
    private float spawnTimer;

    void Start()
    {
        npcElixir = maxElixir;
        ScheduleNextSpawn();
    }

    void Update()
    {
        // recargar elixir del NPC
        if (npcElixir < maxElixir)
        {
            npcElixir += rechargeRate * Time.deltaTime;
            npcElixir = Mathf.Clamp(npcElixir, 0f, maxElixir);
        }

        // contar timer
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
            TrySpawnNPC();
    }

    void TrySpawnNPC()
    {
        // costo random entre 2, 4, 6
        int[] costs = { 2, 4, 6 };
        int cost = costs[Random.Range(0, costs.Length)];

        if (npcElixir >= cost)
        {
            npcElixir -= cost;
            SpawnNPC();
        }

        ScheduleNextSpawn();
    }

    void SpawnNPC()
    {
        if (npcPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject npc = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        var move = npc.GetComponent<CharacterMove>();
        if (move != null) move.isEnemy = true;

        // random NPC stats
        var combat = npc.GetComponent<PlayerCombat>();
        if (combat != null)
            combat.Initialize(
            Random.Range(30, 80),  // attack
            Random.Range(30, 80),  // defense
            Random.Range(1, 3)     // velocity — más lento que jugadores
        );
    }

    void ScheduleNextSpawn()
    {
        spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}