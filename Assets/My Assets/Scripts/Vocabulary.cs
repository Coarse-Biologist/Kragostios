using System.Collections.Generic;

using UnityEngine;
using KragostiosAllEnums;
using UnityEditor.Build.Pipeline;

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

    public static Dictionary<Elements, string[]> MakeElementAdjectiveDict()
    {
        Dictionary<Elements, string[]> elementAdjectivesDict = new Dictionary<Elements, string[]>
{
    { Elements.None, new string[] { "Unaligned", "Void", "Neutral", "Shapeless", "Formless", "Hollow", "Absent", "Undefined", "Fading", "Nameless" } },

    { Elements.Cold, new string[] { "Frigid", "Glacial", "Icy", "Brittle", "Frozen", "Chilling", "Frostbitten", "Numbing", "Hoarfrosted", "Bleak" } },

    { Elements.Water, new string[] { "Fluid", "Turbulent", "Tidal", "Abyssal", "Drenched", "Briny", "Dewy", "Surging", "Flowing", "Undulating" } },

    { Elements.Earth, new string[] { "Stony", "Gravelly", "Earthen", "Gritty", "Unyielding", "Rooted", "Sedimentary", "Craggy", "Tremorous", "Basaltic" } },

    { Elements.Fire, new string[] { "Blazing", "Scorching", "Searing", "Charred", "Fiery", "Infernal", "Smoldering", "Combustive", "Ashen", "Embered" } },

    { Elements.Heat, new string[] { "Sweltering", "Scalding", "Suffocating", "Broiling", "Torrid", "Sizzling", "Oppressive", "Radiant", "Fiery", "Stifling" } },

    { Elements.Air, new string[] { "Ethereal", "Whispering", "Gusting", "Gale-born", "Cyclonic", "Vaporous", "Unbound", "Breathless", "Soaring", "Aerial" } },

    { Elements.Electricity, new string[] { "Crackling", "Arcing", "Jolting", "Static", "Charged", "Voltaic", "Sparking", "Electrified", "Shocking", "Luminous" } },

    { Elements.Poison, new string[] { "Venomous", "Toxic", "Noxious", "Lethal", "Corrupting", "Foul", "Pestilent", "Viral", "Insidious", "Paralyzing" } },

    { Elements.Acid, new string[] { "Caustic", "Corrosive", "Concentrated", "Erosive", "Acrid", "Searing", "Pungent", "Oozing", "Dissolving", "Scalding" } },

    { Elements.Bacteria, new string[] { "Infested", "Septic", "Contagious", "Microbial", "Pernicious", "Pathogenic", "Fetid", "Swarming", "Invasive", "Putrid" } },

    { Elements.Fungi, new string[] { "Moldering", "Spore-laden", "Mycelial", "Creeping", "Gnarled", "Pustular", "Blooming", "Infesting", "Lurking", "Decomposing" } },

    { Elements.Plant, new string[] { "Verdant", "Overgrown", "Rooted", "Thorned", "Vine-twined", "Briary", "Fungal", "Blooming", "Leafy", "Entangling" } },

    { Elements.Virus, new string[] { "Mutating", "Infectious", "Degrading", "Insidious", "Replicating", "Writhing", "Diseased", "Corruptive", "Malevolent", "Unseen" } },

    { Elements.Radiation, new string[] { "Irradiated", "Glowing", "Lethal", "Corrupting", "Mutagenic", "Pulsing", "Invisible", "Entropic", "Atomic", "Unstable" } },

    { Elements.Light, new string[] { "Luminous", "Dazzling", "Blinding", "Radiant", "Hallowed", "Piercing", "Resplendent", "Burning", "Brilliant", "Eclipsing" } },

    { Elements.Psychic, new string[] { "Mind-warping", "Hypnotic", "Eldritch", "Telepathic", "Distorting", "Unfathomable", "Dreamborne", "Echoing", "Illusive", "Reverberating" } }
};
        return elementAdjectivesDict;
    }


}