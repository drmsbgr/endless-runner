using RatRush.Managers;
using UnityEngine;

namespace RatRush.World
{
    public class InitialChunk : MonoBehaviour
    {
        [SerializeField] private float limitZ;

        void Update()
        {
            if (GameManager.instance.gameStatus == Enums.GameStatus.Running)
            {
                transform.position += ChunkManager.instance.moveSpeed * Time.deltaTime * Vector3.back;
                if (transform.position.z <= limitZ)
                    gameObject.SetActive(false);
            }
        }

        public void ResetChunk()
        {
            transform.position = Vector3.zero;
        }
    }
}
