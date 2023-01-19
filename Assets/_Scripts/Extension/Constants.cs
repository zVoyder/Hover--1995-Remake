using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension
{

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
            public const int PLAYER = 3;
            public const int MINIMAP = (1 << 3) | (1 << 6) | (1 << 7);
        }
    }
}
