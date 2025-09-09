using UnityEngine;

namespace SnakeGame.Core.Models
{
    public class PlayerPrefsStorage : IDataStorage
    {
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public int GetInt(string key, int defaultValue = 0)
            => PlayerPrefs.GetInt(key, defaultValue);

        public void SetInt(string key, int value)
            => PlayerPrefs.SetInt(key, value);

        public string GetString(string key, string defaultValue = null)
            => PlayerPrefs.GetString(key, defaultValue ?? string.Empty);

        public void SetString(string key, string value)
            => PlayerPrefs.SetString(key, value ?? string.Empty);

        public void Save() => PlayerPrefs.Save();

        public void DeleteKey(string key) => PlayerPrefs.DeleteKey(key);
    }
}