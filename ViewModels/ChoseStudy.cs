using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WASP_F_E.Models;
using Window = System.Windows.Window;

namespace WASP_F_E.ViewModels
{
    internal delegate void BackroundUpdater();
    class ChoseStudy : INotifyPropertyChanged
    {
        #region Fields

        private StudyContext _context;
        private ObservableCollection<Study> _studies;
        private BackroundUpdater _updater;
        private Study _selectedStudy;

        #endregion

        #region Constructors

        public ChoseStudy(bool forCompare)
        {
            _context = MainContext.GetInstance();
            Studies =  new ObservableCollection<Study>(_context.Studies.ToList());
            DeleteButtonClickCommand = new Command(arg => DeleteButtonClickMethod(arg as Window));
            if (forCompare) ShowStudyClickCommand = new Command(args => CompareStudyClickMethod(args));
            else ShowStudyClickCommand = new Command(args => ShowStudyClickMethod(args));
        }

        #endregion

        #region Commands

        public Command ShowStudyClickCommand { get; set; }
        public Command DeleteButtonClickCommand { get; set; }

        public void DeleteButtonClickMethod(Window arg)
        {
            if (MessageBox.Show(arg, Headers.DeleteConfirmation, Headers.PleaseComnfirmDeleting,
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Study tmp = SelectedStudy;
                _studies.Remove(SelectedStudy);
                Studies.Remove(SelectedStudy);
                _context.Studies.Remove(tmp);
                MainContext.SaveChanges();
            }
        }

        public void CompareStudyClickMethod(object arg)
        {
            Views.CompareStudies view = new Views.CompareStudies(SelectedStudies);
            view.Owner = arg as Window;
            view.Show();

        }

        public void ShowStudyClickMethod(object arg)
        {
            Views.ShowStudy view = new Views.ShowStudy(SelectedStudy);
            view.Owner = (arg as Window);
            view.Show();
        }


        #endregion

        #region Properties

        public ObservableCollection<Study> Studies
        {
            set
            {
                _studies = value;
                RaisePropertyChanged("Studies");
            }
            get { return _studies; }
        }
        public List<Study> SelectedStudies { get; set; }
        public string Years { get; set; }
        public string HydroconditionsCount { get; set; }
        public string PeriodsCount { get; set; }
        public Study SelectedStudy
        {
            get { return _selectedStudy; }
            set
            {
                _selectedStudy = value;
                RaisePropertyChanged("SelectedStudy");
                _updater = UpdateProperties;
                _updater.BeginInvoke(UpdaterCallback, null);
            }
        }

        #endregion

        #region Methods

        public void UpdateProperties()
        {
            if (SelectedStudy != null && SelectedStudy.Scenarios != null && SelectedStudy.Scenarios.Count > 0)
            {
                Years = SelectedStudy.Scenarios.Min(x => x.Year) + " - " + SelectedStudy.Scenarios.Max(x => x.Year);
                HydroconditionsCount = SelectedStudy.Scenarios[0].Hydrocondition.ToString();
                PeriodsCount = SelectedStudy.Scenarios.Max(x => x.Period).ToString();
            }
            else
            {
                Years = String.Empty;
                HydroconditionsCount = String.Empty;
                PeriodsCount = String.Empty;
            }
        }

        private void UpdaterCallback(IAsyncResult res)
        {
            RaisePropertyChanged("Years");
            RaisePropertyChanged("HydroconditionsCount");
            RaisePropertyChanged("PeriodsCount");
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
