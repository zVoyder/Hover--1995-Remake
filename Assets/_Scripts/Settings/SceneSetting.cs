using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSetting
{

    /// <summary>
    /// Screen Resolution constants
    /// </summary>
    public static class ScreenResolution {
        public static Resolution STARTED = Screen.currentResolution;
        public static Vector2 WINDOWED = new Vector2(800, 600);
    }
}
