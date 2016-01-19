using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WASP_F_E.Models;

namespace WASP_F_E.ViewModels
{
    public class StringWithChoiceVm : DependencyObject
    {
        public string Value
        {
            get { return (string) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof (string), typeof (StringWithChoiceVm));

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                "IsSelected", typeof (bool), typeof (StringWithChoiceVm));
    }

    public class PlantTypeWithChoiceVm : DependencyObject
    {
        public PlantType Value
        {
            get { return (PlantType)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(PlantType), typeof(PlantTypeWithChoiceVm));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(
                "IsSelected", typeof(bool), typeof(PlantTypeWithChoiceVm));
    }

    public class ListChoiceVm : DependencyObject
    {
        public ListChoiceVm(List<PlantType> initial)
        {
            Values = new ObservableCollection<PlantTypeWithChoiceVm>();
            foreach (var n in initial)
                Values.Add(new PlantTypeWithChoiceVm() { Value = n, IsSelected = true });
        }

        public ObservableCollection<PlantTypeWithChoiceVm> Values { get; private set; }

        public IEnumerable<PlantType> GetSelectedItems()
        {
            return Values.Where(v => v.IsSelected).Select(v => v.Value);
        }

        public IEnumerable<PlantType> GetUnselectedItems()
        {
            return Values.Where(v => !v.IsSelected).Select(v => v.Value);
        }
    }
}
