using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Namespace used for constants
/// </summary>
namespace Constants
{
    /// <summary>
    /// Tag names
    /// </summary>
    public static class Tags
    {
        public const string PLAYER = "Player";
        public const string ENEMY = "Enemy";
        public const string ENEMY_FLAG = "EnemyFlag";
        public const string PICKUP = "Pickup";
        public const string MINIMAP = "Minimap";
    }

    /// <summary>
    /// Layers
    /// </summary>
    public static class Layers
    {
        public const int PLAYER = 1 << 3;
        public const int ALLMINIMAP = (1 << 6) | (1 << 7);
        public const int MINIMAP = 1 << 6;
        public const int VISIBLEMINIMAP = 1 << 7;
    }

    /// <summary>
    /// Screen Resolution constants
    /// </summary>
    public static class ScreenResolution
    {
        public static Vector2Int WINDOWED = new Vector2Int(800, 600);


    }

    /// <summary>
    /// Default names of the gameobject to find
    /// </summary>
    public static class GameObjectNames
    {
        public const string PAUSE = "Pause";
    }
}
