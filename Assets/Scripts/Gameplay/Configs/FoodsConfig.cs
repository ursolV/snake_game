using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame.Core.Configs
{
    [CreateAssetMenu(fileName = "FoodConfigs", menuName = "Snake Game/Food Configs")]
    public class FoodsConfig : ScriptableObject
    {
        public List<FoodConfig> configs = new List<FoodConfig>();

        public FoodConfig GetConfig(string foodName)
        {
            return configs.Find(c => c.foodName == foodName);
        }
    }
}

[System.Serializable]
public class FoodConfig
{
    public string foodName;
    public int scoreValue;
    public float spawnWeight;
}