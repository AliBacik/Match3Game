using UnityEngine;

public interface IPoolObjectHandler
{
    BaseFruit GetFruitFromPool(BaseFruit.FruitType type);
    BaseFruit GetRandomFruitFromPool();
}
