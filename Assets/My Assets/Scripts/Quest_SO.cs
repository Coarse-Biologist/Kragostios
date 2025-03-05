using KragostiosAllEnums;
using UnityEngine;

[CreateAssetMenu(fileName = "Quests", menuName = "Crew's Quests")]

public class Quest_SO : ScriptableObject
{


    [SerializeField] public string Name;
    [Header("Bools")]
    public bool Repeatable = false;
    public bool LocationIndependent = true;
    public bool BiomeIndependent = true;
    public bool KingdomIndependent = true;

    #region location info

    [Header("Location Info")]

    public Quests.QuestName QuestEnum;
    public Kingdoms Kingdom;
    public LocationType StartLocation;
    public LocationType CompletionLocation;
    public Biomes Biome;

    #endregion

    #region descriptions

    [Header("Descriptions")]

    [TextArea(3, 10)]
    public string StartString;
    [TextArea(3, 10)]
    public string ProgressString;
    [TextArea(3, 10)]
    public string CompletionString;

    #endregion

    #region Rewards

    [Header("Rewards")]
    public int XP_Reward;
    public Item_SO ItemReward;

    public int GoldReward = 1;
    #endregion

}


