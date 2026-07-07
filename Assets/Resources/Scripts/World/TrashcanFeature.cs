using System.Collections.Generic;
using RatRush.Entities;
using UnityEngine;

namespace RatRush.World
{
    public class TrashcanFeature : Feature
    {
        [SerializeField] private List<Trashcan> trashcans = new();

        public override void Setup()
        {
            trashcans.ForEach(x => x.Refresh());
        }
    }
}
