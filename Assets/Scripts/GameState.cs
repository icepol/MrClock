using UnityEngine.SceneManagement;

public static class GameState
{
    public static int Lives = 3;
    public static int Score = 0;

    public static float MinX = 0;
    public static float MaxX = 0;
    
    public static float MinY = 0;
    public static float MaxY = 0;

    public static int Level => int.Parse(SceneManager.GetActiveScene().name);

    public static void Reset()
    {
        Lives = 3;
        Score = 0;
    }
}
