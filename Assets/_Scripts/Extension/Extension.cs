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


            public static bool TryFindGameObject(string name, out GameObject gObject)
            {
                try
                {
                    Debug.Log("Trovate");
                    gObject = GameObject.Find(name); //Try to take it
                }
                catch (System.Exception)
                {
                    gObject = null;
                    return false;
                }

                return true;
            }
        }
    
    /// <summary>
    /// This class is an extension methods holder abount math calculations.
    /// </summary>
    public static class Mathematics
    {
    
        /// <summary>
        /// Return the percent of the number based on the max number given
        /// </summary>
        /// <param name="n">number</param>
        /// <param name="max">max number</param>
        /// <returns>percentage</returns>
        public static float Percent(float n, float max)
        {
            return (n / max) * 100f;
        }
    
    
        /// <summary>
        /// Compares two floating point numbers and return true if they are the approximately the
        /// same number.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// 
        /// <returns>true if they are approximately the same</returns>
        public static bool Approximately(float a, float b, float tollerance)
        {
            a = Mathf.Max(a, b);
            b = Mathf.Min(a, b);

            return (a - b < tollerance);
        }

        /// <summary>
        /// Check if 2 Vector2 are approximately the same 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tollerance">Tollerance</param>
        /// <returns></returns>
        public static bool Approximately(Vector2 a, Vector2 b, float tollerance)
        {
            return Approximately(a.x, b.x, tollerance) && Approximately(a.y, b.y, tollerance);
        }

        public static bool Approximately(Quaternion q1, Quaternion q2, float threshold)
        {
            return Quaternion.Angle(q1, q2) < threshold;
        }

    }
    
    public static class Audios
    {
    
        /// <summary>
        /// Method to allow the passing of 
        /// AudioSFX Object into the mix so that copy the properties.
        /// </summary>
        /// <param name="audioSetting">AudioSFX settings</param>
        /// <param name="pos">position you want to spawn the audio</param>
        /// <returns></returns>
        public static void PlayClipAtPoint(AudioSFX audioSetting, Vector3 pos)
        {
            GameObject tempGO = new GameObject("ClipAtPoint " + pos); // create the temp object
            tempGO.transform.position = pos; // set its position
            AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
            tempASource.clip = audioSetting.clip;
            tempASource.volume = audioSetting.volume;
            tempASource.pitch = audioSetting.pitch;
            tempASource.outputAudioMixerGroup = audioSetting.mixerGroup;
            tempASource.spatialBlend = audioSetting.spatialBlend;

            tempASource.Play(); // play the sound
            MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy the gameobject at clip's end
        }
    
    }

    namespace Data
    {
        [System.Serializable]
        public class IntRange
        {
            public int min;
            public int max;

            public IntRange(int min, int max)
            {
                this.min = min;
                this.max = max;
                Validate();
            }

            private void Validate()
            {
                if (min > max)
                {
                    int temp = min;
                    min = max;
                    max = temp;
                }
            }
        }

        [System.Serializable]
        public class Reps
        {
            [Range(0, 1000)]public int series;
            [Range(0, 1000)]public int steps;

            public int Total()
            {
                return series * steps;
            }

        }
    }
}
