using UnityEngine;
using SnakeGame.Gameplay.Interfaces;
using System.Collections.Generic;
using SnakeGame.Gameplay.Models;

namespace SnakeGame.Gameplay.Services
{
    public class GridService : IGridService
    {
        public Vector2Int GridSize => _gridModel.GridSize;
        public Vector2 CellSize => _gridModel.CellSize;
        public Vector2 GridWorldSize => _gridModel.GridWorldSize;

        private readonly GridModel _gridModel;

        public GridService(Vector2Int gridSize, Vector2 cellSize)
        {
            _gridModel = new GridModel(gridSize, cellSize);
        }

        public Vector2 GridToWorldPosition(Vector2Int gridPosition)
        {
            // Нижній лівий кут клітинки в (0,0), центр клітинки зміщений на половину розміру
            return new Vector2(
                gridPosition.x * CellSize.x + CellSize.x / 2f,
                gridPosition.y * CellSize.y + CellSize.y / 2f
            );
        }

        public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        {
            // Конвертуємо world position до grid position (округлення вниз)
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x / CellSize.x),
                Mathf.FloorToInt(worldPosition.y / CellSize.y)
            );
        }

        public bool IsPositionValid(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < GridSize.x &&
                   gridPosition.y >= 0 && gridPosition.y < GridSize.y;
        }

        public bool IsPositionEmpty(Vector2Int gridPosition) 
            => _gridModel.IsPositionEmpty(gridPosition);

        public Vector2Int GetRandomEmptyPosition() 
            => _gridModel.GetRandomEmptyPosition();

        public void OccupyPosition(Vector2Int gridPosition) 
            => _gridModel.OccupyPosition(gridPosition);

        public void FreePosition(Vector2Int gridPosition) 
            => _gridModel.FreePosition(gridPosition);

        public void ClearAllOccupancies() 
            => _gridModel.ClearAllOccupancies();

        public IEnumerable<Vector2Int> GetOccupiedPositions()
        {
            return _gridModel.GetOccupiedPositions();
        }
    }
}