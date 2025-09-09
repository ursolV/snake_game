using System;
using SnakeGame.Core.Models;

namespace SnakeGame.Core
{
    public interface IGameStateService
    {
        GameState CurrentState { get; }
        event Action<GameState, GameState> OnStateChanged;
        void ChangeState(GameState newState);
        void Pause();
        void Resume();
        bool IsState(GameState state);
    }
}
