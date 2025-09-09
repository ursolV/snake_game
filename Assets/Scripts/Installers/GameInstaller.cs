using SnakeGame.UI;
using SnakeGame.Core;
using UnityEngine;
using Zenject;
using SnakeGame.Gameplay.Controllers;
using SnakeGame.Core.Controllers;
using SnakeGame.Core.Models;

namespace SnakeGame.DI.Installers
{
    public class GameInstaller : MonoInstaller
{
    [SerializeField] private WindowManager _windowManagerPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameStateUIController>().AsSingle();
        Container.BindInterfacesTo<GameStateService>().AsSingle();
        Container.BindInterfacesTo<LoadingSceneController>().AsSingle().NonLazy();
        Container.Bind<IScoreService>().To<ScoreService>().AsSingle();
        Container.Bind<IDataStorage>().To<PlayerPrefsStorage>().AsSingle();
        
        Container.Bind<IWindowManager>()
            .FromComponentInNewPrefab(_windowManagerPrefab)
            .AsSingle()
            .NonLazy();
        }
    }
}
