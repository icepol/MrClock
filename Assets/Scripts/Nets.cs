using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nets : MonoBehaviour
{
    [SerializeField] private float minBrokeDelay = 0.5f;
    [SerializeField] private float maxBrokeDelay = 1f;
    
    private Net[] nets;
    
    void Start()
    {
        EventManager.AddListener(Events.NET_REPAIRED, OnNetRepaired);
        EventManager.AddListener(Events.TOOL_COLLECTED, OnToolCollected);

        nets = GetComponentsInChildren<Net>();

        StartCoroutine(DelayedBroken());
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.NET_REPAIRED, OnNetRepaired);
        EventManager.RemoveListener(Events.TOOL_COLLECTED, OnToolCollected);
    }

    IEnumerator DelayedBroken()
    {
        foreach (Net net in nets)
        {
            yield return new WaitForSeconds(Random.Range(minBrokeDelay, maxBrokeDelay));
            
            net.Broke();
        }
    }

    void OnToolCollected()
    {
        foreach (Net net in nets)
        {
            if (net.IsBroken)
            {
                net.Repair();
                break;
            }
        }
    }

    void OnNetRepaired()
    {
        // check if all nets are repaired
        foreach (Net net in nets)
        {
            if (net.IsBroken)
                return;
        }
        
        // all are repaired, we can switch to next level
        EventManager.TriggerEvent(Events.LEVEL_FINISHED);
    }
}
