using SnakeGame.Gameplay.Skinning;
using SnakeGame.Gameplay.View;
using UnityEngine;
using Zenject;

namespace SnakeGame.Gameplay.Services
{
    public class SnakeSkinHandler : MonoBehaviour
    {
        private ISkinService _skinService;
        [SerializeField]
        private SnakeView _snakeView;

        [Inject]
        public void Construct(ISkinService skinService)
        {
            _skinService = skinService;
            if (_skinService != null)
            {
                _skinService.SkinChanged += HandleSkinChanged;
            }
        }

        private void HandleSkinChanged(string skinId)
        {
            _snakeView.DisposeView();
            _snakeView.Render();
        }

        public void OnDestroy()
        {
            if (_skinService != null)
            {
                _skinService.SkinChanged -= HandleSkinChanged;
            }
        }
    }
}
