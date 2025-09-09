using System;
using SnakeGame.Core;
using SnakeGame.Core.Models;

public class ScoreService : IScoreService
{
    public event Action<int> OnScoreChanged;
    public event Action<int> OnHighScoreChanged;
    public event Action<int> OnNewHighScoreAchieved;

    public int CurrentScore => _scoreModel.CurrentScore;
    public int HighScore => _scoreModel.HighScore;

    private readonly ScoreModel _scoreModel;
    private readonly IDataStorage _dataStorage;
    private const string HighScoreKey = "HighScore";

    public ScoreService(IDataStorage dataStorage)
    {
        _dataStorage = dataStorage;
        int loadedHighScore = _dataStorage.GetInt(HighScoreKey, 0);
        _scoreModel = new ScoreModel(loadedHighScore);
        SubscribeToEvents();
    }

    public void AddScore(int points) => _scoreModel.AddScore(points);

    public void ResetScore() => _scoreModel.ResetScore();

    public void ClearHighScore()
    {
        _scoreModel.ClearHighScore();
        _dataStorage.DeleteKey(HighScoreKey);
        _dataStorage.Save();
    }

    private void SubscribeToEvents()
    {
        _scoreModel.OnScoreChanged += score => OnScoreChanged?.Invoke(score);
        _scoreModel.OnHighScoreChanged += highScore => OnHighScoreChanged?.Invoke(highScore);
        _scoreModel.OnNewHighScoreAchieved += newHighScore =>
        {
            SaveHighScore(newHighScore);
            OnNewHighScoreAchieved?.Invoke(newHighScore);
        };
    }

    private void SaveHighScore(int highScore)
    {
        _dataStorage.SetInt(HighScoreKey, highScore);
        _dataStorage.Save();
    }
}