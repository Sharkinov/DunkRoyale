using System;
using UnityEngine;

[Serializable]
public class SpriteInfo
{
    public string name;
}

[Serializable]
public class PlayerStats
{
    public string card_id;
    public string player_name;
    public int attack;
    public int defense;
    public int velocity;
    public int gatorade_cost;
    public int rarity_id;
    public int sprite_id;
    public string web_url;
    public string pixel_url;
    public SpriteInfo sprite;
}

[Serializable]
public class UserDeckItem
{
    public int slot;
    public PlayerStats card;
}