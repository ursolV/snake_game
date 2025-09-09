using UnityEngine;
using Zenject;
using System.Collections.Generic;
using SnakeGame.Core.Configs;
using SnakeGame.Gameplay.Interfaces;

namespace SnakeGame.Gameplay.Grid
{
    public class SpriteGridRenderer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Sprite _gridLineSprite;
        [SerializeField] private Transform _gridTransforn;

        private IGridService _gridService;
        private GridConfig _gridConfig;

        [Inject]
        public void Construct(IGridService gridService, GridConfig gridConfig)
        {
            _gridService = gridService;
            _gridConfig = gridConfig;
        }

        private List<GameObject> _gridObjects = new List<GameObject>();
        private bool _isVisible = true;

        private void Start()
        {
            CreateGridVisualization();
        }

        private void CreateGridVisualization()
        {
            ClearGridVisualization();

            if (!_isVisible)
                return;

            var gridSize = _gridService.GridSize;
            var cellSize = _gridService.CellSize;

            if (_gridConfig.showGridDuringGameplay)
            {   // Створюємо сітку
                CreateGridLines(gridSize, cellSize);
            }

            // Створюємо border
            CreateBorder(gridSize, cellSize);
        }

        private void CreateGridLines(Vector2Int gridSize, Vector2 cellSize)
        {
            if (_gridConfig.gridLineWidth <= 0.001f)
                return;

            // Вертикальні лінії
            for (int x = 1; x < gridSize.x; x++)
            {
                float xPos = x * cellSize.x;
                CreateLine(
                    new Vector3(xPos, gridSize.y * cellSize.y / 2f, 0f),
                    new Vector3(_gridConfig.gridLineWidth, gridSize.y * cellSize.y, 1f),
                    _gridConfig.gridLineColor,
                    "GridLine_Vertical_" + x
                );
            }

            // Горизонтальні лінії
            for (int y = 1; y < gridSize.y; y++)
            {
                float yPos = y * cellSize.y;
                CreateLine(
                    new Vector3(gridSize.x * cellSize.x / 2f, yPos, 0f),
                    new Vector3(gridSize.x * cellSize.x, _gridConfig.gridLineWidth, 1f),
                    _gridConfig.gridLineColor,
                    "GridLine_Horizontal_" + y
                );
            }
        }

        private void CreateBorder(Vector2Int gridSize, Vector2 cellSize)
        {
            if (_gridConfig.gridBorderWidth <= 0.001f)
                return;

            float width = gridSize.x * cellSize.x;
            float height = gridSize.y * cellSize.y;
            float thickness = _gridConfig.gridBorderWidth;

            // Розраховуємо довжину з перекриттям (2 товщини)
            float horizontalLength = width + thickness * 2f;
            float verticalLength = height + thickness * 2f;

            // Нижній border - розтягуємо на дві товщини в обидві сторони
            CreateLine(
                new Vector3(width / 2f, -thickness / 2f, 0f),
                new Vector3(horizontalLength, thickness, 1f),
                _gridConfig.gridBorderColor,
                "Border_Bottom"
            );

            // Правий border - розтягуємо на дві товщини в обидві сторони
            CreateLine(
                new Vector3(width + thickness / 2f, height / 2f, 0f),
                new Vector3(thickness, verticalLength, 1f),
                _gridConfig.gridBorderColor,
                "Border_Right"
            );

            // Верхній border - розтягуємо на дві товщини в обидві сторони
            CreateLine(
                new Vector3(width / 2f, height + thickness / 2f, 0f),
                new Vector3(horizontalLength, thickness, 1f),
                _gridConfig.gridBorderColor,
                "Border_Top"
            );

            // Лівий border - розтягуємо на дві товщини в обидві сторони
            CreateLine(
                new Vector3(-thickness / 2f, height / 2f, 0f),
                new Vector3(thickness, verticalLength, 1f),
                _gridConfig.gridBorderColor,
                "Border_Left"
            );
        }

        private void CreateLine(Vector3 position, Vector3 scale, Color color, string name = "GridLine")
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(_gridTransforn);
            obj.transform.position = position;
            obj.transform.localScale = scale;

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = _gridLineSprite ?? CreateDefaultSprite();
            renderer.color = color;
            renderer.sortingOrder = -10;

            _gridObjects.Add(obj);
        }

        private Sprite CreateDefaultSprite()
        {
            // Створюємо простий білий спрайт
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        }

        private void ClearGridVisualization()
        {
            foreach (var obj in _gridObjects)
            {
                if (obj != null) Destroy(obj);
            }
            _gridObjects.Clear();
        }

        public void SetVisible(bool visible)
        {
            _isVisible = visible;
            foreach (var obj in _gridObjects)
            {
                if (obj != null) obj.SetActive(visible);
            }
        }

        public void RefreshGrid()
        {
            CreateGridVisualization();
        }

        public void UpdateColors(Color gridColor, Color borderColor)
        {
            foreach (var obj in _gridObjects)
            {
                if (obj == null) continue;

                var renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    if (obj.name.Contains("GridLine"))
                        renderer.color = gridColor;
                    else if (obj.name.Contains("Border"))
                        renderer.color = borderColor;
                }
            }
        }

#if UNITY_EDITOR
        [SerializeField] bool _debugShowOccupiedCells = false;

        private List<GameObject> _debugObjects = new List<GameObject>();

        private void Update()
        {
            if (_debugShowOccupiedCells)
            {
                DebugShowOccupiedCells(_gridService.GetOccupiedPositions());
            }
            else
            {
                ClearDebugVisualization();
            }
        }

        /// <summary>
        /// Debug: Показати зайняті клітинки на гріді (напівпрозорі квадрати).
        /// </summary>
        /// <param name="occupiedCells">Список зайнятих клітинок (позиції в гріді)</param>
        /// <param name="color">Колір для зайнятих клітинок (наприклад, напівпрозорий червоний)</param>
        public void DebugShowOccupiedCells(IEnumerable<Vector2Int> occupiedCells, Color? color = null)
        {
            ClearDebugVisualization();
            var cellSize = _gridService.CellSize;
            Color debugColor = color ?? new Color(1f, 0f, 0f, 0.4f); // напівпрозорий червоний
            foreach (var cell in occupiedCells)
            {
                var obj = new GameObject($"DebugCell_{cell.x}_{cell.y}");
                obj.transform.SetParent(_gridTransforn);
                obj.transform.position = new Vector3((cell.x + 0.5f) * cellSize.x, (cell.y + 0.5f) * cellSize.y, 0f);
                obj.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1f);
                var renderer = obj.AddComponent<SpriteRenderer>();
                renderer.sprite = _gridLineSprite ?? CreateDefaultSprite();
                renderer.color = debugColor;
                renderer.sortingOrder = 10; // поверх гріда
                _debugObjects.Add(obj);
            }
        }

        /// <summary>
        /// Debug: Очистити debug-візуалізацію зайнятих клітинок.
        /// </summary>
        public void ClearDebugVisualization()
        {
            foreach (var obj in _debugObjects)
            {
                if (obj != null) Destroy(obj);
            }
            _debugObjects.Clear();
        }
#endif
    }
}