using UnityEngine;
using Zenject;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.Gameplay.Skinning
{
    public class SkinBackgroundLoader : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
        [SerializeField] private string _backgroundAssetName = "field-back";

        private ISkinService _skinService;
        private IAssetManager _assetManager;
        
        private string _currentSkinId;

        [Inject]
        private void Construct(ISkinService skinService, IAssetManager assetManager)
        {
            _skinService = skinService;
            _assetManager = assetManager;
        }

        private void Start()
        {
            if (_skinService != null)
            {
                _skinService.SkinChanged += HandleSkinChanged;
                // Load initial background
                LoadBackgroundForSkin(_skinService.CurrentSkinId);
            }
        }

        private void ReleaseCurrentBackgroundAsset()
        {
            if (!string.IsNullOrEmpty(_currentSkinId) && _assetManager != null)
            {
                _assetManager.ReleaseAsset(_currentSkinId, _backgroundAssetName);
            }
        }

        private void OnDestroy()
        {
            if (_skinService != null)
            {
                _skinService.SkinChanged -= HandleSkinChanged;
            }
            ReleaseCurrentBackgroundAsset();
        }

        private void HandleSkinChanged(string skinId)
        {
            ReleaseCurrentBackgroundAsset();
            LoadBackgroundForSkin(skinId);
        }

        private async void LoadBackgroundForSkin(string skinId)
        {
            if (string.IsNullOrEmpty(skinId) || _backgroundSpriteRenderer == null) return;
            
            _currentSkinId = skinId;
            try
            {
                var sprite = await _assetManager.LoadAssetAsync<Sprite>(skinId, _backgroundAssetName);
                if (sprite != null)
                {
                    _backgroundSpriteRenderer.sprite = sprite;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to load background sprite for skin {skinId}: {e.Message}");
            }
        }
    }
}
