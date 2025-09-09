using System;

using SnakeGame.Core;

namespace SnakeGame.Gameplay.Skinning
{
    public class SkinService : ISkinService
    {
        private readonly SkinCatalog _catalogAsset;
        private readonly IDataStorage _storage;

        private string _currentSkinId;
        public event Action<string> SkinChanged;

        private const string StorageKey = "SkinId";

        public SkinService(SkinCatalog catalogAsset, IDataStorage storage)
        {
            _catalogAsset = catalogAsset;
            _storage = storage;

            if (_storage != null)
            {
                var savedId = _storage.GetString(StorageKey, "default");
                if (!string.IsNullOrEmpty(savedId))
                    _currentSkinId = savedId;
            }
        }

        public string CurrentSkinId => _currentSkinId;

        public bool SetSkin(string skinId)
        {
            _currentSkinId = skinId;

            // Persist selection by id
            if (_storage != null)
            {
                _storage.SetString(StorageKey, skinId);
                _storage.Save();
            }
            SkinChanged?.Invoke(_currentSkinId);
            return true;
        }

        public string GetBundleName(string skinId)
        {
            if (string.IsNullOrEmpty(skinId)) return null;

            if (_catalogAsset != null && _catalogAsset.TryResolve(skinId, out var name))
            {
                return name;
            }

            // Fallback to convention
            return skinId;
        }
    }
}


