using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.Core.AssetBundles
{
    public class AssetFactory : IAssetFactory
    {
        private readonly IAssetManager _assetManager;
        protected readonly Dictionary<string, GameObject> _instantiatedObjects = new();

        public AssetFactory(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public async UniTask<GameObject> CreateFromBundleAsync(string bundleName, string assetName, Transform parent = null)
        {
            var prefab = await _assetManager.LoadAssetAsync<GameObject>(bundleName, assetName);
            if (prefab == null) return null;

            var instance = GameObject.Instantiate(prefab, parent);
            RegisterInstance(instance);
            
            return instance;
        }

        public async UniTask<T> InstantiateFromBundleAsync<T>(string bundleName, string assetName, Transform parent = null) where T : Component
        {
            var instance = await CreateFromBundleAsync(bundleName, assetName, parent);
            return instance?.GetComponent<T>();
        }

        public void DestroyInstance(GameObject instance)
        {
            if (instance == null) return;

            UnregisterInstance(instance);
            GameObject.Destroy(instance);
        }

        public void DestroyAllInstances()
        {
            foreach (var instance in _instantiatedObjects.Values)
            {
                if (instance != null)
                {
                    GameObject.Destroy(instance);
                }
            }
            _instantiatedObjects.Clear();
        }

        protected void RegisterInstance(GameObject instance)
        {
            _instantiatedObjects[GetInstanceKey(instance)] = instance;
        }

        protected void UnregisterInstance(GameObject instance)
        {
            _instantiatedObjects.Remove(GetInstanceKey(instance));
        }

        private string GetInstanceKey(GameObject instance)
        {
            return $"{instance.GetInstanceID()}";
        }
    }
}