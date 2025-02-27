using KragostiosAllEnums;
using UnityEngine;

public class QuestEncounter_SO : ScriptableObject
{
    [SerializeField] public Kingdoms Kingdom;

    [SerializeField] public Biomes Biome;

    [SerializeField] public bool IsNPC;

    [SerializeField] public bool IsLocation;

    [SerializeField] public bool IsRepeatable;

    [SerializeField] public string Name;

    [SerializeField] public Quests.QuestName Quest;

    [SerializeField] public LocationType CompletionLocation;
}
