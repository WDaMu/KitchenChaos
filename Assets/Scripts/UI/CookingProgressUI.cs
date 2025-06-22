using System;
using UnityEngine;
using UnityEngine.UI;

public class CookingProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject cookingProgressUI;
    [SerializeField] private StoveCounter stoveCounter;

    private Color originalColor;
    private Color burnedColor = Color.red;
    private bool isCooking = false;
    private void Start()
    {
        originalColor = progressBarImage.color; // 记录原始颜色

        stoveCounter.onCookingStarted += stoveCounter_OnCookingStarted;
        stoveCounter.onCookingEnded += stoveCounter_OnCookingEnded;
        stoveCounter.onMeatFried += stoveCounter_OnMeatFried;
    }



    private void Update()
    {
        if (isCooking)
        {
            UpdateCookingProgressBar();
        }
    }

    private void stoveCounter_OnCookingStarted(object sender, EventArgs e)
    {
        isCooking = true;
        cookingProgressUI.SetActive(true); // 烧焦开始时显示进度条UI
    }

    private void stoveCounter_OnCookingEnded(object sender, EventArgs e)
    {
        isCooking = false;
        cookingProgressUI.SetActive(false); // 烧焦结束后隐藏进度条UI
        ResetProgressBar(); // 重置进度条
        
    }
    
    // 烧焦
    private void stoveCounter_OnMeatFried(object sender, EventArgs e)
    {
        progressBarImage.color = burnedColor; // 烹饪结束时改变进度条颜色
    }
    // 烹饪被打断


    private void UpdateCookingProgressBar()
    {
        stoveCounter.GetNormalizeTimer();

        progressBarImage.fillAmount = stoveCounter.GetNormalizeTimer(); // 更新进度条
    }
    private void ResetProgressBar()
    {
        progressBarImage.fillAmount = 0; // 重置进度条
        progressBarImage.color = originalColor; // 恢复原始颜色
    }


}
