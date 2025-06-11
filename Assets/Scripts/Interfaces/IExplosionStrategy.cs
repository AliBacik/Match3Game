using UnityEngine;

public interface IExplosionStrategy
{
    void TileExplode(Vector2Int position, IGridManagerGridController grid,IPlayerInteractStatus playerStatus);
}
