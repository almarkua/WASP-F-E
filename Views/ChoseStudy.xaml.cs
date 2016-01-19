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
using Window = System.Windows.Window;

namespace WASP_F_E.Views
{
    /// <summary>
    /// Логика взаимодействия для ChoseStudy.xaml
    /// </summary>
    public partial class ChoseStudy : Window
    {
        private readonly ViewModels.ChoseStudy _vMChoseStudy;
        public ChoseStudy(bool forCompare)
        {
            InitializeComponent();
            _vMChoseStudy = new ViewModels.ChoseStudy(forCompare);
            DataContext = _vMChoseStudy;
            if (forCompare)
            {
                ButtonShowStudy.Content = Headers.Compare;
                ButtonDeleteStudy.Visibility = Visibility.Hidden;
            }
        }

        private void ListViewStudy_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Study> result = new List<Study>();
            foreach (var obj in (sender as ListView).SelectedItems)
            {
                result.Add(obj as Study);
            }
            _vMChoseStudy.SelectedStudies = result;
        }

        private void ChoseStudy_OnClosing(object sender, CancelEventArgs e)
        {
            MainContext.SaveChanges();
        }
    }
}
