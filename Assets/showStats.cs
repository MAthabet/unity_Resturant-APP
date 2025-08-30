using UnityEngine;
using TMPro;
using NUnit.Framework;
using XCharts.Runtime;

public class showStats : MonoBehaviour
{
    [SerializeField]
    private BarChart BarChart;
    [SerializeField]
    private PieChart PieChart;
    public void OnStatsClicked()
    {
        var data = DatabaseManager.Singleton.GetOrdersStats();
        BarChart.ClearData();
        PieChart.ClearData();

        var title1 = PieChart.EnsureChartComponent<Title>();
        title1.text = "Most Popular Meals";
        var title = BarChart.EnsureChartComponent<Title>();
        title.text = "Top Customers by Items Ordered";

        foreach (var o in data)
        {
            PieChart.AddData(0,o.count, o.MealName);
        }
        var stats = DatabaseManager.Singleton.GetUserOrderStatistics();
        foreach (var o in stats)
        {
            BarChart.AddYAxisData(o.FirstName);
            BarChart.AddData(0, o.TotalItemsOrdered, o.FirstName);
            Debug.Log(o.FirstName);
        }
    }
}
