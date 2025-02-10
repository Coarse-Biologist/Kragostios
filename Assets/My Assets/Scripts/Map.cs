using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor.Rendering;
using System.Numerics;
using System.IO.Compression;
using UnityEngine.InputSystem;
//using System.Numerics;

public class Map : MonoBehaviour
{
    #region // all map variables
    #region // map density variables
    [SerializeField] private int mapSize = 4;
    [SerializeField] private int hostileDensity;
    [SerializeField] private int traderDensity;
    [SerializeField] private int cityDensity;
    [SerializeField] private int villageDensity;
    [SerializeField] private int treasureDensity;
    [SerializeField] private int campsiteDensity;
    [SerializeField] private int impassableTerrainDensity;
    [SerializeField] private int healerDensity;
    [SerializeField] private int barrenDensity;

    #endregion

    #region // other map variables
    private UnityEngine.Vector2 mapDimensions;
    public Dictionary<LocationType, int> LocationDensityDict = new Dictionary<LocationType, int>();

    public Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>
    {
        {Directions.North, Directions.South},
        {Directions.East, Directions.West},
        {Directions.South,Directions.North},
        {Directions.West, Directions.East}
    };
    #endregion

    [SerializeField] private Dictionary<UnityEngine.Vector2, LocationType> mapDict;
    private Dictionary<UnityEngine.Vector2, Tuple<Kingdoms, Biomes>> map;
    public List<Directions> directions { private set; get; } = new List<Directions>
    {
        Directions.North,
        Directions.East,
        Directions.South,
        Directions.West
    };

    private List<UnityEngine.Vector2> vectorDirections = new List<UnityEngine.Vector2>
    {
        new UnityEngine.Vector2(0, 1),  // Up
        new UnityEngine.Vector2(0, -1), // Down
        new UnityEngine.Vector2(1, 0),  // Right
        new UnityEngine.Vector2(-1, 0)  // Left
    };
    #endregion
    #region // domaine and biome variables

    List<Biomes> biomesList;
    private List<Kingdoms> kingdomsList;
    Dictionary<Kingdoms, int> kingdomSizeDict;
    public Dictionary<UnityEngine.Vector2, Kingdoms> kingdomMapDict;
    public Dictionary<UnityEngine.Vector2, Biomes> biomesMapDict;



    #endregion

    private void Awake()
    {
        mapDict = MakeMapDict();
        AddKingdomsToMap();
    }

