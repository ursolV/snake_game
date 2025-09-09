# Snake Game
Playable version of the classic “Snake” game from old Nokia phones

## How to run
1. Open the project in Unity editor (2022.3.59 or any newer 2022.3 LTS version).
2. Open the `Assets/Scenes/Init.unity` scene.
3. Press the Play button.

## Controls
Use arrow keys or WASD to control the snake.

## Unit Tests
The project includes unit tests to verify core gameplay mechanics.

### How to run tests
1. In the Unity editor, go to `Window > General > Test Runner`.
2. In the Test Runner window, you will see `PlayMode` and `EditMode` tests.
3. You can run all tests by clicking `Run All`, or select specific tests or groups to run.

### Included Tests
#### PlayMode
- **SnakeGrowsOnFoodEatenTest**: Verifies that the snake's length increases by exactly one segment after eating food.
- **SnakeSelfCollisionTest**: Ensures that the game ends when the snake collides with itself.
#### EditMode
- **GridModelTests**: Verifies that food does not spawn on a grid cell that is already occupied by the snake.

## Asset Bundles
All skins are loaded from asset bundles. Currently, all skins are located locally in the `StreamingAssets` folder.

### How to build AssetBundles
1. In the Unity editor, go to `Snake Game/Build AssetBundles`.
2. This will create asset bundles in the `Assets/StreamingAssets` folder, in a subdirectory for the current platform.

### How to add a new skin
1. Go to `Assets/Configs/SkinCatalog.asset` and add a new entry. The `Skin Id` should be a unique name for your new skin (e.g., `MyNewSkin`), and the `Name` will be its display name.
2. Create new folders with the same `Skin Id` name in the following locations:
    - `Assets/Prefabs/Grid/Food/`
    - `Assets/Prefabs/Grid/Snake/`
    - `Assets/Sprites/Grid/Background/`
3. For every new folder you've created, select it and in the Inspector, assign the AssetBundle name to be the same as your `Skin Id`.
4. Create the necessary assets for the skin by duplicating and modifying existing assets from other skin folders (like `Default` or `HD`):
    - In `Assets/Prefabs/Grid/Food/[YourSkinId]/`, create food prefabs. The names must match the food names in other skin folders.
    - In `Assets/Prefabs/Grid/Snake/[YourSkinId]/`, create snake part prefabs (e.g., `Head`, `Body`, `Tail`). The names must match those in other skin folders.
    - In `Assets/Sprites/Grid/Background/[YourSkinId]/`, create background sprites. The names must match those in other skin folders.
5. Build the asset bundles by following the "How to build AssetBundles" section.
6. The new skin will now be available in the game.

### How to add a new food
1. Go to `Assets/Configs/FoodConfigs.asset`.
2. Add a new entry to the `Configs` list with the following settings:
    - `Food Name`: A unique name for the new food.
    - `Score Value`: The number of points the player receives.
    - `Spawn Weight`: The probability of this food appearing.
3. In the `Assets/Prefabs/Grid/Food/` directory, you will find subdirectories for each skin type (e.g., `Default`, `HD`).
4. For each skin subdirectory, create a new prefab for the food item. The prefab's name must exactly match the `Food Name` you specified in the `FoodConfigs.asset`.
