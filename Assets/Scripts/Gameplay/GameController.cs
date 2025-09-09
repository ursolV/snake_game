using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using SnakeGame.Core.Services;
using SnakeGame.Gameplay.Interfaces;
using SnakeGame.Gameplay.View;
using SnakeGame.Core;
using SnakeGame.Core.AssetBundles.Interfaces;
using SnakeGame.Core.AssetBundles.Pooling;
using SnakeGame.Gameplay.Skinning;
using SnakeGame.Core.Models;

namespace SnakeGame.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private SnakeView _snakeView;

        private ISnakeService _snakeService;
        private IFoodService _foodService;
        private IGridService _gridService;
        private IGameStateService _gameStateService;
        private IScoreService _scoreService;
        private IAssetFactory _assetFactory;
        private IPooledAssetFactory _pooledAssetFactory;
        private ISkinService _skinService;

        [Inject]
        public void Construct(
            ISnakeService snakeService,
            IFoodService foodService,
            IGridService gridService,
            IGameStateService gameStateService,
            IScoreService scoreService,
            IAssetFactory assetFactory,
            IPooledAssetFactory pooledAssetFactory,
            ISkinService skinService)
        {
            _snakeService = snakeService;
            _foodService = foodService;
            _gridService = gridService;
            _gameStateService = gameStateService;
            _scoreService = scoreService;
            _assetFactory = assetFactory;
            _pooledAssetFactory = pooledAssetFactory;
            _skinService = skinService;
        }

        private void Awake()
        {
            _snakeView.Initialize(_assetFactory, _pooledAssetFactory, _gridService, _skinService, _snakeService);
        }

        private void Start()
        {
            _snakeService.OnSnakeMoved += HandleSnakeMoved;
            _snakeService.OnSnakeDied += HandleSnakeDied;
            _gameStateService.OnStateChanged += HandleGameStateChanged;
        }

        private async UniTaskVoid StartGame()
        {
            _scoreService.ResetScore();

            var startPosition = new Vector2Int(_gridService.GridSize.x / 2, 0);
            _snakeService.Initialize(startPosition);
            _foodService.ClearAllFood();

            _foodService.SpawnFood(_gridService.GetRandomEmptyPosition());
            await _snakeService.StartMovementAsync();
        }

        void HandleGameStateChanged(GameState previousState, GameState state)
        {
            if(state == GameState.Playing && previousState != GameState.Paused)
            {
                StartGame().Forget();
            }
        }

        private void HandleSnakeMoved()
        {
            CheckFoodCollision();
        }

        private void CheckFoodCollision()
        {
            if(_foodService.TryCollectFood(_snakeService.GetHeadPosition(), out var foodConfig))
            {
                OnFoodCollected(_snakeService.GetHeadPosition(), foodConfig.scoreValue);
            }
        }

        public void OnFoodCollected(Vector2Int position, int score)
        {
            _snakeService.Grow();
            _foodService.SpawnFood(_gridService.GetRandomEmptyPosition());
        }

        public void HandleSnakeDied()
        {
            _gameStateService.ChangeState(GameState.GameOver);
            Debug.Log("Game Over!");
        }

        private void OnDestroy()
        {
            _snakeService.OnSnakeMoved -= HandleSnakeMoved;
            _snakeService.OnSnakeDied -= HandleSnakeDied;
            _snakeService.StopMovement();
        }
    }
}