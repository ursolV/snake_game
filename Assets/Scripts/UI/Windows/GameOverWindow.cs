using SnakeGame.Core;
using SnakeGame.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SnakeGame.UI.Windows
{
    public class GameOverWindow : AbstractWindow
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        [SerializeField]
        private TextMeshProUGUI _highScoreText;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _skinButton;

        [Inject]
        IGameStateService _gameStateService;
        [Inject]
        private readonly IWindowManager _windowManager;
        [Inject]
        private readonly IScoreService _scoreService;

        private void Awake()
        {
            _restartButton.onClick.AddListener(HandleRestartClick);
            _skinButton.onClick.AddListener(HandleSkinClick);
        }

        public override void Open()
        {
            base.Open();
            var score = _scoreService.CurrentScore;
            var hiScore = _scoreService.HighScore;
            _scoreText.text = score.ToString("N0");
            _highScoreText.text = hiScore.ToString("N0");
        }

        private void HandleSkinClick()
        {
            _windowManager.OpenWindow(nameof(SkinWindow));
        }

        private void HandleRestartClick()
        {
            _gameStateService.ChangeState(GameState.Playing);
            HandleCloseClick();
        }
    }
}