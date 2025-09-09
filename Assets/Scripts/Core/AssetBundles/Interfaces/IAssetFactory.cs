using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SnakeGame.Core.AssetBundles.Interfaces
{
    public interface IAssetFactory
    {
        UniTask<GameObject> CreateFromBundleAsync(string bundleName, string assetName, Transform parent = null);
        UniTask<T> InstantiateFromBundleAsync<T>(string bundleName, string assetName, Transform parent = null) where T : Component;
        void DestroyInstance(GameObject instance);
        void DestroyAllInstances();
    }
}