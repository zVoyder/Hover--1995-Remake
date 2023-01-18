using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Extension
{
    /// <summary>
    /// This class is an extension methods holder abount findings GameObjects in the scene.
    /// </summary>
    public static class Finder
    {
        /// <summary>
            /// Get the closest gameobject in an array of gameobjects
            /// </summary>
            /// <param name="self">transform source</param>
            /// <param name="gameObjetcs">array of the gameobjects</param>
            /// <returns>the closest GameObject</returns>
        public static GameObject GetClosestGameObject(Transform self, GameObject[] gameObjetcs)
        {
            GameObject tMin = null;
            float minDist = Mathf.Infinity; // I set the minDist to Infinity so it will overwritten
            Vector3 currentPos = self.position;

            foreach (GameObject t in gameObjetcs) //Loop for checking who is the closest
            {
                float dist = Vector3.Distance(t.transform.position, currentPos); // I get the distance
                if (dist < minDist) // if the distance is less of the minimum distance
                {
                    tMin = t; // the closest gameobject for now
                    minDist = dist; // minimum distance for now
                }
            }

            return tMin;
        }

        /// <summary>
            /// Try to get the closest gameobject in an array of gameobjects found with a tag
            /// </summary>
            /// <param name="self">transform source</param>
            /// <param name="tag">tag of the gameobjects</param>
            /// <param name="closest">closestgameobject</param>
            /// <returns>True if the gameobject was found, False if not</returns>
        public static bool TryGetClosestGameObjectWithTag(Transform self, string tag, out GameObject closest)
        {
            GameObject[] gameObjetcs = GameObject.FindGameObjectsWithTag(tag);

            if (gameObjetcs.Length > 0)
            {
                closest = GetClosestGameObject(self, gameObjetcs);
                return true;
            }

            closest = null;
            return false;
        }
    

        /// <summary>
        /// Try to find a GameObject with a specified Tag
        /// </summary>
        /// <param name="tag">Tag name</param>
        /// <param name="gObject">Found Gameobject</param>
        /// <returns>True if it was found, False if not</returns>
        public static bool TryFindGameObjectWithTag(string tag, out GameObject gObject)
        {
            try 
            {
                gObject = GameObject.FindGameObjectWithTag(tag); //Try to take it
            }
            catch (System.Exception)
            {
                gObject = null;
                return false;
            }

            return true;
        }
    }
}
