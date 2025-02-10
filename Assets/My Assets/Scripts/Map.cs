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
    List<Kingdoms> kingdomsList;
    Dictionary<Kingdoms, int> kingdomSizeDict;
    Dictionary<UnityEngine.Vector2, Kingdoms> kingdomMapDict;



    #endregion

    private void Awake()
    {
        mapDict = MakeMapDict();
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

    private void SetKingdomSizes()
    {
        List<Kingdoms> allKingdoms = GetAllEnums<Kingdoms>();

        kingdomSizeDict = new Dictionary<Kingdoms, int>();
        foreach (Kingdoms kingdom in allKingdoms)
        {
            int kingdomSize = UnityEngine.Random.Range(20, 30);
            kingdomSizeDict.Add(kingdom, kingdomSize);
        }
    }

    private Dictionary<UnityEngine.Vector2, Kingdoms> KingdomStartPoints()
    {
        List<Kingdoms> domainType = GetAllEnums<Kingdoms>();
        kingdomMapDict = new Dictionary<UnityEngine.Vector2, Kingdoms>();
        foreach (Kingdoms kingdom in domainType)
        {
            bool found = false;
            while (!found)
            {
                UnityEngine.Vector2 startPoint = mapDict.Keys.ToList()[UnityEngine.Random.Range(0, kingdomMapDict.Count)];
                if (!kingdomMapDict.TryGetValue(startPoint, out Kingdoms kingdoms))
                {
                    kingdomMapDict.Add(startPoint, kingdom);
                    found = true; // leave the while loop
                }
            }
        }
        return kingdomMapDict;
    }

    private void AddKingdomsToMap()
    {
        Dictionary<UnityEngine.Vector2, Kingdoms> kingdomStartPoints = KingdomStartPoints();

        foreach (KeyValuePair<UnityEngine.Vector2, Kingdoms> kvp in kingdomStartPoints)
        {
            List<UnityEngine.Vector2> startPoints = new List<UnityEngine.Vector2>();
            startPoints.Add(kvp.Key);
            int i = 6;
            while (i > 0)
            {
                List<UnityEngine.Vector2> newStartPoints = BranchOut(startPoints, kvp.Value);
                BranchOut(newStartPoints, kvp.Value);
            }
        }
    }

    private List<UnityEngine.Vector2> BranchOut(List<UnityEngine.Vector2> startPoints, Kingdoms kingdom)
    {
        List<UnityEngine.Vector2> newStartPoints = new List<UnityEngine.Vector2>();

        foreach (UnityEngine.Vector2 startPoint in startPoints)
        {
            foreach (UnityEngine.Vector2 direction in vectorDirections)
            {
                UnityEngine.Vector2 newLocation = startPoint + direction;
                kingdomMapDict.TryAdd(newLocation, kingdom);
                newStartPoints.Add(newLocation);
            }
        }

        return newStartPoints;
    }

}
