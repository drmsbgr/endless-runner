using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RatRush.World
{
    public class HumanFeature : Feature
    {
        [SerializeField] private List<GameObject> humans;
        [SerializeField] private int maxCountToSelect;
        [SerializeField] private List<Transform> initialPositions;
        [SerializeField] private bool allowNoHuman;

        public override void Setup()
        {
            var countToSelect = Random.Range(allowNoHuman ? 0 : 1, maxCountToSelect + 1);

            humans.ForEach(x =>
            {
                x.SetActive(false);
                x.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            });

            if (countToSelect == 0)
                return;

            var humanCopy = new List<GameObject>(humans);
            var posCopy = new List<Vector3>(initialPositions.Select(x => x.localPosition));

            for (int i = 0; i < countToSelect; i++)
            {
                var randomHumanIndex = Random.Range(0, humanCopy.Count);
                var randomPositionIndex = Random.Range(0, posCopy.Count);

                var human = humanCopy[randomHumanIndex];
                human.transform.localPosition = posCopy[randomPositionIndex];
                human.SetActive(true);
                humanCopy.RemoveAt(randomHumanIndex);
                posCopy.RemoveAt(randomPositionIndex);
            }
        }
    }
}
