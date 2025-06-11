using DG.Tweening;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private void Start()
    {
        GridManager.LevelStart += Hide;
    }

    private void OnDestroy()
    {
        GridManager.LevelStart -= Hide;
    }
    void Hide()
    {
        transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
