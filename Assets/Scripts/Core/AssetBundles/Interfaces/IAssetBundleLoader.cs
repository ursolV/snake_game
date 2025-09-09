using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SnakeGame.Core.AssetBundles.Interfaces
{
    public interface IAssetBundleLoader
    {
        UniTask<AssetBundle> LoadBundleAsync(string bundleName);
        UniTask<T> LoadAssetAsync<T>(string bundleName, string assetName) where T : Object;
        UniTask<List<T>> LoadAllAssetsAsync<T>(string bundleName) where T : Object;
        void UnloadBundle(string bundleName);
        void UnloadAllBundles();
        bool IsBundleLoaded(string bundleName);
        AssetBundle GetLoadedBundle(string bundleName);
    }
}