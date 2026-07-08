using RatRush.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

namespace RatRush.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;
        public LocalizeStringEvent deathCause;
        [Header("Panels")]
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject pausePanel;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            OpenMainPanel();
        }

        void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (GameManager.instance.gameStatus == GameStatus.Running && !pausePanel.activeInHierarchy)
                    OpenPausePanel();
                else if (pausePanel.activeInHierarchy)
                    GameManager.instance.ResumeGame();
            }
        }

        private void CloseAllPanels()
        {
            inGamePanel.SetActive(false);
            mainPanel.SetActive(false);
            settingsPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            pausePanel.SetActive(false);
        }


        public void OpenMainPanel()
        {
            OpenPanel(mainPanel);
            GameManager.instance.SetGameStatus(GameStatus.Paused);
        }

        public void OpenSettings()
        {
            OpenPanel(settingsPanel);
            GameManager.instance.SetGameStatus(GameStatus.Paused);
        }

        public void OpenGameOver()
        {
            OpenPanel(gameOverPanel);
        }

        public void OpenPausePanel()
        {
            OpenPanel(pausePanel);
            GameManager.instance.SetGameStatus(GameStatus.Paused);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OpenPanel(GameObject panel)
        {
            CloseAllPanels();
            panel.SetActive(true);
        }

        public void ResumeGame()
        {
            OpenPanel(inGamePanel);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
