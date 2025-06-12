using UnityEngine;

public class Bomb : BaseFruit
{
    private void Awake()
    {
        _FruitType = FruitType.Bomb;
        SetExplosionStrategy(new CrossExplosionStrategy());
    }
    protected override void OnHoverEnter()
    {
        transform.localScale *= 1.2f;
    }

    protected override void OnHoverExit()
    {
        transform.localScale = DefaultScale;
    }
    public override void OnExplode()
    {
        ExplodeVFX.Play();
    }
}
