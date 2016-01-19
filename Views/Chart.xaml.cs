using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WASP_F_E.Models;

namespace WASP_F_E.Views
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : Window
    {
        public Chart(List<ChartData> chartDatas, string valueName)
        {
            InitializeComponent();
            LinearAxis xAxis = new LinearAxis();
            xAxis.Title = Headers.Year;
            xAxis.Interval = 1;
            xAxis.Orientation = AxisOrientation.X;
            LinearAxis yAxis = new LinearAxis();
            yAxis.Title = valueName;
            yAxis.Orientation = AxisOrientation.Y;
            Charts.Axes.Add(xAxis);
            Charts.Axes.Add(yAxis);
            foreach (var data in chartDatas)
            {
                var newChart = new ColumnSeries();
                newChart.DependentValuePath = "Y";
                newChart.IndependentValuePath = "X";
                newChart.Title = data.Name;
                newChart.ItemsSource = data.Points;
                newChart.Refresh();
                Charts.Series.Add(newChart);
            }
        }
    }
}
