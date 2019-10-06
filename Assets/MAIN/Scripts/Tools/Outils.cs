using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Outils
{
    public static Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y));
    }

    public static bool GameObjectExistInArray(GameObject[] list_objs, GameObject gameobj)
    {
        foreach (GameObject list_obj in list_objs)
        {
            if (list_obj.Equals(gameobj))
            {
                return true;
            }
        }
        return false;
    }
}
