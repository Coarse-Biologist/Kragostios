using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;
//using System.Numerics;

public class Map : MonoBehaviour
{
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
    private Vector2 mapDimensions;
    public Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>
    {
        {Directions.North, Directions.South},
        {Directions.East, Directions.West},
        {Directions.South,Directions.North}, 
        {Directions.West, Directions.East}
    };

    [SerializeField] private Dictionary<Vector2, LocationType> mapDict;
    private Dictionary<Vector2, Tuple<Kingdoms, Biomes>> map;
    public List<Directions> directions {private set; get;} = new List<Directions>
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

    private void Awake()
    {
        mapDict = MakeMapDict();
    }
    private Dictionary<Vector2, LocationType> MakeMapDict()
    {
        mapDict = new Dictionary<Vector2, LocationType>();

        for (int x = -mapSize * mapSize; x < mapSize; x++)
        {
            for (int y = -mapSize * mapSize; y < mapSize; y++)
            { 
                LocationType locationType = GetRandomLocation();
                mapDict.Add(new Vector2(x , y), locationType);
            }
        }
        mapDimensions = new Vector2 (mapSize, mapSize);
        return mapDict;
    }

    
    
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

        int numberAdded = 0;
        while(numberAdded < hostileDensity)
        {
            locationChanceList.Add(LocationType.Hostile);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < traderDensity)
        {
            locationChanceList.Add(LocationType.Trader);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < cityDensity)
        {
            locationChanceList.Add(LocationType.City);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < villageDensity)
        {
            locationChanceList.Add(LocationType.Village);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < campsiteDensity)
        {
            locationChanceList.Add(LocationType.Campsite);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < impassableTerrainDensity)
        {
            locationChanceList.Add(LocationType.ImpassableTerrain);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < healerDensity)
        {
            locationChanceList.Add(LocationType.Healer);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < barrenDensity)
        {
            locationChanceList.Add(LocationType.Barren);
            numberAdded ++;
        }
        numberAdded = 0;
        while(numberAdded < treasureDensity)
        {
            locationChanceList.Add(LocationType.HiddenTreasure);
            numberAdded ++;
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
        List<Biomes> biomeList = new List<Biomes>{Biomes.Jungle, Biomes.Swamp, Biomes.RollingHills,
         Biomes.GrassyFields, Biomes.Mountains, Biomes.Tundra, 
         Biomes.Glaciers, Biomes.EverGreenForest, Biomes.PerenialForest, Biomes.Desert};

        int biomeNum = biomeList.Count;
        int randomBiomeIndex = UnityEngine.Random.Range(0, biomeNum - 1);
        Biomes randomBiome = biomeList[randomBiomeIndex];
        return randomBiome;
    }

    private Kingdoms GetRandomKingdom()
    {
      List<Kingdoms> kingdomList = new List<Kingdoms>{Kingdoms.Celestia, Kingdoms.Grovchii, Kingdoms.Oshiania, Kingdoms.Bioleb, Kingdoms.SessPool};
        System.Random random = new System.Random();
        int randomKingdomIndex = random.Next(kingdomList.Count);
        Kingdoms randomKingdom = kingdomList[randomKingdomIndex];
        return randomKingdom;  
    }

    private void GiveMapDomaines(Dictionary<Vector2, LocationType> mapDict, int domainNum)
    {
        //set starting points in range MAP SIZE
        float mapWidth = mapDimensions.x;
        float mapHeight = mapDimensions.y;
        int startPointsSet = 0;
        Dictionary<Vector2, Tuple<Kingdoms, Biomes>> startCoordsDict = new Dictionary<Vector2, Tuple<Kingdoms, Biomes>>();
        while (startPointsSet < domainNum)
        {
            Vector2 startPoint = new Vector2(UnityEngine.Random.Range(0, mapWidth), UnityEngine.Random.Range(0, mapHeight));
            Biomes randomBiome = GetRandomBiome();
            Kingdoms randomKingdom = GetRandomKingdom();
            startCoordsDict.Add(startPoint, new Tuple<Kingdoms, Biomes>(randomKingdom, randomBiome));
            startPointsSet ++;
        }

        foreach(KeyValuePair<Vector2, Tuple<Kingdoms, Biomes>> startPointTuple in startCoordsDict)
        {
            //BranchInAllDirections(startPoint, domainType, domainNum);
        }
        //from starting points move one in each direction, check if domain is set or if out of boundary,
        // if not assign domain. 
    }

    private void BranchInAllDirections(Vector2 startingPoint, Tuple<Kingdoms, Biomes> tuple, int domainNum)
    {
        bool expansionMade;
        do
        {
            expansionMade = false;
            var currentMap = new Dictionary<Vector2, Tuple<Kingdoms, Biomes>>(map);

            foreach (var kvp in currentMap)
            {
                if (kvp.Value == null) continue; // Skip unassigned coordinates

                Vector2 coord = kvp.Key;
                Tuple<Kingdoms, Biomes> domain = kvp.Value;

                foreach (var dir in vectorDirections)
                {
                    Vector2 neighbor = coord + dir;

                    // Check bounds and if the neighbor is unassigned
                    if (map.ContainsKey(neighbor) && map[neighbor] == null)
                    {
                        map[neighbor] = domain; // Expand domain
                        expansionMade = true;
                    }
                }
            }
        } while (expansionMade); // Continue until no further expansion is possible
    }
    //{
    //    int spread = domainNum;
    //    int i = 0;
    //    while (i <= domainNum)
    //    {
    //        Vector2 nextSpot = new Vector2(startingPoint.x +1, startingPoint.y);
    //    }
//
   //


}
