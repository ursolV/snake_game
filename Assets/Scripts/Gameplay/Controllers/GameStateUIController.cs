using System;
using SnakeGame.UI;
using SnakeGame.Core;
using Zenject;
using SnakeGame.Core.Models;
using SnakeGame.UI.Windows;

namespace SnakeGame.Gameplay.Controllers
{
    public class GameStateUIController : IInitializable, IDisposable
    {
        [Inject]
        private readonly IGameStateService _gameStateService;
        [Inject]
        private readonly IWindowManager _windowManager;

        public void Initialize()
        {
            _gameStateService.OnStateChanged += HandleGameStateChanged;

            // Початкова настройка UI відповідно до поточного стану
            UpdateUIForState(_gameStateService.CurrentState);
        }

        private void HandleGameStateChanged(GameState previousState, GameState newState)
        {
            UpdateUIForState(newState);
        }

        private void UpdateUIForState(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    _windowManager.OpenWindow(nameof(MenuWindow));
                    break;

                case GameState.Paused:
                    _windowManager.OpenWindow(nameof(PauseWindow));
                    break;

                case GameState.GameOver:
                    _windowManager.OpenWindow(nameof(GameOverWindow));
                    break;
            }
        }

        public void Dispose()
        {
            _gameStateService.OnStateChanged -= HandleGameStateChanged;
        }
    }
}