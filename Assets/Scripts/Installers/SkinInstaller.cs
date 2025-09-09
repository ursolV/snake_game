using SnakeGame.Gameplay.Skinning;
using UnityEngine;
using Zenject;

namespace SnakeGame.DI.Installers
{
    public class SkinInstaller : MonoInstaller
    {
        [SerializeField] private SkinCatalog _catalogAsset;

        public override void InstallBindings()
        {
            Container.BindInstance(_catalogAsset).IfNotBound();
            Container.Bind<ISkinService>().To<SkinService>().AsSingle();
        }
    }
}


