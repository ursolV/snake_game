using Cysharp.Threading.Tasks;
using SnakeGame.Core.AssetBundles.Interfaces;
using UnityEngine;

namespace SnakeGame.Core.AssetBundles.Pooling
{
    public interface IPooledAssetFactory : IAssetFactory
    {
        UniTask<GameObject> GetFromPoolAsync(string bundleName, string assetName);
        void ReturnToPool(GameObject instance);
    }
}