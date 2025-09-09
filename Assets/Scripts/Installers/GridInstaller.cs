using Zenject;
using UnityEngine;
using SnakeGame.Gameplay.Interfaces;
using SnakeGame.Core.Configs;
using SnakeGame.Gameplay.Services;

namespace SnakeGame.DI.Installers
{
    public class GridInstaller : MonoInstaller
    {
        [SerializeField] private GridConfig gridConfig;
        
        public override void InstallBindings()
        {
            // Біндинг конфігурації
            Container.BindInstance(gridConfig).AsSingle();
            
            // Біндинг сервісу сітки
            Container.Bind<IGridService>().FromMethod(CreateGridService).AsSingle();
        }

        private IGridService CreateGridService()
        {
            return new GridService(gridConfig.gridSize, gridConfig.cellSize);
        }
    }
}