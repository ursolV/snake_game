using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.Core.AssetBundles
{
    public class AssetBundleLoader : IAssetBundleLoader
    {
        private readonly Dictionary<string, AssetBundle> _loadedBundles = new();
        private readonly Dictionary<string, string> _bundlePaths = new();
        
        private string _streamingAssetsPath;
        
        private bool _bundleLoadingInProgress = false;

        public AssetBundleLoader()
        {
            _streamingAssetsPath = Application.streamingAssetsPath;
        }

        public async UniTask<AssetBundle> LoadBundleAsync(string bundleName)
        {
            if (_bundleLoadingInProgress)
            {
                await UniTask.WaitWhile(() => _bundleLoadingInProgress);
            }

            if (_loadedBundles.TryGetValue(bundleName, out var existingBundle))
                return existingBundle;

            var bundlePath = GetBundlePath(bundleName);

            if (!File.Exists(bundlePath))
            {
                Debug.LogError($"AssetBundle not found: {bundlePath}");
                return null;
            }

            _bundleLoadingInProgress = true;
            var loadTask = AssetBundle.LoadFromFileAsync(bundlePath);
            await loadTask;

            _bundleLoadingInProgress = false;

            if (loadTask.assetBundle == null)
            {
                Debug.LogError($"Failed to load bundle: {bundleName}");
                return null;
            }

            _loadedBundles[bundleName] = loadTask.assetBundle;

            return loadTask.assetBundle;
        }

        public async UniTask<T> LoadAssetAsync<T>(string bundleName, string assetName) where T : Object
        {
            var bundle = await LoadBundleAsync(bundleName);
            if (bundle == null) return null;

            var loadTask = bundle.LoadAssetAsync<T>(assetName);
            await loadTask;

            if (loadTask.asset == null)
            {
                Debug.LogError($"Asset '{assetName}' not found in bundle '{bundleName}'");
                return null;
            }

            return loadTask.asset as T;
        }

        public async UniTask<List<T>> LoadAllAssetsAsync<T>(string bundleName) where T : Object
        {
            var bundle = await LoadBundleAsync(bundleName);
            if (bundle == null) return new List<T>();

            var loadTask = bundle.LoadAllAssetsAsync<T>();
            await loadTask;

            var assets = new List<T>();
            foreach (var asset in loadTask.allAssets)
            {
                if (asset is T typedAsset)
                {
                    assets.Add(typedAsset);
                }
            }

            return assets;
        }

        public void UnloadBundle(string bundleName)
        {
            if (_loadedBundles.TryGetValue(bundleName, out var bundle))
            {
                bundle.Unload(false);
                _loadedBundles.Remove(bundleName);
            }
        }

        public void UnloadAllBundles()
        {
            foreach (var bundle in _loadedBundles.Values)
            {
                bundle.Unload(false);
            }
            _loadedBundles.Clear();
        }

        public bool IsBundleLoaded(string bundleName)
        {
            return _loadedBundles.ContainsKey(bundleName);
        }

        public AssetBundle GetLoadedBundle(string bundleName)
        {
            _loadedBundles.TryGetValue(bundleName, out var bundle);
            return bundle;
        }

        private string GetPlatformForAssetBundles()
        {
#if UNITY_EDITOR_OSX
            return RuntimePlatform.OSXPlayer.ToString();
#elif UNITY_EDITOR_WIN
            return RuntimePlatform.WindowsPlayer.ToString();
#else
            return Application.platform.ToString();
#endif
        }

        private string GetBundlePath(string bundleName)
        {
            string platformDirectory = GetPlatformForAssetBundles();
            return Path.Combine(_streamingAssetsPath, platformDirectory, bundleName.ToLower());
        }

        public void RegisterBundlePath(string bundleName, string customPath = null)
        {
            string platformDirectory = GetPlatformForAssetBundles();
            _bundlePaths[bundleName] = customPath ?? Path.Combine(_streamingAssetsPath, platformDirectory, bundleName);
        }
    }
}