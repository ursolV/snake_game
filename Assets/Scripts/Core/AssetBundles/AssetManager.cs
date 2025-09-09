using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.Core.AssetBundles
{
    public class AssetManager : IAssetManager
    {
        private readonly IAssetBundleLoader _bundleLoader;
        private readonly Dictionary<string, Dictionary<string, Object>> _loadedAssets = new();

        public AssetManager(IAssetBundleLoader bundleLoader)
        {
            _bundleLoader = bundleLoader;
        }

        public async UniTask<T> LoadAssetAsync<T>(string bundleName, string assetName) where T : Object
        {
            // Перевіряємо чи вже завантажено
            if (_loadedAssets.TryGetValue(bundleName, out var bundleAssets) &&
                bundleAssets.TryGetValue(assetName, out var existingAsset))
            {
                return existingAsset as T;
            }

            // Завантажуємо з бандлу
            var asset = await _bundleLoader.LoadAssetAsync<T>(bundleName, assetName);
            if (asset == null) return null;

            // Кешуємо
            if (!_loadedAssets.ContainsKey(bundleName))
            {
                _loadedAssets[bundleName] = new Dictionary<string, Object>();
            }

            _loadedAssets[bundleName][assetName] = asset;
            return asset;
        }

        public async UniTask PreloadBundleAsync(string bundleName)
        {
            await _bundleLoader.LoadBundleAsync(bundleName);
        }

        public void ReleaseAsset(string bundleName, string assetName)
        {
            if (_loadedAssets.TryGetValue(bundleName, out var bundleAssets))
            {
                bundleAssets.Remove(assetName);
                
                if (bundleAssets.Count == 0)
                {
                    _loadedAssets.Remove(bundleName);
                    _bundleLoader.UnloadBundle(bundleName);
                }
            }
        }

        public void ReleaseBundle(string bundleName)
        {
            _loadedAssets.Remove(bundleName);
            _bundleLoader.UnloadBundle(bundleName);
        }

        public bool IsAssetLoaded(string bundleName, string assetName)
        {
            return _loadedAssets.TryGetValue(bundleName, out var bundleAssets) &&
                   bundleAssets.ContainsKey(assetName);
        }

        public Dictionary<string, Object> GetLoadedAssets(string bundleName)
        {
            _loadedAssets.TryGetValue(bundleName, out var assets);
            return assets;
        }
    }
}