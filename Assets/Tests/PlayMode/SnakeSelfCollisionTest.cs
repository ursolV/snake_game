using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject;
using SnakeGame.Gameplay;
using SnakeGame.Core;
using SnakeGame.Core.Services;
using SnakeGame.Core.Models;

public class SnakeSelfCollisionTest
{
    [UnityTest]
    public IEnumerator SnakeCollidesWithItself_EndsGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Game");

        // Wait for the scene to load completely
        yield return null;

        // Find the GameController in the scene
        var gameController = GameObject.FindObjectOfType<GameController>();
        Assert.IsNotNull(gameController, "GameController not found in the scene.");

        // Get the snake and game state service from the DI container
        var snake = ProjectContext.Instance.Container.Resolve<ISnakeService>();
        var gameStateService = ProjectContext.Instance.Container.Resolve<IGameStateService>();

        // Start the game
        gameStateService.ChangeState(GameState.Playing);

        yield return null;

        // Force the snake to grow
        snake.Grow();
        snake.MoveOneStep();
        snake.Grow();
        snake.MoveOneStep();
        snake.Grow();
        snake.MoveOneStep();

        // Now, make the snake turn into itself
        snake.ChangeDirection(Vector2Int.left);
        snake.MoveOneStep();

        snake.ChangeDirection(Vector2Int.down);
        snake.MoveOneStep();

        snake.ChangeDirection(Vector2Int.right);
        snake.MoveOneStep();

        // The snake should have collided with its body, and the game should be over
        Assert.AreEqual(GameState.GameOver, gameStateService.CurrentState, "Game should be over after snake collides with itself.");
    }
}