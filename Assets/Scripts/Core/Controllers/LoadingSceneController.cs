using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using SnakeGame.Core.Models;

namespace SnakeGame.Core.Controllers
{
    public class LoadingSceneController : IInitializable
    {
        [Inject]
        private readonly IGameStateService _gameStateService;

        private const float MinLoadingTime = 2f;

        private const string GameSceneName = "Game";

        public async void Initialize()
        {
            //if dev start the game not from the Init scene
            if (SceneManager.GetActiveScene().name != GameSceneName)
            {
                await UniTask.Delay((int)(MinLoadingTime * 1000));

                await SceneManager.LoadSceneAsync(GameSceneName).ToUniTask();
            }

            _gameStateService.ChangeState(GameState.Menu);
        }
    }
}