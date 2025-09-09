using SnakeGame.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace SnakeGame.UI.Views
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        
        private IScoreService _scoreService;

        [Inject]
        public void Construct(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        private void Start()
        {
            _scoreService.OnScoreChanged += HandleScoreChanged;
            
            _scoreText.text = _scoreService.CurrentScore.ToString("N0");
        }

        private void HandleScoreChanged(int score)
        {
            _scoreText.text = score.ToString("N0");
        }

        private void OnDestroy()
        {
            _scoreService.OnScoreChanged -= HandleScoreChanged;
        }
    }
}