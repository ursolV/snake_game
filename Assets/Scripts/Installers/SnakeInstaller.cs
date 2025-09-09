using Zenject;
using SnakeGame.Core.Services;
using SnakeGame.Gameplay.Input;
using SnakeGame.Gameplay.Interfaces;
using UnityEngine;
using SnakeGame.Gameplay.Services;

namespace SnakeGame.DI.Installers
{
    public class SnakeInstaller : MonoInstaller
    {
        [SerializeField] private float _snakeMoveDelay = 0.2f;
        
        public override void InstallBindings()
        {
            Container.Bind<ISnakeService>()
                .To<SnakeService>()
                .FromMethod(() => new SnakeService(
                    Container.Resolve<IGridService>(),
                    _snakeMoveDelay
                ))
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesTo<InputController>().AsSingle();
        }
    }
}