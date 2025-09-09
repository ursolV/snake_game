using UnityEngine;

namespace Snake.Gameplay.Cameras
{
    /// <summary>
    /// Скрипт для автоматичного позиціонування та розтягування SpriteRenderer в режимі tiled
    /// щоб він займав всю область видимості 2D камери
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class TiledSpriteFitter : MonoBehaviour
    {
        [SerializeField] 
        private Camera targetCamera;
        
        [SerializeField] 
        private bool fitOnStart = true;
        
        [SerializeField] 
        private bool fitOnUpdate = false;

        private SpriteRenderer spriteRenderer;
        private Vector2 lastCameraSize;
        private Vector3 lastCameraPosition;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }
        }

        private void Start()
        {
            if (fitOnStart)
            {
                FitToCamera();
            }
        }

        private void Update()
        {
            if (fitOnUpdate)
            {
                CheckCameraChanges();
            }
        }

        /// <summary>
        /// Перевіряє чи змінилася камера і оновлює позиціонування якщо потрібно
        /// </summary>
        private void CheckCameraChanges()
        {
            if (targetCamera == null) 
            {
                return;
            }

            var currentCameraSize = GetCameraSize();
            var currentCameraPosition = targetCamera.transform.position;

            if (currentCameraSize != lastCameraSize || currentCameraPosition != lastCameraPosition)
            {
                FitToCamera();
                lastCameraSize = currentCameraSize;
                lastCameraPosition = currentCameraPosition;
            }
        }

        /// <summary>
        /// Позиціонує та розтягує SpriteRenderer щоб він займав всю область видимості камери
        /// </summary>
        public void FitToCamera()
        {
            if (targetCamera == null || spriteRenderer == null) 
            {
                return;
            }

            // Отримуємо розміри камери в світових координатах
            var cameraSize = GetCameraSize();
            
            // Встановлюємо позицію спрайта в центр камери
            transform.position = new Vector3(
                targetCamera.transform.position.x,
                targetCamera.transform.position.y,
                transform.position.z
            );

            // Встановлюємо розмір спрайта рівним розміру камери
            // Для режиму tiled це означає що спрайт буде повторюватися
            spriteRenderer.size = cameraSize;

            // Оновлюємо збережені значення
            lastCameraSize = cameraSize;
            lastCameraPosition = targetCamera.transform.position;
        }

        /// <summary>
        /// Отримує розміри камери в світових координатах
        /// </summary>
        private Vector2 GetCameraSize()
        {
            if (targetCamera == null) 
            {
                return Vector2.zero;
            }

            var height = 2f * targetCamera.orthographicSize;
            var width = height * targetCamera.aspect;
            
            return new Vector2(width, height);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying && spriteRenderer != null)
            {
                FitToCamera();
            }
        }
#endif
    }
}