using UnityEngine;
using System.Collections.Generic;
using RatRush.World;

namespace RatRush.Managers
{
    public class ChunkManager : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private List<ChunkPool> chunkPools;

        [Header("Spawn Settings")]
        [SerializeField] private float chunkLength = 20f;
        [SerializeField] private int initialChunksCount = 5;
        [SerializeField] private float despawnZ = -25f;

        [Header("Movement (Treadmill)")]
        public float moveSpeed = 15f;

        private readonly List<GameObject> activeChunks = new();
        private float spawnPositionZ = 0f;

        void Start()
        {
            InitializePool();

            for (int i = 0; i < initialChunksCount; i++)
                SpawnRandomChunk(true);
        }

        void Update()
        {
            MoveChunks();
            CheckAndDespawn();
        }

        private void InitializePool()
        {
            var poolContainer = new GameObject("Chunk Pool Container");

            foreach (var pool in chunkPools)
            {
                for (int i = 0; i < pool.amountToPool; i++)
                {
                    var obj = Instantiate(pool.chunkPrefab, poolContainer.transform);
                    obj.SetActive(false);
                    pool.inactiveChunks.Enqueue(obj);
                }
            }
        }

        private void SpawnRandomChunk(bool emptyFirst = false)
        {
            int randomIndex = emptyFirst ? 0 : Random.Range(0, chunkPools.Count);

            var selectedPool = chunkPools[randomIndex];

            if (selectedPool.inactiveChunks.Count == 0)
            {
                foreach (var pool in chunkPools)
                {
                    if (pool.inactiveChunks.Count > 0)
                    {
                        selectedPool = pool;
                        break;
                    }
                }
            }

            var chunkToSpawn = selectedPool.inactiveChunks.Dequeue();

            chunkToSpawn.transform.position = new Vector3(0, 0, spawnPositionZ);
            chunkToSpawn.SetActive(true);
            chunkToSpawn.GetComponent<Chunk>().RefreshChunk();

            activeChunks.Add(chunkToSpawn);

            spawnPositionZ += chunkLength;
        }

        private void MoveChunks()
        {
            float moveDistance = moveSpeed * Time.deltaTime;

            foreach (var chunk in activeChunks)
                chunk.transform.Translate(Vector3.back * moveDistance);

            spawnPositionZ -= moveDistance;
        }

        private void CheckAndDespawn()
        {
            if (activeChunks.Count > 0 && activeChunks[0].transform.position.z < despawnZ)
            {
                var oldChunk = activeChunks[0];

                activeChunks.RemoveAt(0);
                oldChunk.SetActive(false);

                foreach (var pool in chunkPools)
                {
                    if (oldChunk.name.Contains(pool.chunkPrefab.name))
                    {
                        pool.inactiveChunks.Enqueue(oldChunk);
                        break;
                    }
                }

                SpawnRandomChunk();
            }
        }
    }
}