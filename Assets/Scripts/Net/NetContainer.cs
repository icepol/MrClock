using System.Collections;
using UnityEngine;

public class NetContainer : MonoBehaviour
{
    [SerializeField] private float minBrokeDelay = 0.5f;
    [SerializeField] private float maxBrokeDelay = 1f;
    
    private Net[] nets;
    
    void Start()
    {
        EventManager.AddListener(Events.NET_REPAIRED, OnNetRepaired);

        nets = GetComponentsInChildren<Net>();

        StartCoroutine(DelayedBroken());
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.NET_REPAIRED, OnNetRepaired);
    }

    IEnumerator DelayedBroken()
    {
        foreach (Net net in nets)
        {
            yield return new WaitForSeconds(Random.Range(minBrokeDelay, maxBrokeDelay));
            
            net.Broke();
        }
    }

    void OnNetRepaired()
    {
        // check if all nets are repaired
        foreach (Net net in nets)
        {
            if (!net.IsRepaired)
                return;
        }
        
        // all are repaired, we can switch to next level
        EventManager.TriggerEvent(Events.NET_ALL_REPAIRED);
    }
}
