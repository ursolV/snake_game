namespace SnakeGame.Core
{
    public interface IDataStorage
{
    bool HasKey(string key);
    int GetInt(string key, int defaultValue = 0);
    void SetInt(string key, int value);
    string GetString(string key, string defaultValue = null);
    void SetString(string key, string value);
    void Save();
    void DeleteKey(string key);
    }
}