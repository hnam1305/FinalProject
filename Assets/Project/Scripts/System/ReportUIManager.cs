using UnityEngine;
using UnityEngine.UI; 

public class ReportUIManager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject page1Stats;
    public GameObject page2Charts;

    [Header("")]
    public Button btnStats;
    public Button btnCharts;

    void Start()
    {
        ShowStatsPage();
    }

    public void ShowStatsPage()
    {
        page1Stats.SetActive(true);
        page2Charts.SetActive(false);

        btnStats.interactable = false; 
        btnCharts.interactable = true;
    }

    public void ShowChartsPage()
    {
        page1Stats.SetActive(false);
        page2Charts.SetActive(true);

        btnStats.interactable = true;
        btnCharts.interactable = false; 
    }
}