using UnityEngine;
using System.Collections.Generic;
using RatRush.World;
using RatRush.Enums;

namespace RatRush.Managers
{
    public class ChunkManager : MonoBehaviour
    {
        public static ChunkManager instance = null;
        [Header("Pool Settings")]
        [SerializeField] private List<ChunkPool> chunkPools;

        [Header("Spawn Settings")]
        [SerializeField] private float chunkLength = 20f;
        [SerializeField] private int initialChunksCount = 5;
        [SerializeField] private float despawnZ = -25f;

        [Header("Movement (Treadmill)")]
        public float moveSpeed = 15f;
        public float maxMoveSpeed = 40f;
        public float rate = 0.5f;

        private readonly List<GameObject> activeChunks = new();
        private float spawnPositionZ = 0f;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            InitializePool();

            for (int i = 0; i < initialChunksCount; i++)
                SpawnRandomChunk(true);
        }

        public void ResetChunks()
        {
            for (int i = 0; i < activeChunks.Count; i++)
                activeChunks[i].transform.position = chunkLength * i * Vector3.forward;
        }

        void Update()
        {
            if (GameManager.instance.gameStatus == GameStatus.Running)
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += rate * Time.deltaTime;
                    if (moveSpeed > maxMoveSpeed)
                        moveSpeed = maxMoveSpeed;
                }
                MoveChunks();
                CheckAndDespawn();
            }
        }

        private GameObject poolContainer;

        private void InitializePool()
        {
            if (poolContainer == null)
                poolContainer = new GameObject("Chunk Pool Container");

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