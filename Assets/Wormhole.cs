using System.Collections;
using UnityEngine;

public class Wormhole : MonoBehaviour
{
    [SerializeField] private float minCloseDelay = 0.2f;
    [SerializeField] private float maxCloseDelay = 0.8f;

    public void Close()
    {
        StartCoroutine(WaitAndClose());
    }

    IEnumerator WaitAndClose()
    {
        yield return new WaitForSeconds(Random.Range(minCloseDelay, maxCloseDelay));
        
        Destroy(gameObject);
    }
}
