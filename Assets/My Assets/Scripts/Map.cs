using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.Collections;
using System.Diagnostics;
using NUnit.Framework;
using Mono.Cecil.Cil;

//using System.Numerics;

public class Map : MonoBehaviour
{
    #region // all map variables
    #region // map density variables
    [SerializeField] private int mapSize = 40;
    [SerializeField] private int hostileDensity;
    [SerializeField] private int traderDensity;
    [SerializeField] private int cityDensity;
    [SerializeField] private int villageDensity;
    [SerializeField] private int treasureDensity;
    [SerializeField] private int campsiteDensity;
    [SerializeField] private int impassableTerrainDensity;
    [SerializeField] private int healerDensity;
    [SerializeField] private int barrenDensity;
    [SerializeField] private int BiomeIterations = 5; // sizes
    [SerializeField] private int KingdomIterations = 6;
    [SerializeField] private int BiomeNumber = 2; // how many times an area of each biome type can be on the map. 
    #endregion

    #region // other map variables
    private Vector2Int mapDimensions;
    public Dictionary<LocationType, int> LocationDensityDict = new Dictionary<LocationType, int>();

    public Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>
    {
        {Directions.North, Directions.South},
        {Directions.East, Directions.West},
        {Directions.South,Directions.North},
        {Directions.West, Directions.East}
    };
    #endregion

    [SerializeField] public Dictionary<Vector2Int, LocationType> mapDict { private set; get; } = new Dictionary<Vector2Int, LocationType>();
    private Dictionary<Vector2Int, Tuple<Kingdoms, Biomes>> map = new Dictionary<Vector2Int, Tuple<Kingdoms, Biomes>>();
    public List<Directions> directions { private set; get; } = new List<Directions>
    {
        Directions.North,
        Directions.East,
        Directions.South,
        Directions.West
    };

    private List<Vector2Int> vectorDirections = new List<Vector2Int>
    {
        new Vector2Int(0, 1),  // Up
        new Vector2Int(0, -1), // Down
        new Vector2Int(1, 0),  // Right
        new Vector2Int(-1, 0)  // Left
    };
    #endregion
    #region // domaine and biome variables

    List<Biomes> biomesList;
    private List<Kingdoms> kingdomsList;
    Dictionary<Kingdoms, int> kingdomSizeDict;
    public Dictionary<Vector2Int, Kingdoms> kingdomMapDict { private set; get; } = new Dictionary<Vector2Int, Kingdoms>();
    public Dictionary<Vector2Int, Biomes> biomesMapDict { private set; get; } = new Dictionary<Vector2Int, Biomes>();



    #endregion

    private void Awake()
    {
        mapDict = MakeMapDict();
        AddKingdomsToMap();
        AddBiomesToMap();
        DisplayNumOfBiomesVectors();
    }

    // makes map of size mapSize squared and assigns random location types to each integer vector location
    private Dictionary<Vector2Int, LocationType> MakeMapDict()
    {
        mapDict = new Dictionary<Vector2Int, LocationType>();

        for (int x = -mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize; y < mapSize; y++)
            {
                LocationType locationType = GetRandomLocation();
                mapDict.Add(new Vector2Int(x, y), locationType);
            }
        }
        mapDimensions = new Vector2Int(mapSize, mapSize);
        //UnityEngine.Debug.Log($"map dict has {mapDict.Keys.Count} vectors");
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
        {LocationType.None, barrenDensity}
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

