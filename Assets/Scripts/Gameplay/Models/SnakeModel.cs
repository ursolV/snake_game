using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame.Gameplay.Models
{
    public class SnakeModel
    {
        public LinkedList<Vector2Int> BodyPositions { get; } = new();
        public Vector2Int Direction { get; private set; } = Vector2Int.up;
        public Vector2Int NextDirection { get; private set; } = Vector2Int.up;
        
        private Vector2Int _lastTailPosition;
        
        public void Initialize(Vector2Int startPosition, int initialLength = 3)
        {
            BodyPositions.Clear();

            // Створюємо початкове тіло
            for (int i = 0; i < initialLength; i++)
            {
                BodyPositions.AddLast(startPosition - new Vector2Int(0, i));
            }

            Direction = Vector2Int.up;
            NextDirection = Vector2Int.up;
        }
        
        public void Move()
        {
            // Оновлюємо напрямок
            Direction = NextDirection;
            
            // Додаємо нову голову
            var newHeadPosition = BodyPositions.First.Value + Direction;
            BodyPositions.AddFirst(newHeadPosition);
            
            _lastTailPosition = BodyPositions.Last.Value;
            // Видаляємо хвіст
            BodyPositions.RemoveLast();
        }
        
        public void Grow()
        {
            BodyPositions.AddLast(_lastTailPosition);
        }
        
        public void ChangeDirection(Vector2Int newDirection)
        {
            // Заборона руху в протилежному напрямку
            if (newDirection != -Direction)
            {
                NextDirection = newDirection;
            }
        }
        
        public bool CheckSelfCollision()
        {
            // Перевіряємо чи голова не стикається з тілом
            var head = BodyPositions.First.Value;
            bool first = true;
            
            foreach (var segment in BodyPositions)
            {
                if (!first && segment == head)
                {
                    return true;
                }
                first = false;
            }
            
            return false;
        }
        
        public bool CheckWallCollision(Vector2Int gridSize)
        {
            var head = BodyPositions.First.Value;
            return head.x < 0 || head.x >= gridSize.x || head.y < 0 || head.y >= gridSize.y;
        }
    }
}