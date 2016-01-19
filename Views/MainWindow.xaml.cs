using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WASP_F_E.Models;

namespace WASP_F_E.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Database.SetInitializer<StudyContext>(new StudyInitializer());
            Console.WriteLine(MainContext.GetInstance().Studies.Count());
        }

        private void AddStudyItem_OnClick(object sender, RoutedEventArgs e)
        {
            AddStudy view=new AddStudy();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowItems_OnClick(object sender, RoutedEventArgs e)
        {
            ChoseStudy view = new ChoseStudy(false);
            view.Owner = this;
            view.ShowDialog();
        }

        private void CompareItem_OnClick(object sender, RoutedEventArgs e)
        {
            ChoseStudy view = new ChoseStudy(true);
            view.Owner = this;
            view.ShowDialog();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
