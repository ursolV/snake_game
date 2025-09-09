using UnityEngine;

namespace SnakeGame.Gameplay.Interfaces
{
    public interface IFoodService
    {
        event System.Action<Vector2Int, int> OnFoodCollected;
        void SpawnFood(Vector2Int gridPosition);
        bool TryCollectFood(Vector2Int gridPosition, out FoodConfig foodConfig);
        void RemoveFood(Vector2Int gridPosition);
        void ClearAllFood();
    }
}