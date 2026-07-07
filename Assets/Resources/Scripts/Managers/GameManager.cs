using UnityEngine;

namespace RatRush.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;
        public GameObject player;
        void Awake()
        {
            instance = this;
        }
    }
}
