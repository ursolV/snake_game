using System;
using System.Collections.Generic;
using SnakeGame.Gameplay.Skinning;
using SnakeGame.Gameplay.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;
using SnakeGame.Core;
using SnakeGame.Core.AssetBundles.Pooling;
using SnakeGame.Core.Configs;

namespace SnakeGame.Gameplay.Services
{
    public class FoodService : IFoodService, IDisposable
    {
        public event Action<Vector2Int, int> OnFoodCollected;
        
        private readonly IGridService _gridService;
        private readonly ISkinService _skinService;
        private readonly IScoreService _scoreService;
        private readonly IPooledAssetFactory _pooledAssetFactory;
        private readonly FoodsConfig _foodConfigs;
        
        private readonly Dictionary<Vector2Int, (FoodConfig foodConfig, GameObject foodObject)> _activeFoods = new();

        public FoodService(IGridService gridService, IPooledAssetFactory pooledAssetFactory, FoodsConfig foodConfigs, ISkinService skinService, IScoreService scoreService)
        {
            _gridService = gridService;
            _skinService = skinService;
            _scoreService = scoreService;
            _pooledAssetFactory = pooledAssetFactory;
            _foodConfigs = foodConfigs;

            _skinService.SkinChanged += HandleSkinChanged;
        }
        
        private void HandleSkinChanged(string skinId)
        {
            RespawnFood();
        }

        private async void RespawnFood()
        {
            foreach (var position in _activeFoods.Keys)
            {
                _pooledAssetFactory.ReturnToPool(_activeFoods[position].foodObject);
            }

            _pooledAssetFactory.DestroyAllInstances();

            var positions = new List<Vector2Int>(_activeFoods.Keys);
            foreach (var position in positions)
            {
                var food = _activeFoods[position];
                var id = food.foodConfig.foodName;

                var newFoodObject = await _pooledAssetFactory.GetFromPoolAsync(_skinService.CurrentSkinId, id);
                newFoodObject.transform.position = _gridService.GridToWorldPosition(position);
                _activeFoods[position] = (food.foodConfig, newFoodObject);
            }
        }

        public async void SpawnFood(Vector2Int gridPosition)
        {
            if (_activeFoods.ContainsKey(gridPosition) || !_gridService.IsPositionEmpty(gridPosition))
                return;

            var worldPosition = _gridService.GridToWorldPosition(gridPosition);
            var foodConfig = GetRandomFoodConfig();
            var foodObject = await _pooledAssetFactory.GetFromPoolAsync(_skinService.CurrentSkinId, foodConfig.foodName);
            foodObject.transform.position = worldPosition;

            _activeFoods[gridPosition] = (foodConfig, foodObject);
            _gridService.OccupyPosition(gridPosition);
        }
        
        private FoodConfig GetRandomFoodConfig()
        {
            float totalWeight = 0f;
            foreach (var config in _foodConfigs.configs)
            {
                totalWeight += config.spawnWeight;
            }

            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var config in _foodConfigs.configs)
            {
                currentWeight += config.spawnWeight;
                if (randomValue <= currentWeight)
                {
                    return config;
                }
            }

            return _foodConfigs.configs[0];
        }

        public bool TryCollectFood(Vector2Int gridPosition, out FoodConfig foodConfig)
        {
            foodConfig = null;

            if (!_activeFoods.ContainsKey(gridPosition))
                return false;

            // Отримаємо конфіг їжі
            if (_activeFoods.TryGetValue(gridPosition, out var food))
            {
                foodConfig = food.foodConfig;
                // Нараховуємо очки
                _scoreService.AddScore(foodConfig.scoreValue);

                // Викликаємо подію
                OnFoodCollected?.Invoke(gridPosition, foodConfig.scoreValue);

                RemoveFood(gridPosition);
                return true;
            }

            return false;
        }

        public void RemoveFood(Vector2Int gridPosition)
        {
            if (_activeFoods.TryGetValue(gridPosition, out var food))
            {
                _pooledAssetFactory.ReturnToPool(food.foodObject);
                _activeFoods.Remove(gridPosition);
                _gridService.FreePosition(gridPosition);
            }
        }

        public void ClearAllFood()
        {
            foreach (var position in _activeFoods.Keys)
            {
                _pooledAssetFactory.ReturnToPool(_activeFoods[position].foodObject);
                _gridService.FreePosition(position);
            }
            
            _activeFoods.Clear();
        }

        public void Dispose()
        {
            _skinService.SkinChanged -= HandleSkinChanged;
        }
    }
}