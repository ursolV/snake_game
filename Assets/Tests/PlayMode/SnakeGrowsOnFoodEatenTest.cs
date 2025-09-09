
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject;
using SnakeGame.Gameplay;
using SnakeGame.Core;
using SnakeGame.Core.Services;
using SnakeGame.Gameplay.Interfaces;
using SnakeGame.Core.Models;

public class SnakeGrowsOnFoodEatenTest
{
    [UnityTest]
    public IEnumerator WhenSnakeEatsFood_LengthIncreases()
    {
        // Load the game scene
        SceneManager.LoadScene("Game");

        // Wait for the scene to load completely
        yield return null;

        // Find the GameController in the scene
        var gameController = GameObject.FindObjectOfType<GameController>();
        Assert.IsNotNull(gameController, "GameController not found in the scene.");

        // Get services from the DI container
        var snake = ProjectContext.Instance.Container.Resolve<ISnakeService>();
        var gameStateService = ProjectContext.Instance.Container.Resolve<IGameStateService>();
        var foodService = ProjectContext.Instance.Container.Resolve<IFoodService>();

        // Start the game
        gameStateService.ChangeState(GameState.Playing);
        yield return null;

        // Get the initial length of the snake
        int initialLength = snake.GetBodyPositions().Count;

        // Spawn food right in front of the snake
        var snakeHeadPosition = snake.GetHeadPosition();
        var foodPosition = snakeHeadPosition + snake.GetDirection();

        // Clear existing food and spawn one at a specific location
        foodService.ClearAllFood();
        foodService.SpawnFood(foodPosition);
        yield return new WaitForSeconds(.2f);

        snake.MoveOneStep();

        // Check that the snake's length has increased by one
        Assert.AreEqual(initialLength + 1, snake.GetBodyPositions().Count, "Snake length should increase by one after eating food.");

        // Check that the game is still in the Playing state
        Assert.AreEqual(GameState.Playing, gameStateService.CurrentState, "Game should still be in 'Playing' state.");
    }
}