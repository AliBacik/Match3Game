using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptable", menuName = "Scriptable Objects/LevelScriptable")]
public class LevelScriptable : ScriptableObject
{
    [System.Serializable]
    public class LevelFruit
    {
        public BaseFruit.FruitType FruitType;
        public Vector2Int position;
    }

    public List<LevelFruit> Fruits = new List<LevelFruit>();
    public List<Vector2Int> BlockedPositions = new List<Vector2Int>();

    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;

    public BaseFruit.FruitType ReturnFruitType(Vector2Int pos)
    {
        foreach (var fruit in Fruits)
        {
            if (pos == fruit.position)
            {
                return fruit.FruitType;
            }
        }

        return 0;
    }
}
