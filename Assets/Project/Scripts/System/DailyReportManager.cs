using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyReportManager : MonoBehaviour
{
    [Header("Daily Financial Data")]
    public float totalIncome = 800f;
    public float supplyCosts = 400f;
    public float rentCost = 120f;

    [Header("Pie Chart UI")]
    public Image supplyPieChart;
    public Image rentPieChart;
    public Image profitPieChart;

    [Header("Bar Chart Settings")]
    public Image[] weeklyBarCharts = new Image[7];
    public float[] weeklyProfitData = new float[7] { 400f, 800f, 400f, 500f, 320f, 500f, 450f };
    public float maxBarHeight = 300f;

    [Header("Animation Settings")]
    public float animDuration = 1f;

    // ==========================================
    // 1. CHẠY ANIMATION MƯỢT MÀ KHI PLAY GAME
    // ==========================================
    public void AnimateCharts()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateChartsSmoothly());
    }

    private IEnumerator UpdateChartsSmoothly()
    {
        float totalCosts = supplyCosts + rentCost;
        float profit = totalIncome - totalCosts;
        float displayProfit = Mathf.Max(0, profit);
        float totalPie = supplyCosts + rentCost + displayProfit;

        float targetSupply = totalPie > 0 ? (supplyCosts + rentCost + displayProfit) / totalPie : 0;
        float targetRent = totalPie > 0 ? (rentCost + displayProfit) / totalPie : 0;
        float targetProfit = totalPie > 0 ? displayProfit / totalPie : 0;

        float startSupplyPie = supplyPieChart != null ? supplyPieChart.fillAmount : 0;
        float startRentPie = rentPieChart != null ? rentPieChart.fillAmount : 0;
        float startProfitPie = profitPieChart != null ? profitPieChart.fillAmount : 0;

        float maxProfit = 0f;
        foreach (float p in weeklyProfitData)
        {
            if (p > maxProfit) maxProfit = p;
        }

        float[] startHeights = new float[weeklyBarCharts.Length];
        float[] targetHeights = new float[weeklyBarCharts.Length];

        for (int i = 0; i < weeklyBarCharts.Length; i++)
        {
            if (weeklyBarCharts[i] != null)
            {
                startHeights[i] = weeklyBarCharts[i].rectTransform.sizeDelta.y;
                targetHeights[i] = maxProfit > 0 ? (weeklyProfitData[i] / maxProfit) * maxBarHeight : 0;
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < animDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.SmoothStep(0, 1, elapsedTime / animDuration);

            if (supplyPieChart != null) supplyPieChart.fillAmount = Mathf.Lerp(startSupplyPie, targetSupply, t);
            if (rentPieChart != null) rentPieChart.fillAmount = Mathf.Lerp(startRentPie, targetRent, t);
            if (profitPieChart != null) profitPieChart.fillAmount = Mathf.Lerp(startProfitPie, targetProfit, t);

            for (int i = 0; i < weeklyBarCharts.Length; i++)
            {
                if (weeklyBarCharts[i] != null)
                {
                    RectTransform rt = weeklyBarCharts[i].rectTransform;
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, Mathf.Lerp(startHeights[i], targetHeights[i], t));
                }
            }

            yield return null;
        }
    }

    // ==========================================
    // 2. CẬP NHẬT TỨC THÌ KHI CHỈNH SỐ THỦ CÔNG (TRONG EDITOR)
    // ==========================================
#if UNITY_EDITOR
    private void OnValidate()
    {
        // Chỉ cập nhật tức thì nếu game đang DỪNG (không ở chế độ Play)
        if (!Application.isPlaying)
        {
            UpdateChartsInstantly();
        }
    }
#endif

    public void UpdateChartsInstantly()
    {
        // Cập nhật Pie Chart
        float totalCosts = supplyCosts + rentCost;
        float profit = Mathf.Max(0, totalIncome - totalCosts);
        float totalPie = supplyCosts + rentCost + profit;

        if (supplyPieChart != null) supplyPieChart.fillAmount = totalPie > 0 ? (supplyCosts + rentCost + profit) / totalPie : 0;
        if (rentPieChart != null) rentPieChart.fillAmount = totalPie > 0 ? (rentCost + profit) / totalPie : 0;
        if (profitPieChart != null) profitPieChart.fillAmount = totalPie > 0 ? profit / totalPie : 0;

        // Cập nhật Bar Chart
        float maxProfit = 0f;
        if (weeklyProfitData != null)
        {
            foreach (float p in weeklyProfitData) if (p > maxProfit) maxProfit = p;
        }

        if (weeklyBarCharts != null && weeklyProfitData != null)
        {
            for (int i = 0; i < weeklyBarCharts.Length; i++)
            {
                if (weeklyBarCharts[i] != null && i < weeklyProfitData.Length)
                {
                    RectTransform rt = weeklyBarCharts[i].rectTransform;
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, maxProfit > 0 ? (weeklyProfitData[i] / maxProfit) * maxBarHeight : 0);
                }
            }
        }
    }
}