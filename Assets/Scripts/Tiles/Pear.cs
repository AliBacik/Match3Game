using UnityEngine;

public class Pear : BaseFruit
{
    private void Awake()
    {
        _FruitType = FruitType.Pear;
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
}
