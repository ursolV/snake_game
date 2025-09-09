using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SnakeGame.Core.AssetBundles.Interfaces
{
    public interface IAssetManager
    {
        UniTask<T> LoadAssetAsync<T>(string bundleName, string assetName) where T : Object;
        UniTask PreloadBundleAsync(string bundleName);
        void ReleaseAsset(string bundleName, string assetName);
        void ReleaseBundle(string bundleName);
        bool IsAssetLoaded(string bundleName, string assetName);
        Dictionary<string, Object> GetLoadedAssets(string bundleName);
    }
}