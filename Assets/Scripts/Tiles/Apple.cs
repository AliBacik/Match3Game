using UnityEngine;

public class Apple : BaseFruit
{
    private void Awake()
    {
       _FruitType=FruitType.Apple;
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
