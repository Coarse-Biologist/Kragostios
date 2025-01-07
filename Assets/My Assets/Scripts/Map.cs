using UnityEngine;
using KragostiosAllEnums;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;

public class Map : MonoBehaviour
{
    [SerializeField] private int mapSize = 5;
    [SerializeField] private int hostileDensity; 
    [SerializeField] private int traderDensity;
    [SerializeField] private int cityDensity;
    [SerializeField] private int villageDensity;
    [SerializeField] private int treasureDensity;
    [SerializeField] private int campsiteDensity;
    [SerializeField] private int impassableTerrainDensity;
    [SerializeField] private int healerDensity;
    [SerializeField] private int barrenDensity;

    [SerializeField] private Dictionary<Vector2, LocationType> mapDict;

    public List<Directions> directions {private set; get;} = new List<Directions>
    {
        Directions.North,
        Directions.East, 
        Directions.South, 
        Directions.West
    };

    private void Awake()
    {
        mapDict = MakeMapDict();
    }
    private Dictionary<Vector2, LocationType> MakeMapDict()
    {
        mapDict = new Dictionary<Vector2, LocationType>();

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            { 
                LocationType locationType = GetRandomLocation();
                mapDict.Add(new Vector2(x , y), locationType);
            }
        }

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
            LocationType locationType = mapDict[playerlocation];
            return locationType;
        }
        else return LocationType.Barren;
    }
}
