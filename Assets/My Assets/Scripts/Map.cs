using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor.Rendering;
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
    private Vector2 mapDimensions;
    public Dictionary<LocationType, int> LocationDensityDict = new Dictionary<LocationType, int>();

    public Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>
    {
        {Directions.North, Directions.South},
        {Directions.East, Directions.West},
        {Directions.South,Directions.North},
        {Directions.West, Directions.East}
    };
    #endregion

    [SerializeField] private Dictionary<Vector2, LocationType> mapDict;
    private Dictionary<Vector2, Tuple<Kingdoms, Biomes>> map;
    public List<Directions> directions { private set; get; } = new List<Directions>
    {
        Directions.North,
        Directions.East,
        Directions.South,
        Directions.West
    };

    private List<Vector2> vectorDirections = new List<Vector2>
    {
        new Vector2(0, 1),  // Up
        new Vector2(0, -1), // Down
        new Vector2(1, 0),  // Right
        new Vector2(-1, 0)  // Left
    };
    #endregion
    #region // domaine and biome variables

    List<Biomes> biomesList;
    List<Kingdoms> kingdomsList;

    #endregion

    private void Awake()
    {
        mapDict = MakeMapDict();
    }

    // makes map of size mapSize squared and assigns random location types to each integer vector location
    private Dictionary<Vector2, LocationType> MakeMapDict()
    {
        mapDict = new Dictionary<Vector2, LocationType>();

        for (int x = -mapSize * mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize * mapSize; y < mapSize; y++)
            {
                LocationType locationType = GetRandomLocation();
                mapDict.Add(new Vector2(x, y), locationType);
            }
        }
        mapDimensions = new Vector2(mapSize, mapSize);
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

    public LocationType GetLocationType(Vector2 playerlocation)
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
        biomesList = GetAllBiomes<Biomes>();
        int biomeNum = biomesList.Count;
        int randomBiomeIndex = UnityEngine.Random.Range(0, biomeNum - 1);
        Biomes randomBiome = biomesList[randomBiomeIndex];
        return randomBiome;
    }
    public static List<Biomes> GetAllBiomes<Biomes>()// where T : Enum
    {
        return Enum.GetValues(typeof(Biomes)).Cast<Biomes>().ToList();
    }
    public static List<Kingdoms> GetAllKingdom<Kingdoms>()// where T : Enum
    {
        return Enum.GetValues(typeof(Kingdoms)).Cast<Kingdoms>().ToList();
    }

    private void SetDomainSizes()
    {

    }

}
