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
        None,
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
        EdgeOfTheWorld
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
        Electricity,
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
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Grand,
        Legndary,
    }
    public enum ItemType
    {
        Quest,
        Potion,
        Scroll,
        Weapon,
        Armor,
    }

    public enum ItemSlot
    {
        None,
        Head,
        Shoulder,
        Chest,
        Gloves,
        Legs,
        Feet,
        RightHand,
        LeftHand,
        TwoHands
    }

    public enum Handedness
    {
        None,
        LeftHand,
        RightHand,
        TwoHands
    }

    public enum Combatants
    {
        Allies,
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
        Zactaal // intellectual kidnapping xenophobes
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
        ColdResistance,
        WaterResistance,
        EarthResistance,
        FireResistance,
        HeatResistance,
        AirResistance,
        ElectrictyResistance,
        PoisonResistance,
        AcidResistance,
        BacteriaResistance,
        FungiResistance,
        PlantResistance,
        VirusResistance,
        RadiationResistance,
        LightResistance,
        PsychiResistance,
    }

    public enum Debuffs
    {
        DamageOverTime,
        Stun,
        Shock,
        HealthDrain,
        StaminaDrain,
        ManaDrain,
        Chill,
        Hot,
        Melting,
        Proned,
        Restrained,
        Retarted, //retarted decreases the enemy accuracy and adds a chance the foe will attack their buddies or themselves.
        Charmed,
        InefficientHeart,
        InefficientStrength,
        InefficientSpirit,

        #region //inflict weakness
        ColdWeakness,
        WaterWeakness,
        EarthWeakness,
        FireWeakness,
        HeatWeakness,
        AirWeakness,
        ElectrictyWeakness,
        PoisonWeakness,
        AcidWeakness,
        BacteriaWeakness,
        FungiWeakness,
        PlantWeakness,
        VirusWeakness,
        RadiationWeakness,
        LightWeakness,
        PsychiWeakness,

        #endregion

        #region //physical weakness

        BludgeoningWeakness,
        SlashingWeakness,
        PiercingWeakness

        #endregion
    }

    public enum AlchemyTools
    {
        None,
        Beaker,
        AccurateWeights,
        Thermometer,
        Barometer,
        Pipette,
        Filter,
        Centrifuge,
        PressureChamber,
        OneWayValves,
        ArcSpring,
        Fire,
        IceBath,

    }

    public enum AbilityVars
    {
        AbilityName, // modable upon crafting, and examining in inventory
        Description, // modable upon crafting, and examining in inventory
        ElementType, // modable on weapons, armors and abbilities and with high knowledge of a given element 
        PhysicalType, // modable on weapons, armors and abbilities and with high knowledge of a given element
        AbilityCost, // modable on abilities, scaling with knowledge
        HealValue, // modable on usable items and abilities, scaling with average knowledge, improved greatly by knowledge of ether cluster power sources
        DamageValue, // modable on usable items and abilities, scaling with average knowledge, improved greatly by knowledge of ether cluster power sources
        TurnDuration, // modable on usable items and abilities, scaling with average knowledge, improved greatly by knowledge of ether cluster power sources. Very hard/heavy to improve
        Targets, // modable on usable items and abilities, scaling with average knowledge, improved greatly by knowledge of ether cluster power sources. Very hard/heavy to improve
        Summons, // not moddable, but craftable. scales with knowledge of elements. made possible by knowledge and crafting ability. 
        DamageOverTime, // modable on usable items and abilities, scaling with average knowledge, improved greatly by knowledge of ether cluster power sources
        SyphonPercentage,
        AbilityLevel,
        BuffEffects, // modable on usable items and abilities, scaling with elemental knowledge. Number of addable buffs? debuffs is capped by both elemental knowledge and powersource knowledge.
        DebuffEffects // ^^
    }

    public enum ItemVars
    {
        Value,
        Name,
        Description,
        Element,
        Damage,
        Heal,
        Debuffs,
        Buffs,
        ArmorBuffs,
        DamageReduction,
    }

}