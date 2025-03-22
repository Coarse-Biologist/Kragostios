using System.Collections.Generic;
using KragostiosAllEnums;
using Unity.Collections;
using UnityEngine;

namespace Summons
{
    public static class SummonCrafting
    {

        public static GameObject CraftSummon(GameObject creaturePrefab, Dictionary<Elements, int> usedCores, Dictionary<Elements, int> playerKnowledge, int toolsknown)
        {
            StatsHandler stats = creaturePrefab.GetComponent<StatsHandler>();
            foreach (KeyValuePair<Elements, int> cores in usedCores)
            {
                stats.IncrementAttribute(stats.ElementStatDict[cores.Key], playerKnowledge[cores.Key], 0);
            }

            return creaturePrefab;
        }

        public static void UpgradeSummon(GameObject creaturePrefab, StatType stat, Dictionary<Elements, int> usedCores, Dictionary<Elements, int> playerKnowledge, int toolsknown)
        {

        }


    }
}
