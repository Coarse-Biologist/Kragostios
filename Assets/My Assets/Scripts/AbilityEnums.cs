using UnityEngine;
namespace AbilityEnums
{

    public enum Abilities
    // this enum class contains the names of all abilities so that if an ability should be added at some time manually for whatever reason (questing) there will be less likelihood of errors.
    // it is used in the class AbilityLibrary where a dict<Abilities, Ability_SO> is stored. This way abilities can be searched by comorehensible name to return the correct ability. 
    {
        Fireball,
        Melee,
        HealingTouch,
        DivineStrike,
        Push,
        ColdLight,
        BrainDamage,
        LavaPortal,
        GlobalCooling
    }
}