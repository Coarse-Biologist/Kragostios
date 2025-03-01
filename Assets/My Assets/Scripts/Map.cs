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
    [SerializeField] private int mapSize = 100;
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
    private UnityEngine.Vector2Int mapDimensions;
    public Dictionary<LocationType, int> LocationDensityDict = new Dictionary<LocationType, int>();

    public Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>
    {
        {Directions.North, Directions.South},
        {Directions.East, Directions.West},
        {Directions.South,Directions.North},
        {Directions.West, Directions.East}
    };
    #endregion

    [SerializeField] public Dictionary<UnityEngine.Vector2Int, LocationType> mapDict { private set; get; } = new Dictionary<UnityEngine.Vector2Int, LocationType>();
    private Dictionary<UnityEngine.Vector2Int, Tuple<Kingdoms, Biomes>> map = new Dictionary<UnityEngine.Vector2Int, Tuple<Kingdoms, Biomes>>();
    public List<Directions> directions { private set; get; } = new List<Directions>
    {
        Directions.North,
        Directions.East,
        Directions.South,
        Directions.West
    };

    private List<UnityEngine.Vector2Int> vectorDirections = new List<UnityEngine.Vector2Int>
    {
        new UnityEngine.Vector2Int(0, 1),  // Up
        new UnityEngine.Vector2Int(0, -1), // Down
        new UnityEngine.Vector2Int(1, 0),  // Right
        new UnityEngine.Vector2Int(-1, 0)  // Left
    };
    #endregion
    #region // domaine and biome variables

    List<Biomes> biomesList;
    private List<Kingdoms> kingdomsList;
    Dictionary<Kingdoms, int> kingdomSizeDict;
    public Dictionary<UnityEngine.Vector2Int, Kingdoms> kingdomMapDict { private set; get; } = new Dictionary<UnityEngine.Vector2Int, Kingdoms>();
    public Dictionary<UnityEngine.Vector2Int, Biomes> biomesMapDict { private set; get; } = new Dictionary<UnityEngine.Vector2Int, Biomes>();



    #endregion

    private void Awake()
    {
        mapDict = MakeMapDict();
        AddKingdomsToMap();
        AddBiomesToMap();
    }

    // makes map of size mapSize squared and assigns random location types to each integer vector location
    private Dictionary<UnityEngine.Vector2Int, LocationType> MakeMapDict()
    {
        mapDict = new Dictionary<UnityEngine.Vector2Int, LocationType>();

        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                LocationType locationType = GetRandomLocation();
                mapDict.Add(new UnityEngine.Vector2Int(x, y), locationType);
            }
        }
        mapDimensions = new UnityEngine.Vector2Int(mapSize, mapSize);
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

    public LocationType GetLocationType(UnityEngine.Vector2Int playerlocation)
    {
        if (mapDict != null)
        {
            if (playerlocation.x <= mapSize && playerlocation.y <= mapSize
            && playerlocation.x >= -mapSize && playerlocation.y <= -mapSize) //bug?
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

    private Dictionary<UnityEngine.Vector2Int, Kingdoms> KingdomStartPoints()
    {
        List<Kingdoms> domainType = GetAllEnums<Kingdoms>();
        kingdomMapDict = new Dictionary<UnityEngine.Vector2Int, Kingdoms>();

        // Store the keys from mapDict into a list once for efficiency
        List<UnityEngine.Vector2Int> availablePoints = mapDict.Keys.ToList();

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
                Debug.Log($"random index: {randomIndex}. found between 0 and {availablePoints.Count}");


                UnityEngine.Vector2Int startPoint = availablePoints[randomIndex];

                if (!kingdomMapDict.ContainsKey(startPoint))
                {
                    kingdomMapDict.Add(startPoint, kingdom);
                    found = true;

                    // Optional: Remove the assigned point from availablePoints to prevent reassignment
                    availablePoints.RemoveAt(randomIndex);
                    Debug.Log($"{kingdom} start point found at {startPoint}");
                }
            }
        }
        return kingdomMapDict;
    }
    public Kingdoms GetKingdom(UnityEngine.Vector2Int vectorLocation)
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
        Dictionary<UnityEngine.Vector2Int, Kingdoms> kingdomStartPoints = KingdomStartPoints();
        List<UnityEngine.Vector2Int> startPointList = kingdomStartPoints.Keys.ToList();

        foreach (UnityEngine.Vector2Int point in startPointList)
        {
            List<UnityEngine.Vector2Int> startPoints = new List<UnityEngine.Vector2Int> { point };
            int iterations = 20; // Expand twice instead of redundant calls

            for (int i = 0; i < iterations; i++)
            {
                Kingdoms kingdom = kingdomStartPoints[point];
                startPoints = BranchOutKingdom(startPoints, kingdom);
                if (startPoints.Count == 0) break; // Stop early if no new points were added
            }
        }
    }
    private List<UnityEngine.Vector2Int> BranchOutKingdom(List<UnityEngine.Vector2Int> startPoints, Kingdoms kingdom)
    {
        List<UnityEngine.Vector2Int> newStartPoints = new List<UnityEngine.Vector2Int>();

        foreach (UnityEngine.Vector2Int startPoint in startPoints)
        {
            foreach (UnityEngine.Vector2Int direction in vectorDirections)
            {
                UnityEngine.Vector2Int newLocation = startPoint + direction;

                if (!kingdomMapDict.ContainsKey(newLocation)) // More efficient check
                {
                    kingdomMapDict[newLocation] = kingdom; // Direct assignment
                    newStartPoints.Add(newLocation);
                    //Debug.Log($"Kingdom at point {newLocation}: {kingdom}");
                }
            }
        }

        return newStartPoints;
    }
    private List<UnityEngine.Vector2Int> BranchOutBiome(List<UnityEngine.Vector2Int> startPoints, Biomes biome)
    {
        List<UnityEngine.Vector2Int> newStartPoints = new List<UnityEngine.Vector2Int>();

        foreach (UnityEngine.Vector2Int startPoint in startPoints)
        {
            foreach (UnityEngine.Vector2Int direction in vectorDirections)
            {
                UnityEngine.Vector2Int newLocation = startPoint + direction;

                if (!biomesMapDict.ContainsKey(newLocation)) // More efficient check
                {
                    biomesMapDict[newLocation] = biome; // Direct assignment
                    newStartPoints.Add(newLocation);
                    //Debug.Log($"Kingdom at point {newLocation}: {biome}");
                }
            }
        }

        return newStartPoints;
    }
    private Dictionary<UnityEngine.Vector2Int, Biomes> BiomeStartPoints()
    {
        List<Biomes> domainType = GetAllEnums<Biomes>(); // gets list of all biomes
        Debug.Log($"Domain types is {domainType.Count} long");
        domainType.AddRange(domainType);
        Debug.Log($"Now Domain types is {domainType.Count} long");

        biomesMapDict = new Dictionary<UnityEngine.Vector2Int, Biomes>(); // makes new dict to store locations and biomes at those locations
        List<UnityEngine.Vector2Int> mapDictList = mapDict.Keys.ToList();
        string desc = "";
        foreach (UnityEngine.Vector2Int vector in mapDictList)
        {
            desc += vector.ToString();
        }
        Debug.Log($"{desc}");
        Debug.Log($"map size: {mapDictList.Count} vector points");


        foreach (Biomes biome in domainType)
        {
            bool found = false;
            while (!found)
            {
                int randomIndex = UnityEngine.Random.Range(0, mapDict.Count);
                Debug.Log($"Biome: random index: {randomIndex}. found between 0 and {mapDictList.Count}");

                UnityEngine.Vector2Int startPoint = mapDictList[randomIndex];
                if (!biomesMapDict.TryGetValue(startPoint, out Biomes biomes))
                {
                    Debug.Log($"{biome} start point found at {startPoint}");
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
        Dictionary<UnityEngine.Vector2Int, Biomes> biomeStartPoints = BiomeStartPoints();
        List<UnityEngine.Vector2Int> startPointList = biomeStartPoints.Keys.ToList();

        foreach (UnityEngine.Vector2Int point in startPointList)
        {
            List<UnityEngine.Vector2Int> startPoints = new List<UnityEngine.Vector2Int> { point };
            int iterations = 20; // Expand twice instead of redundant calls

            for (int i = 0; i < iterations; i++)
            {
                Biomes biome = biomeStartPoints[point];
                startPoints = BranchOutBiome(startPoints, biome);
                if (startPoints.Count == 0) break; // Stop early if no new points were added
            }
        }
    }
    public Biomes GetBiome(UnityEngine.Vector2Int vectorLocation)
    {
        Biomes biome = Biomes.Swamp;
        if (biomesMapDict.Keys.ToList().Contains(vectorLocation))
        {
            biome = biomesMapDict[vectorLocation];
        }
        return biome;
    }

    public void LoadData()
    {
        MapData mapData = SaveSystem.LoadMapData();
        mapDict.Clear();
        biomesMapDict.Clear();
        kingdomMapDict.Clear();

        Dictionary<int[], LocationType> arrayMapDict = mapData.locationTypeDict_SD;

        foreach (KeyValuePair<int[], LocationType> kvp in arrayMapDict)
        {
            int arrayX = kvp.Key[0];
            int arrayY = kvp.Key[1];
            UnityEngine.Vector2Int array = new UnityEngine.Vector2Int(arrayX, arrayY);
            mapDict.Add(array, kvp.Value);
        }
        Dictionary<int[], Biomes> arrayBiomesMapDict = mapData.biomeDict_SD;
        foreach (KeyValuePair<int[], Biomes> kvp in arrayBiomesMapDict)
        {
            int arrayX = kvp.Key[0];
            int arrayY = kvp.Key[1];
            UnityEngine.Vector2Int array = new UnityEngine.Vector2Int(arrayX, arrayY);
            biomesMapDict.Add(array, kvp.Value);
        }
        Dictionary<int[], Kingdoms> arrayKingdomMapDict = mapData.kingdomDict_SD;
        foreach (KeyValuePair<int[], Kingdoms> kvp in arrayKingdomMapDict)
        {
            int arrayX = kvp.Key[0];
            int arrayY = kvp.Key[1];
            UnityEngine.Vector2Int array = new UnityEngine.Vector2Int(arrayX, arrayY);
            kingdomMapDict.Add(array, kvp.Value);
        }
    }


}
