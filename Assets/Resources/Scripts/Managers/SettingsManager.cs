using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

namespace RatRush.Managers
{
    [System.Serializable]
    public class SettingsData
    {
        public int resIndex;
        public string langCode;
        public bool fullscreen;
        public float generalVolume;
        public float sfxVolume;
        public float musicVolume;
    }

    public class SettingsManager : MonoBehaviour
    {
        private static readonly string SETTINGS_KEY = "GAME_SETTINGS";
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Slider generalAudioSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private AudioMixer mixer;
        private SettingsData currentData;

        private void Start()
        {
            currentData = LoadSettings();

            RefreshResolutions();
            RefreshFullscreen();
            RefreshLanguages();
            RefreshVolumes();
        }

        public void SaveSettings()
        {
            var json = JsonUtility.ToJson(currentData);
            PlayerPrefs.SetString(SETTINGS_KEY, json);
            PlayerPrefs.Save();
        }

        public SettingsData LoadSettings()
        {
            if (PlayerPrefs.HasKey(SETTINGS_KEY))
            {
                var json = PlayerPrefs.GetString(SETTINGS_KEY);
                return JsonUtility.FromJson<SettingsData>(json);
            }
            else
            {
                int defaultResIndex = Screen.resolutions.ToList().IndexOf(Screen.currentResolution);
                var data = new SettingsData()
                {
                    resIndex = defaultResIndex != -1 ? defaultResIndex : Screen.resolutions.Length - 1,
                    fullscreen = true,
                    langCode = "tr",
                    generalVolume = 1f,
                    sfxVolume = 1f,
                    musicVolume = 1f
                };
                var json = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(SETTINGS_KEY, json);
                return data;
            }
        }

        private void RefreshResolutions()
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(Screen.resolutions.Select(x => $"{x.width}x{x.height}@{(int)x.refreshRateRatio.value}").ToList());

            SetResolution(currentData.resIndex);
            resolutionDropdown.SetValueWithoutNotify(currentData.resIndex);

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void RefreshFullscreen()
        {
            fullscreenToggle.SetIsOnWithoutNotify(currentData.fullscreen);
            Screen.fullScreen = currentData.fullscreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        private void RefreshLanguages()
        {
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(LocalizationSettings.AvailableLocales.Locales.Select(x => x.LocaleName).ToList());
            var langIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(x => x.Identifier.Code == currentData.langCode);
            if (langIndex != -1)
                languageDropdown.SetValueWithoutNotify(langIndex);
            else
                languageDropdown.SetValueWithoutNotify(0);
            languageDropdown.onValueChanged.AddListener(SetLocale);
        }

        private void RefreshVolumes()
        {
            generalAudioSlider.SetValueWithoutNotify(currentData.generalVolume);
            sfxSlider.SetValueWithoutNotify(currentData.sfxVolume);
            musicSlider.SetValueWithoutNotify(currentData.musicVolume);

            mixer.SetFloat("MASTER_VOLUME", CalculateDB(currentData.generalVolume));
            mixer.SetFloat("SFX_VOLUME", CalculateDB(currentData.sfxVolume));
            mixer.SetFloat("MUSIC_VOLUME", CalculateDB(currentData.musicVolume));

            generalAudioSlider.onValueChanged.AddListener(v => SetVolume("MASTER_VOLUME", v));
            sfxSlider.onValueChanged.AddListener(v => SetVolume("SFX_VOLUME", v));
            musicSlider.onValueChanged.AddListener(v => SetVolume("MUSIC_VOLUME", v));
        }

        private void SetResolution(int i)
        {
            if (i < 0 || i >= Screen.resolutions.Length) return;
            var targetRes = Screen.resolutions[i];
            Screen.SetResolution(targetRes.width, targetRes.height, Screen.fullScreenMode, targetRes.refreshRateRatio);

            currentData.resIndex = i;
            SaveSettings();
        }

        private void SetFullscreen(bool b)
        {
            Screen.fullScreen = b;
            currentData.fullscreen = b;
            SaveSettings();
        }

        private void SetLocale(int i)
        {
            var target = LocalizationSettings.AvailableLocales.Locales[i];
            LocalizationSettings.SelectedLocale = target;
            currentData.langCode = target.Identifier.Code;
        }
        private void SetVolume(string key, float value)
        {
            mixer.SetFloat(key, CalculateDB(value));

            switch (key)
            {
                case "MASTER_VOLUME":
                    currentData.generalVolume = value;
                    break;
                case "SFX_VOLUME":
                    currentData.sfxVolume = value;
                    break;
                case "MUSIC_VOLUME":
                    currentData.musicVolume = value;
                    break;
            }
        }

        public float CalculateDB(float input)
        {
            input = Mathf.Clamp(input, 0.0001f, 1f);
            return 20f * Mathf.Log10(input);
        }

        public float CalculateInput(float db)
        {
            return Mathf.Pow(10f, db / 20f);
        }
    }
}