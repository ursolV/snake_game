using SnakeGame.Core;
using SnakeGame.Core.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SnakeGame.UI.Windows
{
    public class MenuWindow : AbstractWindow
    {
        [SerializeField]
        private Button _startButton;
        [SerializeField]
        private Button _skinButton;
        [SerializeField]
        private Button _quitButton;

        [Inject]
        IWindowManager _windowManager;
        [Inject]
        IGameStateService _gameStateService;

        private void Awake()
        {
            _startButton.onClick.AddListener(HandleStartClick);
            _quitButton.onClick.AddListener(HandleQuitClick);
            _skinButton.onClick.AddListener(HandleSkinClick);
        }

        private void HandleSkinClick()
        {
            _windowManager.OpenWindow(nameof(SkinWindow));
        }

        private void HandleStartClick()
        {
            _gameStateService.ChangeState(GameState.Playing);
            HandleCloseClick();
        }

        private void HandleQuitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}