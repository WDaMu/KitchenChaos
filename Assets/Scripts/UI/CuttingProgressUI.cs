using System;
using UnityEngine;
using UnityEngine.UI;

public class CuttingProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject cuttingProgressUI;
    [SerializeField] private CuttingCounter cuttingCounter;

    private void Awake()
    {
        Hide();
    }
    private void Start()
    {
        cuttingCounter.OnCuttingStarted += cuttingCounter_OnCuttingStarted;
        cuttingCounter.OnCuttingEnded += cuttingCounter_OnCuttingEnded;
    }

    private void cuttingCounter_OnCuttingEnded(object sender, EventArgs e)
    {
        Hide();
        UpdateProgressBar(0f); // 重置进度条
    }

    private void cuttingCounter_OnCuttingStarted(object sender, CuttingCounter.OnCuttingStartedEventArgs e)
    {
        Show();
        UpdateProgressBar(e.progressNormalized);
    }

    private void UpdateProgressBar(float progress)
    {
        progressBarImage.fillAmount = progress;
    }

    private void Show()
    {
        cuttingProgressUI.SetActive(true);
    }

    private void Hide()
    {
        cuttingProgressUI.SetActive(false);
    }
}

