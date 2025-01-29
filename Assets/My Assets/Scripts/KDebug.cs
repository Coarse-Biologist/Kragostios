using UnityEngine;

public static class KDebug
{
private static bool DebugMode = true;


public static void SeekBug(string debugMessage)
{
    if(DebugMode) Debug.Log(debugMessage);
}

}
