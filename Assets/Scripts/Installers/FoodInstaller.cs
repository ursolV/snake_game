using Zenject;
using UnityEngine;
using SnakeGame.Core.Configs;
using SnakeGame.Gameplay.Services;

namespace SnakeGame.DI.Installers
{
    public class FoodInstaller : MonoInstaller
    {
        [SerializeField] private FoodsConfig _foodConfigs;
        
        public override void InstallBindings()
        {
            // Біндинг конфігів
            Container.Bind<FoodsConfig>().FromInstance(_foodConfigs).AsSingle();
            
            // Біндинг сервісу
            Container.BindInterfacesTo<FoodService>().AsSingle();
        }
    }
}