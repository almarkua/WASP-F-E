using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для Plants.xaml
    /// </summary>
    public partial class Plants : Window
    {
        private Study _study;
        private ViewModels.Plants _vM;
        public Plants(Study currentStudy)
        {
            InitializeComponent();
            _study = currentStudy;
            _vM = new ViewModels.Plants(_study);
            DataContext = _vM;
        }

        private void Plants_OnClosing(object sender, CancelEventArgs e)
        {
            MainContext.SaveChanges();
        }
    }
}
