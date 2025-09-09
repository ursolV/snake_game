using SnakeGame.Core.AssetBundles.Pooling;

namespace SnakeGame.Gameplay.Pooling
{
    public class PoolableSnakeSegment : PoolableMonoBehaviour
    {
        public override void OnSpawnFromPool()
        {
            base.OnSpawnFromPool();
        }

        public override void OnReturnToPool()
        {
            base.OnReturnToPool();
        }
    }
}