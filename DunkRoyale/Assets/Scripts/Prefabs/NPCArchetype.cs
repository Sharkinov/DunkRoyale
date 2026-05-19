using System;
using UnityEngine;
[Serializable]
public class NPCArchetype
{
    public int npc_archetype_id;
    public int sim_team_id; //es el opposing team en el marcador
    public string jersey_sprite_name;
    public string archetype_name;
    public int weight;
    public int gatorade_cost;
    public int final_attack;
    public int final_defense;
    public int final_velocity;
    public float spawn_interval_min;
    public float spawn_interval_max;
    public int max_npc_on_court;
    public string difficulty_label;
    public int player_power_min;
    public int player_power_max;
}

[Serializable]
public class NpcArchetypeResponse
{
    public NPCArchetype[] items;
}