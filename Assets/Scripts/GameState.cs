using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static int Lives = 3;
    public static int Level = 1;
    public static int Score = 0;

    public static void Reset()
    {
        Lives = 3;
        Score = 0;
    }
}
