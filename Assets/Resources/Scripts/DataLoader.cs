using System.Collections.Generic;
using System.Linq;
using RatRush.ScriptableObjects;
using UnityEngine;

namespace RatRush
{
    public static class DataLoader
    {
        public static List<CollectibleSO> collectibles = Resources.LoadAll<CollectibleSO>("Data/Collectibles").ToList();
    }
}