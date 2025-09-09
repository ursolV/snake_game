using UnityEngine;
using Cysharp.Threading.Tasks;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.Core.AssetBundles.Pooling
{
    public class PooledAssetFactory : AssetFactory, IPooledAssetFactory
    {
        private readonly IAssetPool _assetPool;

        public PooledAssetFactory(IAssetManager assetManager, IAssetPool assetPool) 
            : base(assetManager)
        {
            _assetPool = assetPool;
        }

        public async UniTask<GameObject> GetFromPoolAsync(string bundleName, string assetName)
        {
            var instance = await _assetPool.GetAsync(bundleName, assetName);
            if (instance != null)
            {
                RegisterInstance(instance);
            }
            return instance;
        }

        public void ReturnToPool(GameObject instance)
        {
            if (instance == null) return;

            UnregisterInstance(instance);
            _assetPool.Return(instance);
        }

        public new void DestroyInstance(GameObject instance)
        {
            ReturnToPool(instance);
        }

        public new void DestroyAllInstances()
        {
            _assetPool.ClearAllPools();
            _instantiatedObjects.Clear();
        }
    }
}