using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Prefabs")]
    public GameObject[] npcPrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // puntos en el lado del NPC

    [Header("Fallback Config (si Supabase falla)")]
    public float fallbackSpawnMin    = 7f;
    public float fallbackSpawnMax    = 11f;
    public int   fallbackMaxOnCourt  = 3;
    public int   fallbackAttack      = 50;
    public int   fallbackDefense     = 50;
    public int   fallbackVelocity    = 2;

    [Header("Elixir NPC")]
    // public float minElixir = 2f;   // elixir minimo para spawnear
    public float maxElixir = 10f;  // elixir maximo del NPC
    public float rechargeRate = 0.3f;      // was 0.8f
    // public float minSpawnInterval = 5f;   // was 3f
    // public float maxSpawnInterval = 8f;   // was 7f

    private NPCArchetype[]  archetypes;
    private float spawnIntervalMin;
    private float spawnIntervalMax;
    private int maxNpcOnCourt;
    private float npcElixir;
    private float spawnTimer;
    private bool configLoaded = false;
    private readonly List<GameObject> activeNpcs = new();

    void ApplyFallbackConfig()
    {
        spawnIntervalMin = fallbackSpawnMin;
        spawnIntervalMax = fallbackSpawnMax;
        maxNpcOnCourt    = fallbackMaxOnCourt;
    }

    void Start()
    {
        npcElixir = maxElixir;
        ApplyFallbackConfig();
        ScheduleNextSpawn();
        var cardManager = FindAnyObjectByType<CardManager>();
        StartCoroutine(LoadConfigWhenReady(cardManager));
    }
    IEnumerator LoadConfigWhenReady(CardManager cardManager)
    {
        // esperar al deck para que no falle
        float waited = 0f;
        while ((cardManager == null || cardManager.loadedCards == null) && waited < 10f)
        {
            yield return new WaitForSeconds(0.5f);
            waited += 0.5f;
        }

        int playerPower = CalculatePlayerPower(cardManager);
        Debug.Log($"[NPCSpawner] Player power calculado: {playerPower}");

        //equipo rival
        int teamId = PlayerPrefs.GetInt("OpposingTeamId", 1); 

        var dataService = FindAnyObjectByType<GameDataService>();
        if (dataService == null)
        {
            Debug.LogWarning("[NPCSpawner] No GameDataService encontrado, usando fallback.");
            yield break;
        }

        yield return dataService.GetNpcConfig(
            teamId,
            playerPower,
            onSuccess: (data) =>
            {
                archetypes = data;
                if (archetypes.Length > 0)
                {
                    spawnIntervalMin = archetypes[0].spawn_interval_min;
                    spawnIntervalMax = archetypes[0].spawn_interval_max;
                    maxNpcOnCourt    = archetypes[0].max_npc_on_court;
                    configLoaded     = true;
                    Debug.Log($"[NPCSpawner] Config cargada: {archetypes[0].difficulty_label}, " +
                              $"max={maxNpcOnCourt}, spawn={spawnIntervalMin}-{spawnIntervalMax}s");
                }
            },
            onError: () =>
            {
                Debug.LogWarning("[NPCSpawner] No se pudo cargar config, usando fallback.");
            }
        );
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
    int CalculatePlayerPower(CardManager cardManager)
    {
        if (cardManager == null || cardManager.loadedCards == null || cardManager.loadedCards.Length == 0)
            return 50; // poder neutro si no hay info

        float total = 0f;
        foreach (var item in cardManager.loadedCards)
        {
            if (item?.card == null) continue;
            total += (item.card.attack + item.card.defense) / 2f;
        }
        return Mathf.RoundToInt(total / cardManager.loadedCards.Length);
    }
    void TrySpawnNPC()
    {
        ScheduleNextSpawn();

        // Respetar límite de cancha
        if (activeNpcs.Count >= maxNpcOnCourt) return;

        // Costo basado en arquetipo seleccionado
        NPCArchetype chosen = PickArchetype();
        int cost = chosen?.gatorade_cost ?? 3;

        if (npcElixir < cost) return;

        npcElixir -= cost;
        SpawnNPC(chosen);
    }
    void SpawnNPC(NPCArchetype archetype)
    {
        if (spawnPoints.Length == 0) return;

        // Seleccionar prefab por nombre de sprite, o el primero como fallback
        GameObject prefab = GetPrefabByName(archetype?.jersey_sprite_name) ?? 
                            (npcPrefabs.Length > 0 ? npcPrefabs[0] : null);
        if (prefab == null)
        {
            Debug.LogWarning("[NPCSpawner] Sin prefabs configurados.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject npc = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        var move = npc.GetComponent<CharacterMove>();
        if (move != null) move.isEnemy = true;

        var combat = npc.GetComponent<PlayerCombat>();
        if (combat != null)
        {
            int atk = archetype?.final_attack  ?? fallbackAttack;
            int def = archetype?.final_defense ?? fallbackDefense;
            int vel = archetype?.final_velocity ?? fallbackVelocity;
            combat.Initialize(atk, def, vel);
        }

        activeNpcs.Add(npc);
        Debug.Log($"[NPCSpawner] Spawneado: {archetype?.archetype_name ?? "Fallback"} " +
                  $"({activeNpcs.Count}/{maxNpcOnCourt} en cancha)");
    }
    // void SpawnNPC()
    // {
    //     if (npcPrefabs.Length == 0 || spawnPoints.Length == 0) return;

    //     GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
    //     Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

    //     GameObject npc = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

    //     var move = npc.GetComponent<CharacterMove>();
    //     if (move != null) move.isEnemy = true;

    //     // random NPC stats
    //     var combat = npc.GetComponent<PlayerCombat>();
    //     if (combat != null)
    //         combat.Initialize(
    //         Random.Range(30, 80),  // attack
    //         Random.Range(30, 80),  // defense
    //         Random.Range(1, 3)     // velocity — más lento que jugadores
    //     );
    // }

    void ScheduleNextSpawn()
    {
        spawnTimer = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    GameObject GetPrefabByName(string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName)) return null;
        foreach (var prefab in npcPrefabs)
        {
            if (prefab != null && prefab.name == spriteName)
                return prefab;
        }
        return null;
    }

    NPCArchetype PickArchetype()
    {
        if (archetypes == null || archetypes.Length == 0) return null;

        int totalWeight = 0;
        foreach (var a in archetypes) totalWeight += a.weight;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;
        foreach (var a in archetypes)
        {
            cumulative += a.weight;
            if (roll < cumulative) return a;
        }

        return archetypes[0];
    }
}