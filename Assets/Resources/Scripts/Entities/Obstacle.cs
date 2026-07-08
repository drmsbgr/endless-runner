using RatRush.Enums;
using UnityEngine;
using UnityEngine.Localization;

namespace RatRush.Entities
{
    public class Obstacle : MonoBehaviour
    {
        public ObstacleType obstacleType;

        void Start()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        public LocalizedString GetDeathStringReference()
        {
            var tableName = "UI_TABLE";
            return obstacleType switch
            {
                ObstacleType.Normal => new LocalizedString(tableName, "UI_FOOD_FOR_CAT"),
                ObstacleType.Bench => new LocalizedString(tableName, "UI_IMPACT_BENCH"),
                ObstacleType.Trashcan => new LocalizedString(tableName, "UI_CRUSHED_BY_TRASHCAN"),
                ObstacleType.Human => new LocalizedString(tableName, "UI_CRUSHED_BY_HUMAN"),
                _ => null,
            };
        }
    }
}