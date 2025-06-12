using UnityEngine;

public class BlueBerry : BaseFruit
{
    private void Awake()
    {
        _FruitType = FruitType.Blueberry;
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
