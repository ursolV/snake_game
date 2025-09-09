using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame.Gameplay.Interfaces
{
    public interface IGridService
    {
        Vector2Int GridSize { get; }
        Vector2 CellSize { get; }
        Vector2 GridWorldSize { get; }
        
        Vector2 GridToWorldPosition(Vector2Int gridPosition);
        Vector2Int WorldToGridPosition(Vector2 worldPosition);
        bool IsPositionValid(Vector2Int gridPosition);
        bool IsPositionEmpty(Vector2Int gridPosition);
        Vector2Int GetRandomEmptyPosition();
        void OccupyPosition(Vector2Int gridPosition);
        void FreePosition(Vector2Int gridPosition);
        void ClearAllOccupancies();
        IEnumerable<Vector2Int> GetOccupiedPositions();
    }
}