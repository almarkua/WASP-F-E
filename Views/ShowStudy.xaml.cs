using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Логика взаимодействия для ShowStudy.xaml
    /// </summary>
    public partial class ShowStudy : Window
    {
        private readonly ViewModels.ShowStudy _vMShowStudy;
        public ShowStudy(Study currentStudy)
        {
            InitializeComponent();
            Title = currentStudy.Name + " | " + currentStudy.Date.ToShortDateString();
            _vMShowStudy = new ViewModels.ShowStudy(currentStudy);
            DataContext = _vMShowStudy;
        }

        private void ExpanderTotal_OnExpanded(object sender, RoutedEventArgs e)
        {
            DataTableThermal.Margin = new Thickness(10, 172, 10, 185);
        }

        private void ExpanderTotal_OnCollapsed(object sender, RoutedEventArgs e)
        {
            DataTableThermal.Margin = new Thickness(10, 172, 10, 25);
        }
    }
}
