using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int ToolsCount { get; private set; }

    void Awake()
    {
        EventManager.AddListener(Events.LEVEL_START, OnLevelStart);
        EventManager.AddListener(Events.TOOL_COLLECTED, OnToolCollected);
        EventManager.AddListener(Events.NET_REPAIRED, OnNetRepaired);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.LEVEL_START, OnLevelStart);
        EventManager.RemoveListener(Events.TOOL_COLLECTED, OnToolCollected);
        EventManager.RemoveListener(Events.NET_REPAIRED, OnNetRepaired);
    }

    private void OnLevelStart()
    {
        GameState.Tools = ToolsCount;
    }
    
    private void OnToolCollected()
    {
        ToolsCount++;
        GameState.Tools = ToolsCount;
        
        EventManager.TriggerEvent(Events.TOOL_COUNT_CHANGE, ToolsCount);
    }

    private void OnNetRepaired()
    {
        ToolsCount--;
        GameState.Tools = ToolsCount;
        
        EventManager.TriggerEvent(Events.TOOL_COUNT_CHANGE, ToolsCount);
    }
}
