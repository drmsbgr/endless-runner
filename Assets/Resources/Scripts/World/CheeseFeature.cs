using System.Collections.Generic;
using System.Linq;
using RatRush.Utilities;
using UnityEngine;

namespace RatRush.World
{
    public class CheeseFeature : Feature
    {
        [SerializeField] private List<GameObject> groups;
        [SerializeField] private int maxCountToSelect;
        [SerializeField] private bool randomizeCurve;
        [SerializeField] private List<Transform> initialPositions;

        public override void Setup()
        {
            var countToSelect = Random.Range(1, maxCountToSelect + 1);

            foreach (var item in groups)
            {
                for (int i = 0; i < item.transform.childCount; i++)
                    item.transform.GetChild(i).gameObject.SetActive(false);
                item.SetActive(false);
            }

            var groupCopy = new List<GameObject>(groups);
            var posCopy = new List<Vector3>(initialPositions.Select(x => x.localPosition));

            for (int i = 0; i < countToSelect; i++)
            {
                var randomGroupIndex = Random.Range(0, groupCopy.Count);
                var randomPositionIndex = Random.Range(0, posCopy.Count);

                var group = groupCopy[randomGroupIndex];
                group.transform.localPosition = posCopy[randomPositionIndex];

                for (int j = 0; j < group.transform.childCount; j++)
                    group.transform.GetChild(j).gameObject.SetActive(true);

                if (randomizeCurve)
                    group.GetComponent<MultiAligner>().curve = Random.Range(0, 2) == 0;

                group.SetActive(true);
                groupCopy.RemoveAt(randomGroupIndex);
                posCopy.RemoveAt(randomPositionIndex);
            }
        }
    }
}
