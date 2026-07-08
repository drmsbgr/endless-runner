using System.Collections.Generic;
using RatRush.Enums;
using RatRush.World;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RatRush.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static bool startImmediately;
        public static GameManager instance = null;
        public GameObject player;
        public GameStatus gameStatus;
        public GameObject playerCam;
        public InitialChunk startChunk;

        //gameplay
        public Dictionary<string, int> collectiblesData = new();
        public float score;
        public AudioSource musicSource;
        //collect particles
        [SerializeField] private List<GameObject> collectParticles = new();

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            if (startImmediately)
            {
                startImmediately = false;
                StartGame();
            }
        }

        void Update()
        {
            if (gameStatus == GameStatus.Running)
            {
                score += ChunkManager.instance.moveSpeed * Time.deltaTime;
                UIManager.instance.scoreLabel.text = ((int)score).ToString();
                if (musicSource.pitch < 1.5f)
                {
                    musicSource.pitch += 0.002f * Time.deltaTime;
                    if (musicSource.pitch > 1.5f)
                        musicSource.pitch = 1.5f;
                }
            }
        }

        public void ActivateParticle(Vector3 pos)
        {
            var availables = collectParticles.FindAll(x => !x.activeInHierarchy);
            var index = Random.Range(0, availables.Count);
            availables[index].transform.position = pos;
            availables[index].SetActive(true);
        }

        public void SetGameStatus(GameStatus newStatus) => gameStatus = newStatus;

        public void ResumeGame()
        {
            UIManager.instance.ResumeGame();
            SetGameStatus(GameStatus.Running);
        }

        public void RestartGame()
        {
            startImmediately = true;
            SceneManager.LoadScene("Game");
        }

        public void StartGame()
        {
            musicSource.Play();
            SetGameStatus(GameStatus.Running);
            UIManager.instance.ResumeGame();
            playerCam.SetActive(true);
            GameEvents.OnGameStart?.Invoke();
        }

        public void GameOver()
        {
            musicSource.Stop();
            musicSource.pitch = 1f;
            SetGameStatus(GameStatus.Over);
            UIManager.instance.OpenGameOver();

            SaveNewData();
            UIManager.instance.ReloadDataUI();
            collectiblesData = new();
            score = 0f;

            GameEvents.OnGameOver?.Invoke();
        }

        private void SaveNewData()
        {
            var newData = DataManager.LoadData();

            if (score > newData.highScore)
                newData.highScore = score;

            foreach (var item in collectiblesData)
            {
                var found = newData.collectibleData.Find(x => x.key == item.Key);

                if (found != null)
                    found.value += item.Value;
                else
                    newData.collectibleData.Add(new() { key = item.Key, value = item.Value });
            }

            DataManager.SaveData(newData);
        }
    }
}