    // makes map of size mapSize squared and assigns random location types to each integer vector location
    private Dictionary<UnityEngine.Vector2, LocationType> MakeMapDict()
    {
        mapDict = new Dictionary<UnityEngine.Vector2, LocationType>();

        for (int x = -mapSize * mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize * mapSize; y < mapSize; y++)
            {
                LocationType locationType = GetRandomLocation();
                mapDict.Add(new UnityEngine.Vector2(x, y), locationType);
            }
        }
        mapDimensions = new UnityEngine.Vector2(mapSize, mapSize);
        return mapDict;
    }
    private Dictionary<LocationType, int> MakeLocationDensityDict()
    {
        LocationDensityDict = new Dictionary<LocationType, int>
        {
        {LocationType.Hostile, hostileDensity},
        {LocationType.Trader, traderDensity},
        {LocationType.City, cityDensity},
        {LocationType.Village, villageDensity},
        {LocationType.HiddenTreasure, treasureDensity},
        {LocationType.Campsite, campsiteDensity},
        {LocationType.ImpassableTerrain, impassableTerrainDensity},
        {LocationType.Healer, healerDensity},
        {LocationType.Barren, barrenDensity}
    };
        return LocationDensityDict;
    }
    // returns a random location based from possible list of locations
    private LocationType GetRandomLocation()
    {
        List<LocationType> locationChanceList = GetChanceList();
        System.Random random = new System.Random();
        int randomIndex = random.Next(locationChanceList.Count);
        LocationType randomLocationType = locationChanceList[randomIndex];
        return randomLocationType;

    }

    private List<LocationType> GetChanceList()
    {
        List<LocationType> locationChanceList = new List<LocationType>();

        LocationDensityDict = MakeLocationDensityDict();
        foreach (KeyValuePair<LocationType, int> kvp in LocationDensityDict)
        {
            int numberAdded = 0;
            while (numberAdded < LocationDensityDict[kvp.Key])
            {
                LocationType locationType = kvp.Key;
                int density = kvp.Value;
                locationChanceList.Add(locationType);
                numberAdded++;
            }

        }
        return locationChanceList;

    }

    public LocationType GetLocationType(UnityEngine.Vector2 playerlocation)
    {
        if (mapDict != null)
        {
            if (playerlocation.x <= mapSize || playerlocation.y <= mapSize
            || playerlocation.x >= -mapSize || playerlocation.y <= -mapSize)
            {
                LocationType locationType = mapDict[playerlocation];
                return locationType;
            }
            else return LocationType.EdgeOfTheWorld;

        }
        else return LocationType.Barren;
    }

    private Biomes GetRandomBiome()
    {
        biomesList = GetAllEnums<Biomes>();
        int biomeNum = biomesList.Count;
        int randomBiomeIndex = UnityEngine.Random.Range(0, biomeNum - 1);
        Biomes randomBiome = biomesList[randomBiomeIndex];
        return randomBiome;
    }

    public static List<T> GetAllEnums<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    private Dictionary<UnityEngine.Vector2, Kingdoms> KingdomStartPointss()
    {
        List<Kingdoms> domainType = GetAllEnums<Kingdoms>();
        kingdomMapDict = new Dictionary<UnityEngine.Vector2, Kingdoms>();
        List<UnityEngine.Vector2> mapDictList = mapDict.Keys.ToList();

        foreach (Kingdoms kingdom in domainType)
        {
            bool found = false;
            while (!found)
            {
                int randomIndex = UnityEngine.Random.Range(0, mapDict.Count);
                UnityEngine.Vector2 startPoint = mapDictList[randomIndex];
                Debug.Log($"{startPoint}");
                if (!kingdomMapDict.TryGetValue(startPoint, out Kingdoms kingdoms))
                {
                    kingdomMapDict.Add(startPoint, kingdom);
                    found = true; // leave the while loop
                }
                Debug.Log($"found: {found}");

            }
        }
        return kingdomMapDict;
    }
    private Dictionary<UnityEngine.Vector2, Kingdoms> KingdomStartPoints()
    {
        List<Kingdoms> domainType = GetAllEnums<Kingdoms>();
        kingdomMapDict = new Dictionary<UnityEngine.Vector2, Kingdoms>();

        // Store the keys from mapDict into a list once for efficiency
        List<UnityEngine.Vector2> availablePoints = mapDict.Keys.ToList();

        foreach (Kingdoms kingdom in domainType)
        {
            bool found = false;
            while (!found)
            {
                if (availablePoints.Count == 0)
                {
                    Debug.LogWarning("No available points left to assign kingdoms!");
                    break;
                }

                int randomIndex = UnityEngine.Random.Range(0, availablePoints.Count);
                UnityEngine.Vector2 startPoint = availablePoints[randomIndex];

                if (!kingdomMapDict.ContainsKey(startPoint))
                {
                    kingdomMapDict.Add(startPoint, kingdom);
                    found = true;

                    // Optional: Remove the assigned point from availablePoints to prevent reassignment
                    availablePoints.RemoveAt(randomIndex);
                }
            }
        }
        return kingdomMapDict;
    }
    public Kingdoms GetKingdom(UnityEngine.Vector2 vectorLocation)
    {
        Kingdoms kingdom = Kingdoms.SessPool;
        if (kingdomMapDict.Keys.ToList().Contains(vectorLocation))
        {
            kingdom = kingdomMapDict[vectorLocation];

        }
        return kingdom;
    }
    private void AddKingdomsToMap()
    {
        Dictionary<UnityEngine.Vector2, Kingdoms> kingdomStartPoints = KingdomStartPoints();
        List<UnityEngine.Vector2> startPointList = kingdomStartPoints.Keys.ToList();

        foreach (UnityEngine.Vector2 point in startPointList)
        {
            List<UnityEngine.Vector2> startPoints = new List<UnityEngine.Vector2> { point };
            int iterations = 10; // Expand twice instead of redundant calls

            for (int i = 0; i < iterations; i++)
            {
                Kingdoms kingdom = kingdomStartPoints[point];
                startPoints = BranchOut(startPoints, kingdom);
                if (startPoints.Count == 0) break; // Stop early if no new points were added
            }
        }
    }
    private List<UnityEngine.Vector2> BranchOutKingdom(List<UnityEngine.Vector2> startPoints, Kingdoms kingdom)
    {
        List<UnityEngine.Vector2> newStartPoints = new List<UnityEngine.Vector2>();

        foreach (UnityEngine.Vector2 startPoint in startPoints)
        {
            foreach (UnityEngine.Vector2 direction in vectorDirections)
            {
                UnityEngine.Vector2 newLocation = startPoint + direction;

                if (!kingdomMapDict.ContainsKey(newLocation)) // More efficient check
                {
                    kingdomMapDict[newLocation] = kingdom; // Direct assignment
                    newStartPoints.Add(newLocation);
                    Debug.Log($"Kingdom at point {newLocation}: {kingdom}");
                }
            }
        }

        return newStartPoints;
    }
    private List<UnityEngine.Vector2> BranchOutBiome(List<UnityEngine.Vector2> startPoints, Biomes biome)
    {
        List<UnityEngine.Vector2> newStartPoints = new List<UnityEngine.Vector2>();

        foreach (UnityEngine.Vector2 startPoint in startPoints)
        {
            foreach (UnityEngine.Vector2 direction in vectorDirections)
            {
                UnityEngine.Vector2 newLocation = startPoint + direction;

                if (!biomesMapDict.ContainsKey(newLocation)) // More efficient check
                {
                    biomesMapDict[newLocation] = biome; // Direct assignment
                    newStartPoints.Add(newLocation);
                    Debug.Log($"Kingdom at point {newLocation}: {biome}");
                }
            }
        }

        return newStartPoints;
    }
    private Dictionary<UnityEngine.Vector2, Biomes> BiomeStartPoints()
    {
        List<Biomes> domainType = GetAllEnums<Biomes>();
        biomesMapDict = new Dictionary<UnityEngine.Vector2, Biomes>();
        List<UnityEngine.Vector2> mapDictList = mapDict.Keys.ToList();

        foreach (Biomes biome in domainType)
        {
            bool found = false;
            while (!found)
            {
                int randomIndex = UnityEngine.Random.Range(0, mapDict.Count);
                UnityEngine.Vector2 startPoint = mapDictList[randomIndex];
                Debug.Log($"{startPoint}");
                if (!biomesMapDict.TryGetValue(startPoint, out Biomes biomes))
                {
                    biomesMapDict.Add(startPoint, biome);
                    found = true; // leave the while loop
                }
                Debug.Log($"found: {found}");

            }
        }
        return biomesMapDict;
    }
    private void AddBiomesToMap()
    {
        Dictionary<UnityEngine.Vector2, Biomes> biomeStartPoints = BiomeStartPoints();
        List<UnityEngine.Vector2> startPointList = biomeStartPoints.Keys.ToList();

        foreach (UnityEngine.Vector2 point in startPointList)
        {
            List<UnityEngine.Vector2> startPoints = new List<UnityEngine.Vector2> { point };
            int iterations = 10; // Expand twice instead of redundant calls

            for (int i = 0; i < iterations; i++)
            {
                Biomes biome = biomeStartPoints[point];
                startPoints = BranchOutBiome(startPoints, biome);
                if (startPoints.Count == 0) break; // Stop early if no new points were added
            }
        }
    }
    public Kingdoms GetBiome(UnityEngine.Vector2 vectorLocation)
    {
        Biomes biome = Biomes.Swamp;
        if (biomesMapDict.Keys.ToList().Contains(vectorLocation))
        {
            biome = biomesMapDict[vectorLocation];

        }
        return biome;
    }

}
