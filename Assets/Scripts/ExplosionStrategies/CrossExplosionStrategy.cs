using DG.Tweening;
using UnityEngine;

public class CrossExplosionStrategy : IExplosionStrategy
{
    public void TileExplode(Vector2Int position, IGridManagerGridController grid,IPlayerInteractStatus playerStatus)
    {
        for (int x = 0; x < grid.ReturnWidth(); x++)
        {
            Vector2Int targetPos = new Vector2Int(x, position.y);
            var fruit = grid.GridGetValue(targetPos);
            if (fruit != null)
            {
                EmptyGridAndMapHandling(fruit, grid);
                DisableGameObject(fruit);
            }
        }

        for (int y = 0; y < grid.ReturnHeight(); y++)
        {
            Vector2Int targetPos = new Vector2Int(position.x, y);
            var fruit = grid.GridGetValue(targetPos);
            if (fruit != null)
            {
                EmptyGridAndMapHandling(fruit, grid);
                DisableGameObject(fruit);
            }
        }

        playerStatus.TriggerPostBehaviours();
    }

    void EmptyGridAndMapHandling(BaseFruit fruit, IGridManagerGridController grid)
    {
        grid.EmptyGrid(fruit.GridPosition.x, fruit.GridPosition.y);
        grid.GridRemoveFromMap(fruit.GridPosition);
    }
    public void DisableGameObject(BaseFruit fruit)
    {
        fruit.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                fruit.gameObject.SetActive(false);
            });
    }
}

