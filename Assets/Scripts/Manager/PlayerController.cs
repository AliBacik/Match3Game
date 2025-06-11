using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IPlayerInteractStatus
{
    private IGridManagerGridController gridController;
    private IScoreAdder scoreAdder;
 
    [SerializeField] private bool isGridInteractable = false;
    public void Initialize(IGridManagerGridController grid,IScoreAdder score)
    {
        gridController = grid;
        scoreAdder = score;
    }
    private void OnEnable()
    {
        BaseFruit.OnAnyFruitClicked += OnFruitClicked;
    }

    private void OnDisable()
    {
        BaseFruit.OnAnyFruitClicked -= OnFruitClicked;
    }

    public void OnFruitClicked(BaseFruit fruit) 
    {
        if (isGridInteractable==false) return;
        fruit.Explode(gridController,this);
    }

    IEnumerator BehaviourCoroutines()
    {
        yield return gridController.CollapseFruits();
        yield return new WaitForSeconds(0.3f);
        yield return gridController.FillEmptyGrids();
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(gridController.CheckIfMatchPossible());
    }

    #region Interface Methods

    //Interface methods
    public void TriggerPostBehaviours()
    {
        StartCoroutine(BehaviourCoroutines());
    }
    public void TriggerScore(int count)
    {
        scoreAdder.AddScore(count);
    }
    public void PlayerStatusSwitch(bool active)
    {
        isGridInteractable = active;
    }

    #endregion
}
