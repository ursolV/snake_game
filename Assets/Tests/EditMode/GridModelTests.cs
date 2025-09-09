using NUnit.Framework;
using SnakeGame.Gameplay.Models;
using UnityEngine;

namespace SnakeGame.Tests
{
    public class GridModelTests
    {
        [Test]
        public void FoodDoesNotSpawnOnOccupiedCells()
        {
            // Arrange
            var gridSize = new Vector2Int(5, 5);
            var cellSize = new Vector2(1, 1);
            var gridModel = new GridModel(gridSize, cellSize);

            // Occupy some positions
            var occupiedPositions = new[]
            {
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 3)
            };
            foreach (var pos in occupiedPositions)
                gridModel.OccupyPosition(pos);

            // Act
            for (int i = 0; i < 20; i++) // Try multiple times for randomness
            {
                var foodPos = gridModel.GetRandomEmptyPosition();
                // Assert
                Assert.IsFalse(System.Array.Exists(occupiedPositions, p => p == foodPos),
                    $"Food spawned on occupied cell: {foodPos}");
            }
        }
    }
}
