using UnityEngine;
using Zenject;
using SnakeGame.Gameplay.Interfaces;

namespace SnakeGame.Gameplay.Camera
{
    public class CameraGridFitter : MonoBehaviour
    {
        [Header("Camera References")]
        [SerializeField] private UnityEngine.Camera _camera;

        [Header("Padding Settings")]
        [SerializeField] private float _generalPadding = 1f;
        [SerializeField] private float _extraTopPadding = 0.5f;

        [Header("Settings")]
        [SerializeField] private bool _fitOnStart = true;
        [SerializeField] private bool _maintainAspectRatio = true;

        private IGridService _gridService;

        [Inject]
        public void Construct(IGridService gridService)
        {
            _gridService = gridService;
        }

        private void Start()
        {
            InitializeCamera();

            if (_fitOnStart)
            {
                FitCameraToGrid();
            }
        }

        private void InitializeCamera()
        {
            if (_camera == null)
                _camera = UnityEngine.Camera.main;

            if (_camera == null)
            {
                Debug.LogError("Camera reference is missing!");
                return;
            }

            if (!_camera.orthographic)
            {
                _camera.orthographic = true;
            }
        }

        public void FitCameraToGrid()
        {
            if (_camera == null || _gridService == null)
                return;

            var gridSize = _gridService.GridSize;
            var cellSize = _gridService.CellSize;

            if (gridSize.x <= 0 || gridSize.y <= 0 || cellSize.x <= 0 || cellSize.y <= 0)
                return;

            // Розраховуємо реальні розміри сітки в юнітах
            var gridWorldWidth = gridSize.x * cellSize.x;
            var gridWorldHeight = gridSize.y * cellSize.y;

            // Розраховуємо загальні відступи
            float totalHorizontalPadding = _generalPadding * 2f;
            float totalVerticalPadding = _generalPadding * 2f + _extraTopPadding;

            // Додаємо padding
            var targetWidth = gridWorldWidth + totalHorizontalPadding;
            var targetHeight = gridWorldHeight + totalVerticalPadding;

            // Отримаємо співвідношення сторін камери
            var screenAspect = _camera.aspect;
            float orthoSize;

            if (_maintainAspectRatio)
            {
                // Підганяємо під обидві осі з збереженням пропорцій
                if (targetWidth / screenAspect > targetHeight)
                {
                    // Ширина обмежує - підганяємо по ширині
                    orthoSize = targetWidth / (2f * screenAspect);
                }
                else
                {
                    // Висота обмежує - підганяємо по висоті
                    orthoSize = targetHeight / 2f;
                }
            }
            else
            {
                // Просто підганяємо по висоті
                orthoSize = targetHeight / 2f;
            }

            _camera.orthographicSize = orthoSize;

            // Центруємо камеру - тепер правильно для top padding
            var cameraCenter = CalculateCameraCenter(gridWorldWidth, gridWorldHeight);

            _camera.transform.position = cameraCenter;

            Debug.Log($"Camera fitted: Position={cameraCenter}, Size={orthoSize}, " +
                     $"GeneralPadding={_generalPadding}, ExtraTopPadding={_extraTopPadding}");
        }

        private Vector3 CalculateCameraCenter(float gridWidth, float gridHeight)
        {
            float centerX = gridWidth / 2f;

            // Правильне центрування для top padding:
            // Камера має бути вище, щоб зверху було більше місця
            float centerY = gridHeight / 2f + _extraTopPadding / 2f;

            return new Vector3(centerX, centerY, _camera.transform.position.z);
        }
    }
}