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
            SetGameStatus(GameStatus.Running);
            UIManager.instance.ResumeGame();
            playerCam.SetActive(true);
            GameEvents.OnGameStart?.Invoke();
        }

        public void GameOver()
        {
            SetGameStatus(GameStatus.Over);
            UIManager.instance.OpenGameOver();
            GameEvents.OnGameOver?.Invoke();
        }
    }
}
