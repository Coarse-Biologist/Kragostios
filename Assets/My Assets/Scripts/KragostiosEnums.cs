using UnityEngine;
namespace KragostiosAllEnums
{


public enum AbilityCategories
{
    Heal,
    Attack,
    Summon,
    Buff,
    Debuff,
    Syphon, // heal and attack
    BuffDebuff,
    BuffHeal,
    DebuffAttack
   
}

public enum ResourceTypes
{
    Health,
    Mana,
    Stamina
}

public enum LocationType
{
    Barren,
    Hostile,
    City,
    Village,
    Trader,
    Healer,
    Campsite,
    ImpassableTerrain,
    HiddenTreasure,
}

public enum Elements
{
    Ice,
    Water,
    Earth,
    Fire,
    Lava,
    Air,
    Electricty,
    Poison,
    Acid,
    Bacteria,
    Virus,
    Radiation,
    Light
}

public enum Combatants 
{
    Player,
    Summon,
    Companion,
    Enemy
}

public enum Rewards
{
Gold,
Xp

}

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Brutal,
    Nightmare
}

public enum Directions
{
    North,
    East,
    South,
    West
}

}