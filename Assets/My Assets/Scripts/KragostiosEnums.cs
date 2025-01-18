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
    None,
    Cold,
    //Ice,
    Water,
    Earth,
    Fire,
    //Lava,
    Heat,
    Air,
    Electricty,
    Poison,
    Acid,
    Bacteria,
    Fungi,
    Plant,
    Virus,
    Radiation,
    Light,
    Psychic
}

public enum PhysicalDamage
{
    None,
    Bludgeoning,
    Slashing,
    Piercing
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

public enum Kingdoms
{
    Celestia, //  Heavenly people
    Grovchii, // Orc savage people
    Oshiania, // Water people
    Bioleb, // Jungle  people 
    SessPool, // Nasty swamp rat people
}

public enum Biomes
{
    Jungle,
    Swamp,
    RollingHills,
    GrassyFields,
    Mountains,
    Tundra,
    Glaciers,
    EverGreenForest,
    PerenialForest,
    Desert
}

public enum Buffs
{
Invisibility,
Shield,
Strengthen,
}

public enum Debuffs
{
Stun,
Shock,
StaminaDrain,
Chill,
Hot,
Proned,
Restrained,


}
}