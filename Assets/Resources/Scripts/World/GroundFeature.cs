using System.Collections.Generic;
using UnityEngine;

namespace RatRush.World
{
    public class GroundFeature : Feature
    {
        [SerializeField] private List<GameObject> leftGrounds = new();
        [SerializeField] private List<GameObject> rightGrounds = new();

        public override void Setup()
        {
            leftGrounds.ForEach(x => x.SetActive(false));
            rightGrounds.ForEach(x => x.SetActive(false));
            ChooseRandom(leftGrounds);
            ChooseRandom(rightGrounds);
        }

        private void ChooseRandom(List<GameObject> targetList)
        {
            var index = Random.Range(0, targetList.Count);
            targetList[index].SetActive(true);
        }
    }
}
