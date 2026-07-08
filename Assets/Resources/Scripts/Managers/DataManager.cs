using System.Collections.Generic;
using UnityEngine;

namespace RatRush.Managers
{
    /// <summary>
    /// basit bir save sistemi yöneticisi
    /// </summary>
    public static class DataManager
    {
        private static readonly string GAME_DATA_KEY = "game_data";

        public static GameData LoadData()
        {
            if (!PlayerPrefs.HasKey(GAME_DATA_KEY))
                InitializeData();

            var json = PlayerPrefs.GetString(GAME_DATA_KEY);
            var data = JsonUtility.FromJson<GameData>(json);
            return data;
        }

        public static void SaveData(GameData data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(GAME_DATA_KEY, json);
        }

        public static void InitializeData()
        {
            var data = new GameData();
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(GAME_DATA_KEY, json);
        }
    }

    [System.Serializable]
    public class GameData
    {
        public List<CollectibleData> collectibleData = new();
        public float highScore;
    }

    [System.Serializable]
    public class CollectibleData
    {
        public string key;
        public int value;
    }
}