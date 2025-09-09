using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Zenject;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.Core.AssetBundles.Pooling
{
    public class AssetPool : IAssetPool, IInitializable
    {
        private readonly IAssetManager _assetManager;
        private readonly Dictionary<string, Dictionary<string, Queue<GameObject>>> _pools = new();
        private readonly Transform _poolContainer;

        public AssetPool(IAssetManager assetManager)
        {
            _assetManager = assetManager;
            _poolContainer = new GameObject("AssetPoolContainer").transform;
            _poolContainer.gameObject.SetActive(false);
        }

        public void Initialize()
        {
        }

        public async UniTask<GameObject> GetAsync(string bundleName, string assetName)
        {
            var poolKey = GetPoolKey(bundleName, assetName);
            
            if (TryGetFromPool(poolKey, assetName, out var instance))
            {
                PrepareInstance(instance);
                return instance;
            }

            return await CreateNewInstanceAsync(bundleName, assetName);
        }

        public void Return(GameObject instance)
        {
            if (instance == null) return;

            var poolKey = GetPoolKeyFromInstance(instance);
            ReturnToPool(instance, poolKey);
        }

        public async UniTask Prewarm(string bundleName, string assetName, int count)
        {
            var poolKey = GetPoolKey(bundleName, assetName);
            
            InitializePool(poolKey, assetName);

            for (int i = 0; i < count; i++)
            {
                var instance = await CreateNewInstanceAsync(bundleName, assetName, true);
                _pools[poolKey][assetName].Enqueue(instance);
            }
        }

        public void ClearPool(string bundleName, string assetName)
        {
            var poolKey = GetPoolKey(bundleName, assetName);
            
            if (_pools.TryGetValue(poolKey, out var assetPools) &&
                assetPools.TryGetValue(assetName, out var pool))
            {
                while (pool.Count > 0)
                {
                    var instance = pool.Dequeue();
                    GameObject.Destroy(instance);
                }
                assetPools.Remove(assetName);
            }
        }

        public void ClearAllPools()
        {
            foreach (var assetPools in _pools.Values)
            {
                foreach (var pool in assetPools.Values)
                {
                    while (pool.Count > 0)
                    {
                        var instance = pool.Dequeue();
                        GameObject.Destroy(instance);
                    }
                }
            }
            _pools.Clear();
        }

        private bool TryGetFromPool(string poolKey, string assetName, out GameObject instance)
        {
            instance = null;
            
            if (_pools.TryGetValue(poolKey, out var assetPools) &&
                assetPools.TryGetValue(assetName, out var pool) &&
                pool.Count > 0)
            {
                instance = pool.Dequeue();
                return true;
            }
            
            return false;
        }

        private async UniTask<GameObject> CreateNewInstanceAsync(string bundleName, string assetName, bool forPool = false)
        {
            var prefab = await _assetManager.LoadAssetAsync<GameObject>(bundleName, assetName);
            if (prefab == null) return null;

            var instance = GameObject.Instantiate(prefab, forPool ? _poolContainer : null);
            instance.name = GetPoolKey(bundleName, assetName);
            
            if (forPool)
            {
                instance.SetActive(false);
            }
            
            return instance;
        }

        private void ReturnToPool(GameObject instance, string poolKey)
        {
            var assetName = GetAssetNameFromInstance(instance);
            
            InitializePool(poolKey, assetName);

            var poolable = instance.GetComponent<IPoolableAsset>();
            poolable?.OnReturnToPool();

            instance.SetActive(false);
            instance.transform.SetParent(_poolContainer);
            instance.transform.position = Vector3.zero;
            
            _pools[poolKey][assetName].Enqueue(instance);
        }

        private void PrepareInstance(GameObject instance)
        {
            instance.SetActive(true);
            instance.transform.SetParent(null);
            
            var poolable = instance.GetComponent<IPoolableAsset>();
            poolable?.OnSpawnFromPool();
        }

        private void InitializePool(string poolKey, string assetName)
        {
            if (!_pools.ContainsKey(poolKey))
            {
                _pools[poolKey] = new Dictionary<string, Queue<GameObject>>();
            }

            if (!_pools[poolKey].ContainsKey(assetName))
            {
                _pools[poolKey][assetName] = new Queue<GameObject>();
            }
        }

        private string GetPoolKey(string bundleName, string assetName)
        {
            return $"{bundleName}_{assetName}";
        }

        private string GetPoolKeyFromInstance(GameObject instance)
        {
            return instance.name.Replace("(Clone)", "").Trim();
        }

        private string GetAssetNameFromInstance(GameObject instance)
        {
            return GetPoolKeyFromInstance(instance).Split('_')[^1];
        }
    }
}