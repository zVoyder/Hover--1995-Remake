using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension
{
    public static GameObject GetClosestGameObject(Transform self, GameObject[] gameObjetcs)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = self.position;

        foreach (GameObject t in gameObjetcs)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }

        return tMin;
    }

    public static bool TryGetClosestGameObjectWithTag(Transform self, string tag, out GameObject closest)
    {
        GameObject[] gameObjetcs = GameObject.FindGameObjectsWithTag(tag);

        if(gameObjetcs.Length > 0)
        {
            closest = GetClosestGameObject(self, gameObjetcs);
            return true;
        }

        closest = null;
        return false;
    }
}
