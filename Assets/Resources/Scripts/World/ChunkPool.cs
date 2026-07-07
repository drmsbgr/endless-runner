using System.Collections.Generic;
using UnityEngine;

namespace RatRush.World
{
    [System.Serializable]
    public class ChunkPool
    {
        public GameObject chunkPrefab;
        public int amountToPool = 3;
        [HideInInspector]
        public Queue<GameObject> inactiveChunks = new();
    }
}
