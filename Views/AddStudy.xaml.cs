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
using Microsoft.Win32;

namespace WASP_F_E.Views
{
    /// <summary>
    /// Логика взаимодействия для AddStudy.xaml
    /// </summary>
    public partial class AddStudy : Window
    {
        private readonly ViewModels.AddStudy _vMAddStudy = new ViewModels.AddStudy();
        
        public AddStudy()
        {
            InitializeComponent();
            DataContext = _vMAddStudy;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
