using UnityEngine;
using UnityEngine.UI;
using SnakeGame.Core;
using Zenject;
using SnakeGame.Core.Models;

namespace SnakeGame.UI.Windows
{
    public class PauseWindow : AbstractWindow
    {
        [SerializeField]
        private Button changeSkinButton;
        [SerializeField]
        private Button resumeButton;
        [SerializeField]
        private Button _quitButton;

        [Inject]
        private IWindowManager _windowManager;
        [Inject]
        private IGameStateService _gameStateService;

        private void Awake()
        {
            changeSkinButton.onClick.AddListener(HandleChangeSkinClick);
            resumeButton.onClick.AddListener(HandleResumeClick);
            _quitButton.onClick.AddListener(HandleQuitClick);
        }

        private void OnEnable()
        {
            if (_gameStateService != null)
            {
                _gameStateService.OnStateChanged += HandleGameStateChanged;
            }
        }

        private void OnDisable()
        {
            if (_gameStateService != null)
            {
                _gameStateService.OnStateChanged -= HandleGameStateChanged;
            }
        }

        private void HandleChangeSkinClick()
        {
            _windowManager.OpenWindow(nameof(SkinWindow));
        }

        private void HandleResumeClick()
        {
            HandleCloseClick();
            _gameStateService.ChangeState(GameState.Playing);
        }

        private void HandleQuitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void HandleGameStateChanged(GameState prev, GameState current)
        {
            if (current == GameState.Playing)
            {
                HandleCloseClick();
            }
        }
    }
}