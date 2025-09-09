using UnityEngine;

namespace SnakeGame.Core.Configs
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "Snake Game/Grid Config")]
    public class GridConfig : ScriptableObject
    {
        [Header("Grid Dimensions")]
        public Vector2Int gridSize = new Vector2Int(20, 20);
        public Vector2 cellSize = new Vector2(1f, 1f);
        
        [Header("Visual Settings - Gameplay")]
        public bool showGridDuringGameplay = true;
        public Color gridLineColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        public Color gridBorderColor = new Color(1f, 1f, 1f, 0.5f);
        [Range(0.0f, 0.1f)]
        public float gridLineWidth = 0.02f;
        [Range(0.0f, 0.3f)]
        public float gridBorderWidth = 0.05f;
    }
}