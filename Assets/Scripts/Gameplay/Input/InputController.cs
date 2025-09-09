using System;
using SnakeGame.Core;
using SnakeGame.Core.Models;
using SnakeGame.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SnakeGame.Gameplay.Input
{
    public class InputController : IInitializable, IDisposable
    {
        [Inject]
        private readonly ISnakeService _snakeService;
        [Inject]
        private readonly IGameStateService _gameStateService;

        private void HandleMovePerformed(InputAction.CallbackContext callbackContext)
        {
            if (_gameStateService.CurrentState != GameState.Playing)
            {
                return;
            }
            var value = callbackContext.ReadValue<Vector2>();
            _snakeService.ChangeDirection(value.ToVector2Int());
        }

        private void HandleEscapeKeyPerformed(InputAction.CallbackContext obj)
        {
            switch (_gameStateService.CurrentState)
            {
                case GameState.Playing:
                    _gameStateService.Pause();
                    break;
                case GameState.Paused:
                    _gameStateService.Resume();
                    break;
            }
        }

        public void Initialize()
        {
            InputSystem.actions.FindAction("Move").performed += HandleMovePerformed;
            InputSystem.actions.FindAction("Cancel").performed += HandleEscapeKeyPerformed;
        }

        public void Dispose()
        {
            InputSystem.actions.FindAction("Move").performed -= HandleMovePerformed;
            InputSystem.actions.FindAction("Cancel").performed -= HandleEscapeKeyPerformed;
        }
    }
}