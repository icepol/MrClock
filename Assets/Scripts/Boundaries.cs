using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    void Start()
    {
        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;
        
        foreach (Wall wall in GetComponentsInChildren<Wall>())
        {
            Vector2 position = wall.transform.position;

            minX = position.x < minX ? position.x : minX;
            maxX = position.x > maxX ? position.x : maxX;
            
            minY = position.y < minY ? position.y : minY;
            maxY = position.y > maxY ? position.y : maxY;
        }
        
        EventManager.TriggerEvent(Events.BOUNDARIES_BOTTOM_LEFT, new Vector2(minX, minY));
        EventManager.TriggerEvent(Events.BOUNDARIES_TOP_RIGHT, new Vector2(maxX, maxY));
        
        EventManager.TriggerEvent(Events.CAMERA_START_FOLLOWING);
    }
}
