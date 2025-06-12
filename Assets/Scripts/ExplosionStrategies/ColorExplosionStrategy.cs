using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ColorExplosionStrategy : IExplosionStrategy
{
    public void TileExplode(Vector2Int position, IGridManagerGridController grid, IPlayerInteractStatus playerStatus)
    {
        if (!grid.GridGetValue(position)) return;

        var startFruit = grid.GridGetValue(position);
        List<BaseFruit> matched = new List<BaseFruit>();
        HashSet<Vector2Int> visited = new();
        Queue<Vector2Int> queue = new();

        var type = startFruit._FruitType;
        queue.Enqueue(position);
        visited.Add(position);

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            if (grid.GridGetValue(currentPos))
                matched.Add(grid.GridGetValue(currentPos));

            foreach (var dir in directions)
            {
                var neighbor = currentPos + dir;
                if (visited.Contains(neighbor)) continue;

                if (grid.GridGetValue(neighbor) &&
                    grid.GridGetValue(neighbor)._FruitType == type)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        if (matched.Count < 3) return;

        playerStatus.TriggerScore(matched.Count); 

        foreach (var fruit in matched)
        {
            fruit.OnExplode();

            grid.EmptyGrid(fruit.GridPosition.x, fruit.GridPosition.y);
            grid.GridRemoveFromMap(fruit.GridPosition);

            fruit.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack)
                .OnComplete(() => fruit.gameObject.SetActive(false));
        }

        playerStatus.TriggerPostBehaviours();
    }
}
    
  

