using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace SnakeGame.Core.Services
{
    public interface ISnakeService
    {
        event System.Action OnSnakeMoved;
        event System.Action OnSnakeGrew;
        event System.Action OnSnakeDied;

        Vector2Int GetHeadPosition();
        LinkedList<Vector2Int> GetBodyPositions();
        Vector2Int GetDirection();

        void Initialize(Vector2Int startPosition);
        void ChangeDirection(Vector2Int direction);
        void Grow();
        bool CheckCollisions();

        UniTask StartMovementAsync();
        void StopMovement();
        void MoveOneStep();
    }
}