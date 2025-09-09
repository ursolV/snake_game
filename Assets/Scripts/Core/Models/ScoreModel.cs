using System;

namespace SnakeGame.Core.Models
{
    public class ScoreModel
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnHighScoreChanged;
        public event Action<int> OnNewHighScoreAchieved;

        private int _currentScore;
        private int _highScore;

        public int CurrentScore
        {
            get => _currentScore;
            private set
            {
                if (_currentScore != value)
                {
                    _currentScore = value;
                    OnScoreChanged?.Invoke(_currentScore);
                    CheckHighScore();
                }
            }
        }

        public int HighScore
        {
            get => _highScore;
            private set
            {
                if (_highScore != value)
                {
                    _highScore = value;
                    OnHighScoreChanged?.Invoke(_highScore);
                }
            }
        }

        public ScoreModel(int initialHighScore = 0)
        {
            _highScore = initialHighScore;
        }

        public void AddScore(int points)
        {
            if (points <= 0)
            {
                return;
            }

            CurrentScore += points;
        }

        public void ResetScore()
        {
            CurrentScore = 0;
        }

        private void CheckHighScore()
        {
            if (CurrentScore > HighScore)
            {
                HighScore = CurrentScore;
                OnNewHighScoreAchieved?.Invoke(HighScore);
            }
        }

        public void ClearHighScore()
        {
            HighScore = 0;
        }
    }
}