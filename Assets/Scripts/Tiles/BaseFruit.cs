using System;
using UnityEngine;

public class BaseFruit : MonoBehaviour
{
    private IExplosionStrategy _explosionStrategy;
    public enum FruitType { Apple, Banana, Orange, Blueberry, Pear, Bomb }
    public FruitType _FruitType;
    public ParticleSystem ExplodeVFX;
    public Vector3 DefaultScale;
    public Vector2Int GridPosition {  get; set; }

    protected bool isHovered = false;

    public static event Action<BaseFruit> OnAnyFruitClicked;


    public void SetExplosionStrategy(IExplosionStrategy strategy)
    {
        _explosionStrategy = strategy;
    }
    public void Explode(IGridManagerGridController gridManagerGridController,IPlayerInteractStatus playerStatus)
    {
        _explosionStrategy?.TileExplode(GridPosition,gridManagerGridController,playerStatus);
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        OnHoverEnter();
    }

    private void OnMouseExit()
    {
        isHovered = false;
        OnHoverExit();
    }

    private void OnMouseDown()
    {
        OnAnyFruitClicked?.Invoke(this);
    }

    protected virtual void OnHoverEnter()
    {
        transform.localScale *=  1.2f;
    }

    protected virtual void OnHoverExit()
    {
        transform.localScale = DefaultScale;
    }

    public virtual void OnExplode()
    {
        ExplodeVFX.Play();
    }

    private void OnDisable()
    {
        transform.localScale = DefaultScale;
    }

}
