using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame.Gameplay.Models
{
    public class GridModel
    {
        public Vector2Int GridSize { get; private set; }
        public Vector2 CellSize { get; private set; }
        public Vector2 GridWorldSize { get; private set; }
        
        private readonly HashSet<Vector2Int> _occupiedPositions = new();

        public GridModel(Vector2Int gridSize, Vector2 cellSize)
        {
            GridSize = gridSize;
            CellSize = cellSize;
            GridWorldSize = new Vector2(gridSize.x * cellSize.x, gridSize.y * cellSize.y);
        }

        public Vector2 GridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector2(
                gridPosition.x * CellSize.x + CellSize.x / 2f,
                gridPosition.y * CellSize.y + CellSize.y / 2f
            );
        }

        public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        {
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
        {
            return IsPositionValid(gridPosition) && !_occupiedPositions.Contains(gridPosition);
        }

        public Vector2Int GetRandomEmptyPosition()
        {
            var emptyPositions = new List<Vector2Int>();
            
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    var position = new Vector2Int(x, y);
                    if (IsPositionEmpty(position))
                    {
                        emptyPositions.Add(position);
                    }
                }
            }

            if (emptyPositions.Count == 0)
                return Vector2Int.zero;

            return emptyPositions[Random.Range(0,emptyPositions.Count)];
        }

        public void OccupyPosition(Vector2Int gridPosition)
        {
            if (IsPositionValid(gridPosition))
            {
                _occupiedPositions.Add(gridPosition);
            }
        }

        public void FreePosition(Vector2Int gridPosition)
        {
            _occupiedPositions.Remove(gridPosition);
        }

        public void ClearAllOccupancies()
        {
            _occupiedPositions.Clear();
        }

        public int GetEmptyCellsCount()
        {
            int count = 0;
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    if (IsPositionEmpty(new Vector2Int(x, y)))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        internal IEnumerable<Vector2Int> GetOccupiedPositions()
        {
            return _occupiedPositions;
        }
    }
}