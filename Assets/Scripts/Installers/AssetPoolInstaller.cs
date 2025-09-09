using Zenject;
using SnakeGame.Core.AssetBundles;
using SnakeGame.Core.AssetBundles.Pooling;
using SnakeGame.Core.AssetBundles.Interfaces;

namespace SnakeGame.DI.Installers
{
    public class AssetPoolInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetBundleLoader>().To<AssetBundleLoader>().AsSingle();
            Container.Bind<IAssetManager>().To<AssetManager>().AsSingle();
            Container.Bind<IAssetFactory>().To<AssetFactory>().AsTransient();
            
            Container.Bind<IAssetPool>().To<AssetPool>().AsTransient();

            Container.Bind<IPooledAssetFactory>().To<PooledAssetFactory>().AsTransient();
        }
    }
}