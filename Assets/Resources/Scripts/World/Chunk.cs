using System.Collections.Generic;
using UnityEngine;

namespace RatRush.World
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private List<Feature> features = new();

        public void RefreshChunk()
        {
            features.ForEach(x => x.Setup());
        }
    }
}
