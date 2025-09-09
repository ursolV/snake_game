using System.Collections.Generic;
using UnityEngine;
using SnakeGame.Gameplay.Interfaces;
using SnakeGame.Core.Services;
using SnakeGame.Core.AssetBundles.Interfaces;
using SnakeGame.Core.AssetBundles.Pooling;
using SnakeGame.Gameplay.Skinning;

namespace SnakeGame.Gameplay.View
{
    public class SnakeView : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        private IAssetFactory _assetFactory;
        private IGridService _gridService;
        private IPooledAssetFactory _pooledAssetFactory;
        private List<GameObject> _segments = new();
        private GameObject _head;
        private GameObject _tail;

        private ISkinService _skinService;
        private ISnakeService _snakeService;

        public void Initialize(IAssetFactory assetFactory, IPooledAssetFactory pooledAssetFactory, IGridService gridService, ISkinService skinService, ISnakeService snakeService)
        {
            _assetFactory = assetFactory;
            _pooledAssetFactory = pooledAssetFactory;
            _gridService = gridService;
            _skinService = skinService;
            _snakeService = snakeService;

            _snakeService.OnSnakeMoved += Render;
            _snakeService.OnSnakeGrew += Render;
        }

        // Сам рендер зроблено не дуже оптимально, бажано оптимізувати
        public async void Render()
        {
            Clear();
            var positions = _snakeService.GetBodyPositions();
            var direction = _snakeService.GetDirection();
            if (positions.Count == 0) return;
            var posList = new List<Vector2Int>(positions);

            // Head
            _head = await _assetFactory.CreateFromBundleAsync(_skinService.CurrentSkinId, "Head");
            _head.transform.position = _gridService.GridToWorldPosition(posList[0]);
            _head.transform.rotation = Quaternion.Euler(0, 0,
                direction == Vector2Int.up ? 270 :
                direction == Vector2Int.down ? 90 :
                direction == Vector2Int.left ? 0 : 180);
            _head.transform.SetParent(_parent, false);
            _segments.Add(_head);

            // Body
            for (int i = 1; i < posList.Count - 1; i++)
            {
                var body = await _pooledAssetFactory.GetFromPoolAsync(_skinService.CurrentSkinId, "Body");
                body.transform.position = _gridService.GridToWorldPosition(posList[i]);
                body.transform.rotation = Quaternion.Euler(0, 0,
                    posList[i - 1] - posList[i] == Vector2Int.up ? 270 :
                    posList[i - 1] - posList[i] == Vector2Int.down ? 90 :
                    posList[i - 1] - posList[i] == Vector2Int.left ? 0 : 180);
                body.transform.SetParent(_parent, false);
                _segments.Add(body);
            }

            // Tail
            if (posList.Count > 1)
            {
                _tail = await _assetFactory.CreateFromBundleAsync(_skinService.CurrentSkinId, "Tail");
                _tail.transform.position = _gridService.GridToWorldPosition(posList[^1]);
                _tail.transform.rotation = Quaternion.Euler(0, 0,
                    posList[^2] - posList[^1] == Vector2Int.up ? 270 :
                    posList[^2] - posList[^1] == Vector2Int.down ? 90 :
                    posList[^2] - posList[^1] == Vector2Int.left ? 0 : 180);
                _tail.transform.SetParent(_parent, false);
                _segments.Add(_tail);
            }
        }

        public void DisposeView()
        {
            Clear();
            _assetFactory.DestroyAllInstances();
            _pooledAssetFactory.DestroyAllInstances();
        }

        public void Clear()
        {
            foreach (var seg in _segments)
            {
                if (seg == _head || seg == _tail)
                    _assetFactory.DestroyInstance(seg);
                else
                    _pooledAssetFactory.ReturnToPool(seg);
            }
            _segments.Clear();
            _head = null;
            _tail = null;
        }

        private void OnDestroy()
        {
            DisposeView();
            
            _snakeService.OnSnakeMoved -= Render;
            _snakeService.OnSnakeGrew -= Render;
        }
    }
}
