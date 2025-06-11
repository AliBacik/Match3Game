using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PoolManager : MonoBehaviour , IPoolObjectHandler
{
    private IGridManagerFruitRefAdder gridManagerRefAdder;
    private List<BaseFruit> FruitPool = new List<BaseFruit>();
    private List<string> prefabAddresses = new List<string>
    {
        "Apple",
        "Orange",
        "Blueberry",
        "Bomb",
        "Banana",
        "Pear"
    };

    public void Initialize(IGridManagerFruitRefAdder gridManager)
    {
        gridManagerRefAdder = gridManager;
    }

    void Start()
    {
        InstantiateFruits();
    }
    void InstantiateFruits()
    {
        foreach (var address in prefabAddresses)
        {
            string currentAddress = address;

            Addressables.LoadAssetAsync<GameObject>(address).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;

                    for (int i = 0; i < 25; i++)
                    {
                        GameObject fruit = Instantiate(handle.Result, Vector3.zero, Quaternion.identity);
                        fruit.SetActive(false);

                        BaseFruit baseFruit = fruit.GetComponent<BaseFruit>();

                        if(baseFruit != null)
                        {
                            FruitPool.Add(baseFruit);
                            AddFruitToGridManager(baseFruit);
                        }
                        else
                        {
                            Debug.LogWarning("BaseFruit componenti yok.");
                        }
                    }
                }
                else
                {
                    Debug.LogError("Prefab yüklenemedi");
                }
            };
        }
    }

    void AddFruitToGridManager(BaseFruit fruit)
    {
        gridManagerRefAdder?.AddReferanceToGridManager(fruit);
    }

    //Interface methods
    public BaseFruit GetFruitFromPool(BaseFruit.FruitType type)
    {
        List<int> inactiveIndices = new List<int>();
        for (int i = 0; i < FruitPool.Count; i++)
        {
            if (FruitPool[i] != null && !FruitPool[i].gameObject.activeInHierarchy && FruitPool[i]._FruitType==type)
                inactiveIndices.Add(i);
        }
        if (inactiveIndices.Count == 0)
            return null;

        int randomIndex = inactiveIndices[Random.Range(0, inactiveIndices.Count)];
        return FruitPool[randomIndex];
    }

    public BaseFruit GetRandomFruitFromPool()
    {
        List<int> inactiveIndices = new List<int>();
        for (int i = 0; i < FruitPool.Count; i++)
        {
            if (FruitPool[i] != null && !FruitPool[i].gameObject.activeInHierarchy)
                inactiveIndices.Add(i);
        }
        if (inactiveIndices.Count == 0)
            return null;

        int randomIndex = inactiveIndices[Random.Range(0, inactiveIndices.Count)];
        return FruitPool[randomIndex];
    }


}
