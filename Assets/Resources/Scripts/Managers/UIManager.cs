using System.Collections.Generic;
using RatRush.Entities;
using RatRush.Enums;
using RatRush.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
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
        [Header("Scores&Points")]
        [SerializeField] private GameObject collectiblePrefab;
        public TextMeshProUGUI scoreLabel;
        public LocalizeStringEvent highScoreLabel;
        public TextMeshProUGUI totalCheeseLabel;
        private readonly List<UICollectible> _uiCollectibles = new();

        void Awake()
        {
            instance = this;
        }

        void OnEnable()
        {
            GameEvents.OnPlayerCollect += OnPlayerCollect;
        }

        void OnDisable()
        {
            GameEvents.OnPlayerCollect -= OnPlayerCollect;
        }

        void Start()
        {
            OpenMainPanel();
            CreateCollectibleUI();
            ReloadDataUI();
        }

        public void ReloadDataUI()
        {
            var data = DataManager.LoadData();
            (highScoreLabel.StringReference["high_score"] as IntVariable).Value = (int)data.highScore;

            var d = data.collectibleData.Find(x => x.key == "collectible_cheese");
            if (d != null)
                totalCheeseLabel.text = d.value.ToString();
            else
                totalCheeseLabel.text = "0";
        }

        private void CreateCollectibleUI()
        {
            foreach (var item in DataLoader.collectibles)
            {
                var g = Instantiate(collectiblePrefab, collectiblePrefab.transform.parent);
                var ui = g.GetComponent<UICollectible>();
                ui.Set(item);
                _uiCollectibles.Add(ui);
            }
        }

        void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (GameManager.instance.gameStatus == GameStatus.Running && !pausePanel.activeInHierarchy)
                {
                    Time.timeScale = 0f;
                    GameManager.instance.musicSource.Pause();
                    OpenPausePanel();
                }
                else if (pausePanel.activeInHierarchy)
                {
                    Time.timeScale = 1f;
                    GameManager.instance.musicSource.UnPause();
                    GameManager.instance.ResumeGame();
                }
            }
        }

        private void OnPlayerCollect(Collectible collectible)
        {
            var ui = _uiCollectibles.Find(x => x.data == collectible.data);
            ui.SetCount(GameManager.instance.collectiblesData[collectible.data.name]);
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
