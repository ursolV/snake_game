using UnityEngine;
using Cysharp.Threading.Tasks;

namespace SnakeGame.Core.AssetBundles.Pooling
{
    public interface IAssetPool
    {
        UniTask<GameObject> GetAsync(string bundleName, string assetName);
        void Return(GameObject instance);
        UniTask Prewarm(string bundleName, string assetName, int count);
        void ClearPool(string bundleName, string assetName);
        void ClearAllPools();
    }
}