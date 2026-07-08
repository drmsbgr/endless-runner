using UnityEngine;

namespace RatRush.ScriptableObjects
{
    [CreateAssetMenu(fileName = "collectible_", menuName = "RatRush/Collectible")]
    public class CollectibleSO : ScriptableObject
    {
        public Sprite collectibleIcon;
        public float scoreBonus;
    }
}