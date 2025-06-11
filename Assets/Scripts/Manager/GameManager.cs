using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ScoreTracker scoreTracker;

    private void Awake()
    {
        poolManager.Initialize(gridManager);
        gridManager.Initialize(playerController,poolManager);
        playerController.Initialize(gridManager, scoreTracker);
    }
}
