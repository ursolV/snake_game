using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SnakeGame.Core.Services;
using SnakeGame.Gameplay.Interfaces;
using SnakeGame.Gameplay.Models;
using UnityEngine;

namespace SnakeGame.Gameplay.Services
{
public class SnakeService : ISnakeService, IDisposable
{
    public event Action OnSnakeMoved;
    public event Action OnSnakeGrew;
    public event Action OnSnakeDied;

    private readonly SnakeModel _snakeModel;
    private readonly IGridService _gridService;
    private readonly float _moveDelay;

    private CancellationTokenSource _movementCts;
    private readonly CancellationTokenSource _lifetimeCts = new();
    private bool _isMoving = false;

    public Vector2Int GetHeadPosition() => _snakeModel.BodyPositions.First.Value;

    public LinkedList<Vector2Int> GetBodyPositions()=> _snakeModel.BodyPositions;

    public Vector2Int GetDirection()=> _snakeModel.Direction;

    public SnakeService(IGridService gridService, float moveDelay = 0.2f)
    {
        _snakeModel = new SnakeModel();
        _gridService = gridService;
        _moveDelay = moveDelay;
    }

    public void Initialize(Vector2Int startPosition)
    {
        _snakeModel.Initialize(startPosition);
    }

    public async UniTask StartMovementAsync()
    {
        StopMovement(); // Зупиняємо попередній рух

        _isMoving = true;
        _movementCts = new CancellationTokenSource();

        await MovementLoop(_movementCts.Token);
    }

    public void StopMovement()
    {
        _isMoving = false;
        _movementCts?.Cancel();
        _movementCts?.Dispose();
        _movementCts = null;
    }

    public void ChangeDirection(Vector2Int direction)
    {
        _snakeModel.ChangeDirection(direction);
    }

    public async UniTask MoveAsync()
    {
        ClearOccupancies();

        _snakeModel.Move();

        if (CheckCollisions())
        {
            OnSnakeDied?.Invoke();
            StopMovement();
            return;
        }

        OccupyPositions();

        OnSnakeMoved?.Invoke();
        await UniTask.Yield();
    }
    
    public void MoveOneStep()
    {
        MoveAsync().Forget();
    }

    private void ClearOccupancies()
        {
            foreach (var pos in _snakeModel.BodyPositions)
            {
                _gridService.FreePosition(pos);
            }
        }

    private void OccupyPositions()
    {
        foreach (var pos in _snakeModel.BodyPositions)
        {
            _gridService.OccupyPosition(pos);
        }
    }

    public void Grow()
    {
        _snakeModel.Grow();
        //if grow, we need to re-occupy the last tail position
        OccupyPositions();
        OnSnakeGrew?.Invoke();
    }

    public bool CheckCollisions()
    {
        return _snakeModel.CheckSelfCollision() ||
               _snakeModel.CheckWallCollision(_gridService.GridSize);
    }

    public void Dispose()
    {
        StopMovement();
        _lifetimeCts?.Cancel();
        _lifetimeCts?.Dispose();
    }

    private async UniTask MovementLoop(CancellationToken ct)
    {
        // Комбінуємо токени для безпечного завершення
        var linkedCt = CancellationTokenSource.CreateLinkedTokenSource(
            ct,
            _lifetimeCts.Token
        ).Token;

        while (_isMoving && !linkedCt.IsCancellationRequested)
        {
            await MoveAsync();
            await UniTask.Delay(
                (int)(_moveDelay * 1000),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update,
                linkedCt
            );
        }
    }
}
}