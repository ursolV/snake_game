using System;

namespace SnakeGame.Core
{
    public interface IScoreService
    {
        event Action<int> OnScoreChanged;
        event Action<int> OnHighScoreChanged;
        event Action<int> OnNewHighScoreAchieved;

        int CurrentScore { get; }
        int HighScore { get; }

        void AddScore(int points);
        void ResetScore();
        void ClearHighScore();
    }
}