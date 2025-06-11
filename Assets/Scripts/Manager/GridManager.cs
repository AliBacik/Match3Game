using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour , IGridManagerFruitRefAdder , IGridManagerGridController, IFloodFillMatchHandler
{
    //Interfaces
    private IPlayerInteractStatus _playerInteractStatus;
    private IPoolObjectHandler _poolObjectHandler;

    [SerializeField] private LevelScriptable LevelScriptable;

    private int width = 0;
    private int height = 0;

    public float Xspacing = 0.62f;
    public float Yspacing = 0.5f;

    private GameObject[,] grid;
    private int zDepth = 0;

    [Header("Positions to Skip")]
    public List<Vector2Int> BlockedPositions = new List<Vector2Int>();
    public Dictionary<Vector2Int, BaseFruit> FruitMap = new Dictionary<Vector2Int,BaseFruit>();
    public List<BaseFruit> baseFruits = new List<BaseFruit>();

    [SerializeField] private bool isMatchPossible=true;
    [SerializeField] private int AvailableMatchCounts = 0;

    private bool gridCreated = false;

    //Event
    public static Action LevelStart;

    Vector2Int[] directions = new Vector2Int[]
   {
        Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right
   };
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Initialize(IPlayerInteractStatus playerManager, IPoolObjectHandler poolManager)
    {
        _playerInteractStatus = playerManager;
        _poolObjectHandler = poolManager;
    }

    void LevelStartAction()
    {
        LevelStart?.Invoke();
    }

    public void InitializeSetup()
    {
        if (gridCreated) return;

        if (!gridCreated)
        {
            width = LevelScriptable.width;
            height = LevelScriptable.height;

            BlockedPositions.Clear();
            BlockedPositions = LevelScriptable.BlockedPositions;

            gridCreated=true;
            GenerateGrid();

            LevelStartAction(); //trigger event
        }
    }

    public void GenerateGrid()
    {
        grid = new GameObject[width, height];

        Vector3 gridOffset = new Vector3((width - 1) * Xspacing * 0.5f, (height - 1) * Yspacing * 0.5f, zDepth);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int currentPos = new Vector2Int(x, y);

                if (BlockedPositions.Contains(currentPos)) // if that position blocked in the level , just continue
                {
                    grid[x, y] = null;
                    continue;
                }

                SpawnFruitAt(x, y, gridOffset); 
            }
        }
        //Check if any match possible
       StartCoroutine(CheckIfMatchPossible()); 
    }

    void SpawnFruitAt(int x, int y, Vector3 gridOffset)
    {
        Vector3 spawnPos = new Vector3(x * Xspacing, y * Yspacing, zDepth) - gridOffset;
        Vector2Int pos = new Vector2Int(x, y);

        BaseFruit.FruitType type = LevelScriptable.ReturnFruitType(pos);
        GameObject fruit = _poolObjectHandler.GetFruitFromPool(type).gameObject;

        if(fruit == null)
        {
            Debug.Log("fruit is null");
        }

        BaseFruit baseFruit = fruit.GetComponent<BaseFruit>();  //class referance
        Collider2D colliderFruit = fruit.GetComponent<Collider2D>(); //collider referance

        if (baseFruit != null)
        {
            baseFruit.GridPosition = new Vector2Int(x, y);
            Debug.Log(baseFruit.GridPosition);
        }

        FruitMap[pos] = baseFruit; //add referances to the dictionary
        grid[x, y] = fruit;

        //Activate in the scene
        fruit.transform.position = spawnPos;
        fruit.transform.rotation = Quaternion.identity;
        fruit.transform.localScale = Vector3.zero;
        fruit.SetActive(true);
        fruit.transform.DOScale(baseFruit.DefaultScale,0.35f);
    }

    bool HasAnyValidMove()
    {
        foreach (var pair in FruitMap)
        {
            var fruit = pair.Value;
            var matched = GetAllConnectedFruits(fruit);

            if (matched.Count >= 3)
                return true;
        }
        return false;
    }

    IEnumerator ShuffleGrid()
    {
        List<BaseFruit> allFruits = new List<BaseFruit>();
        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (BlockedPositions.Contains(pos)) continue;
                if (!FruitMap.ContainsKey(pos)) continue;

                allFruits.Add(FruitMap[pos]);
                validPositions.Add(pos);
            }
        }

        
        foreach (var pos in validPositions)
        {
            grid[pos.x, pos.y] = null;
            FruitMap.Remove(pos);
        }

     
        for (int i = 0; i < allFruits.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, allFruits.Count);
            (allFruits[i], allFruits[randIndex]) = (allFruits[randIndex], allFruits[i]);
        }

        
        for (int i = 0; i < validPositions.Count; i++)
        {
            Vector2Int pos = validPositions[i];
            BaseFruit fruit = allFruits[i];

            fruit.GridPosition = pos;
            fruit.transform.DOMove(GridToWorld(pos.x, pos.y), 0.8f); 

            grid[pos.x, pos.y] = fruit.gameObject;
            FruitMap[pos] = fruit;
        }

        return null;
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(
            x * Xspacing - (width - 1) * Xspacing * 0.5f,
            y * Yspacing - (height - 1) * Yspacing * 0.5f,
            0
        );
    }

    #region Interface Methods
    //Interface methods
    public IEnumerator CollapseFruits()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                Vector2Int currentPos = new Vector2Int(x, y);

                if (BlockedPositions.Contains(currentPos))
                    continue;

                if (grid[x, y] != null)
                {
                    int newY = y;

                    while (newY > 0 && grid[x, newY - 1] == null && !BlockedPositions.Contains(new Vector2Int(x, newY - 1)))
                    {
                        newY--;
                    }

                    if (newY != y)
                    {

                        grid[x, newY] = grid[x, y];
                        grid[x, y] = null;


                        var fruit = FruitMap[currentPos];
                        FruitMap.Remove(currentPos);

                        Vector2Int newPos = new Vector2Int(x, newY);
                        FruitMap[newPos] = fruit;
                        fruit.GridPosition = newPos;

                        fruit.transform.DOMove(GridToWorld(newPos.x, newPos.y), 0.7f).SetEase(Ease.InOutBack); //Animation

                    }
                }
            }
        }

        return null;
    }
    public IEnumerator FillEmptyGrids()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int currentPos = new Vector2Int(x, y);

                if (BlockedPositions.Contains(currentPos))
                    continue;

                if (grid[currentPos.x, currentPos.y] == null)
                {
                    GameObject fruit = _poolObjectHandler.GetRandomFruitFromPool().gameObject;
                    if (fruit != null)
                    {
                        BaseFruit baseFruit = null;

                        for (int i = 0; i < baseFruits.Count; i++)
                        {
                            if (baseFruits[i].gameObject == fruit)
                            {
                                baseFruit = baseFruits[i];
                                FruitMap[currentPos] = baseFruit;
                                break;
                            }
                        }
                        if (baseFruit == null) { Debug.Log("fruit is null"); }
                        baseFruit.GridPosition = currentPos;

                        Vector3 spawnPos = GridToWorld(x, height + 1);
                        Vector3 targetPos = GridToWorld(x, y);

                        baseFruit.transform.position = spawnPos;
                        baseFruit.gameObject.SetActive(true);
                        baseFruit.transform.DOMove(targetPos, 0.7f);

                        grid[x, y] = fruit;
                    }
                }


            }
        }
        return null;
    }
    public IEnumerator CheckIfMatchPossible()
    {
        isMatchPossible = HasAnyValidMove();

        if (isMatchPossible == false)
        {
            yield return ShuffleGrid();
            StartCoroutine(CheckIfMatchPossible());
        }
        else
        {
            _playerInteractStatus.PlayerStatusSwitch(true);
        }
    }
    public void AddReferanceToGridManager(BaseFruit fruit)
    {
        if(!baseFruits.Contains(fruit))
        {
            baseFruits.Add(fruit);
        }
    }
    public void EmptyGrid(int x, int y)
    {
        grid[x, y] = null;
    }
    public void GridRemoveFromMap(Vector2Int position)
    {
        FruitMap.Remove(position);
    }

    public int ReturnWidth()
    {
        return width;
    }

    public int ReturnHeight()
    {
        return height;
    }

    public BaseFruit GridGetValue(Vector2Int pos)
    {
        if (FruitMap.TryGetValue(pos, out var fruit))
        {
            return fruit;
        }

        return null;
    }

    public List<BaseFruit> GetAllConnectedFruits(BaseFruit startFruit)
    {
        List<BaseFruit> matched = new List<BaseFruit>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<BaseFruit> queue = new Queue<BaseFruit>();

        BaseFruit.FruitType type = startFruit._FruitType;
        Vector2Int startPos = new Vector2Int(startFruit.GridPosition.x,startFruit.GridPosition.y);

        queue.Enqueue(startFruit);
        visited.Add(startPos);

        while (queue.Count > 0)
        {
            BaseFruit current = queue.Dequeue();
            matched.Add(current);
            Vector2Int currentPos = new Vector2Int(current.GridPosition.x,current.GridPosition.y);

            foreach (var dir in directions) //defined top
            {
                Vector2Int neighborPos = currentPos + dir;

                if (visited.Contains(neighborPos))
                    continue;

                var nextFruit = GridGetValue(neighborPos);
                if (nextFruit)
                {
                    if (nextFruit._FruitType == type)
                    {
                        queue.Enqueue(nextFruit);
                        visited.Add(neighborPos);
                    }
                }
            }
        }
        if (matched.Count >= 3)
            return matched;
        else
            return new List<BaseFruit>();
    }

    #endregion
}
