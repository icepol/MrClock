using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public const string LEVEL_START = "LevelStart";
    public const string LEVEL_FINISHED = "LevelFinished";
    public const string LEVEL_FAILED = "LevelFailed";
    
    public const string PLAYER_DIED = "PlayerDied";
    public const string PLAYER_SHOOT = "PlayerShoot";
    public const string PLAYER_UNDER_ATTACK = "PlayerUnderAttack";
    
    public const string ENEMY_DIED = "EnemyDied";

    public const string NET_BROKEN = "NetBroken";
    public const string NET_REPAIRED = "NetRepaired";
    public const string NET_DESTROYED = "NetDestroyed";

    public const string TOOL_SPAWNED = "ToolSpawned";
    public const string TOOL_COLLECTED = "ToolCollected";
    public const string TOOL_COUNT_CHANGE = "ToolCountChange";

    public const string TRANSITION_OPEN = "TransitionOpen";
    public const string TRANSITION_CLOSE = "TransitionClose";
    public const string TRANSITION_OPEN_FINISHED = "TransitionOpenFinished";
    public const string TRANSITION_CLOSE_FINISHED = "TransitionCloseFinished";
    
    // camera events 
    public const string BOUNDARIES_TOP_RIGHT = "BoundariesTopRight";
    public const string BOUNDARIES_BOTTOM_LEFT = "BoundariesBottomLeft";
    public const string CAMERA_START_FOLLOWING = "CameraStartFollowing";
    public const string CAMERA_STOP_FOLLOWING = "CameraStopFollowing";
}
