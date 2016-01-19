using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Win32;
using WASP_F_E.Models;
using WASP_F_E.Views;
using Scenario = WASP_F_E.Models.Scenario;
using Window = System.Windows.Window;

namespace WASP_F_E.ViewModels
{
    public class ShowStudy:INotifyPropertyChanged
    {
        #region Fields

        private Study _currentStudy;
        private int _currentYear;
        private int _currentPeriod;
        private int _currentHydroconditions;
        private string pathToExcelFile;
        private BackgroundWorker _backgroundWorker;
        private Window _view;

        #endregion
        
        #region Constructors 

        public ShowStudy(Models.Study currentStudy)
        {
            _currentStudy = currentStudy;
            ExcelClickCommand = new Command(arg => ExcelClickMethod(arg as Window));
            PlantsClickCommand = new Command(arg => PlantsClickMethod(arg));
            TypesClickCommand = new Command(arg => TypesClickMethod(arg));
            EmissionsClickCommand = new Command(arg => EmissionsClickMethod());
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Excel.WriteStudyToExcel(_currentStudy,pathToExcelFile);
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.Dispatcher.Invoke(() => _view.Cursor = Cursors.Arrow);
        }

        #endregion

        #region Commands 

        public Command ExcelClickCommand { get; set; }
        public Command PlantsClickCommand { get; set; }
        public Command TypesClickCommand { get; set; }
        public Command EmissionsClickCommand { get; set; }

        public void EmissionsClickMethod()
        {
            Views.Emissions view = new Views.Emissions(_currentStudy);
            view.ShowDialog();
        }

        public void TypesClickMethod(object obj)
        {
            Types view = new Types(_currentStudy);
            view.Owner = obj as Window;
            view.ShowDialog();
        }

        public void PlantsClickMethod(object obj)
        {
            Views.Plants view = new Views.Plants(_currentStudy);
            view.Owner = obj as Window;
            view.ShowDialog();
        }

        private void ExcelClickMethod(Window view)
        {
            _view = view;
            
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file|*.xlsx";
            sfd.FileOk += sfd_FileOk;
            if ((bool)sfd.ShowDialog()) {
                _view.Cursor = Cursors.Wait;
            }
        }

        #endregion

        #region Properties

        public HashSet<int> Years {
            get
            {   HashSet<int> resultList = new HashSet<int>();
                _currentStudy.Scenarios.ForEach( obj => resultList.Add(obj.Year));
                CurrentYear = resultList.First();
                CurrentPeriod = Periods.First();
                CurrentHydroconditions = Hydroconditions.First();
                return resultList;
            }
        }

        public int CurrentYear {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                RaisePropertyChanged("CurrentYear");
                RaisePropertyChanged("Periods");
                RaisePropertyChanged("Hydroconditions");
                RaisePropertyChanged("HPlants");
                RaisePropertyChanged("TPlants");
                RaisePropertyChanged("CurrentScenario");
            }
        }

        public List<int> Periods
        {
            get
            {
                List<int> resultList = new List<int>();
                foreach (var scenario in _currentStudy.Scenarios)
                {
                    if (scenario.Year == CurrentYear)
                    {
                        resultList.Add(scenario.Period);
                    }
                }
                
                return resultList;
            }
        }

        public int CurrentPeriod {
            get { return _currentPeriod; }
            set
            {
                _currentPeriod = value;
                RaisePropertyChanged("CurrentPeriod");
                RaisePropertyChanged("Hydroconditions");
                RaisePropertyChanged("CurrentScenario");
                RaisePropertyChanged("HPlants");
                RaisePropertyChanged("TPlants");
            }
        }

        public List<int> Hydroconditions
        {
            get
            {
                List<int> resultList = new List<int>();
                foreach (var scenario in _currentStudy.Scenarios)
                {
                    if (scenario.Year == CurrentYear && scenario.Period == CurrentPeriod)
                    {
                        resultList.Add(scenario.Hydrocondition);
                    }
                }
                return resultList;
            }
        }

        public int CurrentHydroconditions {
            get { return _currentHydroconditions; }
            set
            {
                _currentHydroconditions = value;
                RaisePropertyChanged("HPlants");
                RaisePropertyChanged("TPlants");
                RaisePropertyChanged("CurrentScenario");
            }
        }

        public List<HPlant> HPlants
        {
            get
            {
                if (_currentStudy.Scenarios.Find(st =>
                    (st.Year == CurrentYear) && (st.Period == CurrentPeriod) &&
                    (st.Hydrocondition == CurrentHydroconditions)) != null) {
                    return _currentStudy.Scenarios.Find(st =>
                    (st.Year == CurrentYear) && (st.Period == CurrentPeriod) &&
                    (st.Hydrocondition == CurrentHydroconditions)).HPlants;
                }
                return new List<HPlant>();
            }
        }

        public List<TPlant> TPlants
        {
            get
            {
                if (_currentStudy.Scenarios.Find(st =>
                    (st.Year == CurrentYear) && (st.Period == CurrentPeriod) &&
                    (st.Hydrocondition == CurrentHydroconditions)) != null)
                {
                    return _currentStudy.Scenarios.Find(st =>
                    (st.Year == CurrentYear) && (st.Period == CurrentPeriod) &&
                    (st.Hydrocondition == CurrentHydroconditions)).TPlants;
                }
                return new List<TPlant>();
            }
        }

        public Scenario CurrentScenario
        {
            get
            {
                return _currentStudy.Scenarios.Find(st =>
                    (st.Year == CurrentYear) && (st.Period == CurrentPeriod) &&
                    (st.Hydrocondition == CurrentHydroconditions));
            }
        }

        #endregion

        #region Methods

        void sfd_FileOk(object sender, CancelEventArgs e)
        {
            pathToExcelFile = ((SaveFileDialog)sender).FileName;
            _backgroundWorker.RunWorkerAsync();
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
