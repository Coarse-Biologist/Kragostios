using System.Collections.Generic;

using UnityEngine;

public class Vocabulary
{
    public static string GetRandomVillainousAdjective()
    {
        string[] villainousAdjectives = {
    "Malevolent", "Nefarious", "Insidious", "Devious", "Ruthless", "Vile", "Sinister", "Diabolical",
    "Treacherous", "Wicked", "Corrupt", "Venomous", "Depraved", "Morbid", "Menacing", "Ominous",
    "Wretched", "Heinous", "Fiendish", "Malicious", "Scheming", "Brutal", "Ghastly", "Monstrous",
    "Cruel", "Barbaric", "Atrocious", "Shadowy", "Venomous", "Callous", "Treasonous", "Lecherous",
    "Repugnant", "Sordid", "Grotesque", "Abhorrent", "Unholy", "Eerie", "Spectral", "Grim",
    "Ghostly", "Tyrannical", "Infernal", "Macabre", "Deviant", "Loathsome", "Putrid", "Despicable",
    "Unsavory", "Horrid", "Wily", "Remorseless", "Jagged", "Cursed"
    };
        return villainousAdjectives[Random.Range(0, villainousAdjectives.Length)];
    }
    public static string GetRandomHighLevelVillainousCreatures()
    {
        string[] villainousCreatures = {
    "Shadowfiend", "Dreadspawn", "Ghoulmonger", "Abysslurker", "Cursed Revenant", "Pale Wraith",
    "Hollow Stalker", "Infernal Behemoth", "Phantom Leech", "Maliceborn", "Specter Maw", "Chittering Horror",
    "Twilight Devourer", "Bloodbound Harbinger", "Void Howler", "Fleshweaver", "Omenfang", "Warped Colossus",
    "Tenebrous Lurker", "Mire Serpent", "Sinister Homunculus", "Graveborn Abomination", "Nightmare Chimera",
    "Wretched Eidolon", "Doomwraith", "Crypt Horror", "Goreclaw Beast", "Pestilent Harpy", "Venomfang Revenant",
    "Witherfiend", "Ooze Tyrant", "Putrescent Horror", "Hellspawned Chimera", "Boneclad Marauder", "Gloomfang",
    "Aether Ghast", "Carrion Sovereign", "Soulflayer", "Pale Wurm", "Oblivion Fiend", "Blightborn Leviathan",
    "Havoc Strider", "Forsaken Drudge", "Fleshrend Gargoyle", "Dreadhusk", "Malformed Titan", "Warpstalker",
    "Lichbound Horror", "Whispering Shade", "Riftborn Banshee", "Skulltide Leviathan", "Void Revenant"
};
        return villainousCreatures[Random.Range(0, villainousCreatures.Length)];
    }
    public static string GetRandomMidLevelVillainousCreatures()
    {
        string[] pitifulCreatures =
        {
    "Blighted Homunculus", "Wilted Gremlin", "Tattered Wretch", "Gasping Sludge", "Withering Husk",
    "Hollow-Eyed Mongrel", "Mewling Thrall", "Sputtering Imp", "Broken Lurker", "Fumbling Abomination",
    "Crumbling Golem", "Shivering Husk", "Crooked Homunculus", "Slouching Wight", "Scrawny Horror",
    "Whimpering Shade", "Dribbling Ghoul", "Lopsided Revenant", "Spindly Creep", "Soggy Bogling",
    "Rattling Boneheap", "Mangled Scarefiend", "Pustule Fiend", "Wheezing Ghast", "Drooping Slimekin",
    "Twitching Knave", "Gummy Mawspawn", "Hobbling Phantasm", "Misshapen Scrag", "Sallow Gob",
    "Frayed Poppet", "Stumbling Wisp", "Wiltfang", "Dregspawn", "Wretched Toadling", "Shambling Bogbeast",
    "Pockmarked Leperkin", "Damp-Eyed Wailer", "Sniveling Fumblefiend", "Sagging Specter", "Mottled Grubkin",
    "Tatterwing Harpy", "Hunched Carrionette", "Drooling Nightlurker", "Spasmclaw", "Brittlebone Creep",
    "Peeling Skinsludge", "Gurgling Murkspawn", "Twitching Hollow", "Gloombound Whelp", "Warped Beggarwraith"
    };
        return pitifulCreatures[Random.Range(0, pitifulCreatures.Length)];
    }

    public static string GetRandomlowLevelVillainousCreatures()
    {
        string[] strangeCreatures = {
    "Homunculus", "Gremlin", "Wretch", "Sludge", "Husk", "Mongrel", "Thrall", "Imp", "Lurker", "Abomination",
    "Golem", "Wight", "Horror", "Shade", "Ghoul", "Revenant", "Creep", "Bogling", "Boneheap", "Scarefiend",
    "Fiend", "Ghast", "Slimekin", "Knave", "Mawspawn", "Phantasm", "Scrag", "Gob", "Poppet", "Wisp",
    "Dreg", "Toadling", "Bogbeast", "Leperkin", "Wailer", "Fumblefiend", "Specter", "Grubkin", "Harpy",
    "Carrionette", "Nightlurker", "Spasmclaw", "Creep", "Skinsludge", "Murkspawn", "Hollow", "Whelp", "Beggarwraith"
};
        return strangeCreatures[Random.Range(0, strangeCreatures.Length)];

    }
}