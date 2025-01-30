using UnityEngine;

public static class KDebug
{
    private static bool DebugMode = true; // toggle debug logs on and off


    public static void SeekBug(string debugMessage)
    {
        if (DebugMode) Debug.Log(debugMessage);
    }

}
