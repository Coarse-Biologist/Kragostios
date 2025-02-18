using UnityEngine;

using KragostiosAllEnums;
using UnityEditor.Experimental.GraphView;

public class TravelScript : MonoBehaviour
{
    public Vector2 playerLocation { private set; get; } = new Vector2(0, 0);

    private void MoveNorth()
    {
        playerLocation += new Vector2(0, 1);
        Debug.Log($"Your location is now {playerLocation}");
    }

    private void MoveEast()
    {
        playerLocation += new Vector2(1, 0);
        Debug.Log($"Your location is now {playerLocation}");
    }
    private void MoveSouth()
    {
        playerLocation -= new Vector2(0, 1);
        Debug.Log($"Your location is now {playerLocation}");

    }
    private void MoveWest()
    {
        playerLocation -= new Vector2(1, 0);
        Debug.Log($"Your location is now {playerLocation}");
    }

    public void TravelInDirection(Directions direction)
    {
        switch (direction)
        {
            case (Directions.North):
                MoveNorth();
                break;
            case (Directions.South):
                MoveSouth();
                break;
            case (Directions.East):
                MoveEast();
                break;
            case (Directions.West):
                MoveWest();
                break;
            default:
                break;
        }
    }
    //myButton.onClick.AddListener(() => CustomMethod("Hello, World!"));
    //myButton.onClick.RemoveAlllisteners();
    public void LoadData()
    {
        LocationData locationData = SaveSystem.LoadPlayerLocationData();
        float arrayX = locationData.playerLocation_SD[0];
        float arrayY = locationData.playerLocation_SD[1];
        playerLocation = new UnityEngine.Vector2(arrayX, arrayY);
    }

}
