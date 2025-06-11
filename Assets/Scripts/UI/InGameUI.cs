using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Slider ScoreBar;
    [SerializeField] float maxBarValue = 5000;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] private GameObject Header;
    [SerializeField] private GameObject Bat;

    private void OnDestroy()
    {
        GridManager.LevelStart -= SetUp;
        ScoreTracker.ScoreChanged -= UpdateScoreBar;
    }
    void Start()
    {
        GridManager.LevelStart += SetUp;
        ScoreTracker.ScoreChanged += UpdateScoreBar;
    }

    public void UpdateScoreBar(int currentScore)
    {
        Score.text = currentScore.ToString();
        float normalizedValue = Mathf.Clamp01((float)currentScore / maxBarValue);
        StopAllCoroutines();
        StartCoroutine(AnimateSlider(ScoreBar.value, normalizedValue, 0.5f)); // 0.5 saniyede animasyon
    }
    void SetUp()
    {
        ScoreBar.value = 0;
        Bat.transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Bat.SetActive(false);
            Header.SetActive(true);
            Header.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        });

    }

    IEnumerator AnimateSlider(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            ScoreBar.value = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ScoreBar.value = to;
    }
}
