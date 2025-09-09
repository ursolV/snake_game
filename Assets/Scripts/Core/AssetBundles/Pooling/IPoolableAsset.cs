namespace SnakeGame.Core.AssetBundles.Pooling
{
    public interface IPoolableAsset
    {
        void OnSpawnFromPool();
        void OnReturnToPool();
    }
}