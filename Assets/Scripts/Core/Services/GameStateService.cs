using System;
using System.Collections.Generic;
using UnityEngine;
using SnakeGame.Core.Models;

namespace SnakeGame.Core
{
    public class GameStateService : IGameStateService
    {
        private GameState _currentState = GameState.Loading;

        private readonly Dictionary<GameState, List<GameState>> _allowedTransitions = new()
        {
            { GameState.Loading, new List<GameState> { GameState.Menu } },
            { GameState.Menu, new List<GameState> { GameState.Playing } },
            { GameState.Playing, new List<GameState> { GameState.Paused, GameState.GameOver, GameState.Menu } },
            { GameState.Paused, new List<GameState> { GameState.Playing, GameState.Menu } },
            { GameState.GameOver, new List<GameState> { GameState.Menu, GameState.Playing } }
        };

        public GameState CurrentState => _currentState;

        public event Action<GameState, GameState> OnStateChanged;

        public void ChangeState(GameState newState)
        {
            if (!CanTransitionTo(newState))
            {
                Debug.LogWarning($"Invalid state transition from {_currentState} to {newState}");
                return;
            }

            var previousState = _currentState;
            _currentState = newState;

            HandleStateSpecificActions(previousState, newState);
            OnStateChanged?.Invoke(previousState, newState);
        }

        public void Pause()
        {
            Debug.Log("Pausing the game.");
            if (_currentState == GameState.Playing)
                ChangeState(GameState.Paused);
        }

        public void Resume()
        {
            Debug.Log("Resuming the game.");
            if (_currentState == GameState.Paused)
                ChangeState(GameState.Playing);
        }

        public bool IsState(GameState state) => _currentState == state;

        private bool CanTransitionTo(GameState newState)
        {
            return _allowedTransitions.ContainsKey(_currentState) &&
                   _allowedTransitions[_currentState].Contains(newState);
        }

        private void HandleStateSpecificActions(GameState previousState, GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;

                case GameState.Menu:
                    Time.timeScale = 1f;
                    break;
            }
        }
    }
}