    public LocationType GetLocationType(Vector2Int playerlocation)
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
        else return LocationType.None;
    }



    private List<int> DisplayNumOfBiomesVectors()
    {
        int jungleInt = 0;
        int rollingHillsInt = 0;
        int swampInt = 0;
        int desertInt = 0;
        int glaciersInt = 0;
        int everGreenForestInt = 0;
        int grassyFieldsInt = 0;
        int perenialForestInt = 0;
        int tundraInt = 0;
        int mountainsInt = 0;
        int volcanicTerritoryInt = 0;
        int riverInt = 0;

        foreach (KeyValuePair<Vector2Int, Biomes> kvp in biomesMapDict)
        {
            switch (kvp.Value)
            {
                case Biomes.Jungle:
                    jungleInt++;
                    break;
                case Biomes.Swamp:
                    swampInt++;
                    break;
                case Biomes.RollingHills:
                    rollingHillsInt++;
                    break;
                case Biomes.Desert:
                    desertInt++;
                    break;
                case Biomes.Glaciers:
                    glaciersInt++;
                    break;
                case Biomes.EverGreenForest:
                    everGreenForestInt++;
                    break;
                case Biomes.GrassyFields:
                    grassyFieldsInt++;
                    break;
                case Biomes.PerenialForest:
                    perenialForestInt++;
                    break;
                case Biomes.Tundra:
                    tundraInt++;
                    break;
                case Biomes.Mountains:
                    mountainsInt++;
                    break;
                case Biomes.VolcanicTerritory:
                    volcanicTerritoryInt++;
                    break;
                case Biomes.River:
                    riverInt++;
                    break;
            }
        }
        List<int> biomeNumbers = new List<int> { grassyFieldsInt, everGreenForestInt, glaciersInt, jungleInt, rollingHillsInt, swampInt, mountainsInt, tundraInt, perenialForestInt, riverInt, volcanicTerritoryInt };

        UnityEngine.Debug.Log($"{jungleInt} jungle vectors. \n {grassyFieldsInt} grassy fields. {everGreenForestInt} evergreen forest. {glaciersInt} glaciers. {rollingHillsInt} rolling hills. {swampInt} swamp. {mountainsInt} mountains. {tundraInt} tundra. {perenialForestInt} perenial forest. {volcanicTerritoryInt} volcanic territory. {riverInt} river area");

        return biomeNumbers;
    }

    private Biomes GetRandomBiome()
    {
        biomesList = GeneralFunctions.GetAllEnums<Biomes>();
        int biomeNum = biomesList.Count;
        int randomBiomeIndex = UnityEngine.Random.Range(0, biomeNum - 1);
        Biomes randomBiome = biomesList[randomBiomeIndex];
        return randomBiome;
    }


    private Dictionary<Vector2Int, Kingdoms> KingdomStartPoints()
    {
        List<Kingdoms> domainType = GeneralFunctions.GetAllEnums<Kingdoms>();
        kingdomMapDict = new Dictionary<Vector2Int, Kingdoms>();

        // Store the keys from mapDict into a list once for efficiency
        List<Vector2Int> availablePoints = mapDict.Keys.ToList();

        foreach (Kingdoms kingdom in domainType)
        {
            bool found = false;
            while (!found)
            {
                if (availablePoints.Count == 0)
                {
                    UnityEngine.Debug.LogWarning("No available points left to assign kingdoms!");
                    break;
                }

                int randomIndex = UnityEngine.Random.Range(0, availablePoints.Count);
                //Debug.Log($"random index: {randomIndex}. found between 0 and {availablePoints.Count}");


                Vector2Int startPoint = availablePoints[randomIndex];

                if (!kingdomMapDict.ContainsKey(startPoint))
                {
                    kingdomMapDict.Add(startPoint, kingdom);
                    found = true;

                    // Optional: Remove the assigned point from availablePoints to prevent reassignment
                    availablePoints.RemoveAt(randomIndex);
                    //UnityEngine.Debug.Log($"{kingdom} start point found at {startPoint}");
                }
            }
        }
        return kingdomMapDict;
    }
    public Kingdoms GetKingdom(Vector2Int vectorLocation)
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
        Dictionary<Vector2Int, Kingdoms> kingdomStartPoints = KingdomStartPoints();
        List<Vector2Int> startPointList = kingdomStartPoints.Keys.ToList();

        foreach (Vector2Int point in startPointList)
        {
            List<Vector2Int> startPoints = new List<Vector2Int> { point };
            // Expand twice instead of redundant calls

            for (int i = 0; i < KingdomIterations; i++)
            {
                Kingdoms kingdom = kingdomStartPoints[point];
                startPoints = BranchOutKingdom(startPoints, kingdom);
                if (startPoints.Count == 0) break; // Stop early if no new points were added
            }
        }
    }
    private List<Vector2Int> BranchOutKingdom(List<Vector2Int> startPoints, Kingdoms kingdom)
    {
        List<Vector2Int> newStartPoints = new List<Vector2Int>();

        foreach (Vector2Int startPoint in startPoints)
        {
            foreach (Vector2Int direction in vectorDirections)
            {
                Vector2Int newLocation = startPoint + direction;

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
    private List<Vector2Int> BranchOutBiome(List<Vector2Int> startPoints, Biomes biome)
    {
        List<Vector2Int> newStartPoints = new List<Vector2Int>();

        foreach (Vector2Int startPoint in startPoints)
        {
            foreach (Vector2Int direction in vectorDirections)
            {
                Vector2Int newLocation = startPoint + direction;

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
    private Dictionary<Vector2Int, Biomes> BiomeStartPoints()
    {
        List<Biomes> domainType = GeneralFunctions.GetAllEnums<Biomes>(); // gets list of all biomes
        //Debug.Log($"Domain types is {domainType.Count} long");
        for (int i = 0; i < BiomeNumber; i++)
        {
            domainType.AddRange(domainType);
        }
        //Debug.Log($"Now Domain types is {domainType.Count} long");

        biomesMapDict = new Dictionary<Vector2Int, Biomes>(); // makes new dict to store locations and biomes at those locations
        List<Vector2Int> mapDictList = mapDict.Keys.ToList();


        foreach (Biomes biome in domainType)
        {
            bool found = false;
            while (!found)
            {
                int randomIndex = UnityEngine.Random.Range(0, mapDict.Count);
                //Debug.Log($"Biome: random index: {randomIndex}. found between 0 and {mapDictList.Count}");

                Vector2Int startPoint = mapDictList[randomIndex];
                if (!biomesMapDict.TryGetValue(startPoint, out Biomes biomes))
                {
                    //UnityEngine.Debug.Log($"{biome} start point found at {startPoint}");
                    biomesMapDict.Add(startPoint, biome);
                    found = true; // leave the while loop
                }
                //Debug.Log($"found: {found}");
            }
        }
        return biomesMapDict;
    }
    private void AddBiomesToMap()
    {
        Dictionary<Vector2Int, Biomes> biomeStartPoints = BiomeStartPoints();
        List<Vector2Int> startPointList = biomeStartPoints.Keys.ToList();

        foreach (Vector2Int point in startPointList)
        {
            List<Vector2Int> startPoints = new List<Vector2Int> { point };
            // Expand twice instead of redundant calls

            for (int i = 0; i < BiomeIterations; i++)
            {
                Biomes biome = biomeStartPoints[point];
                startPoints = BranchOutBiome(startPoints, biome);
                if (startPoints.Count == 0) break; // Stop early if no new points were added  

            }
        }
    }
    public Biomes GetBiome(Vector2Int vectorLocation)
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
            Vector2Int array = new Vector2Int(arrayX, arrayY);
            mapDict.Add(array, kvp.Value);
        }
        Dictionary<int[], Biomes> arrayBiomesMapDict = mapData.biomeDict_SD;
        foreach (KeyValuePair<int[], Biomes> kvp in arrayBiomesMapDict)
        {
            int arrayX = kvp.Key[0];
            int arrayY = kvp.Key[1];
            Vector2Int array = new Vector2Int(arrayX, arrayY);
            biomesMapDict.Add(array, kvp.Value);
        }
        Dictionary<int[], Kingdoms> arrayKingdomMapDict = mapData.kingdomDict_SD;
        foreach (KeyValuePair<int[], Kingdoms> kvp in arrayKingdomMapDict)
        {
            int arrayX = kvp.Key[0];
            int arrayY = kvp.Key[1];
            Vector2Int array = new Vector2Int(arrayX, arrayY);
            kingdomMapDict.Add(array, kvp.Value);
        }
    }


}
