using System.Collections.Generic;
using RatRush.Entities;
using UnityEngine;

namespace RatRush.World
{

    public class BenchFeature : Feature
    {
        [SerializeField] private List<Bench> benches = new();

        public override void Setup()
        {
            benches.ForEach(x =>
            {
                x.hasUsed = false;
                x.gameObject.SetActive(false);
            });

            if (benches.Count > 0)
            {
                var amount = Random.Range(1, benches.Count);

                for (int i = 0; i < amount; i++)
                {
                    var usableBenches = benches.FindAll(x => !x.hasUsed);
                    var index = Random.Range(0, usableBenches.Count);
                    var bench = usableBenches[index];
                    bench.gameObject.SetActive(true);
                    bench.Refresh();
                    bench.hasUsed = true;
                }
            }
        }

    }
}
