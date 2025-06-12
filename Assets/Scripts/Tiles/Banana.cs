using UnityEngine;

public class Banana : BaseFruit
{
    private void Awake()
    {
        _FruitType = FruitType.Banana;
        SetExplosionStrategy(new ColorExplosionStrategy());
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
