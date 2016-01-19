using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для Emissions.xaml
    /// </summary>
    public partial class Emissions : Window
    {
        private ViewModels.Emissions _vM;
        public Emissions(Study currentStudy)
        {
            InitializeComponent();
            _vM = new ViewModels.Emissions(currentStudy);
            DataContext = _vM;
        }

        private void Emissions_OnClosing(object sender, CancelEventArgs e)
        {
            MainContext.SaveChanges();
        }
    }
}
