using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using Microsoft.Win32;
using WASP_F_E.Models;
using Point = System.Windows.Point;

namespace WASP_F_E.ViewModels
{
    class CompareStudies:INotifyPropertyChanged
    {
        #region Fields
        
        private readonly List<Study> _studies;
        private int _minYear, _maxYear;
        private int _fromYear, _toYear;
        private string _excelFile;
        private List<TypesAdditionalVm> _dataForWrite;
        private string _selectedGroup;
        private string _selectedParameter;
        private ObservableCollection<PlantTypeWithChoiceVm> _types;
        private BackgroundWorker _backgroundWorker;
        private System.Windows.Window _view;
        #endregion
        
        #region Constructors

        public CompareStudies(List<Study> studies)
        {
            _studies = studies;
            _types = GetTypes();
            RaisePropertyChanged("Types");
            _minYear = _studies.Min(x => x.Scenarios.Min(obj => obj.Year));
            _maxYear = _studies.Max(x => x.Scenarios.Max(obj => obj.Year));
            _fromYear = _minYear;
            _toYear = _maxYear;
            AcceptClickCommand = new Command(arg => AcceptClickMethod());
            ChartClickCommand = new Command(arg => ChartClickMethod(arg as DataGrid));
            ExcelClickCommand = new Command(arg => ExcelClickMethod(arg as Window));
            ClearClickCommand = new Command(arg => ClearClickMethod());
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        //по завершенню запису в ексель зміна курсора
        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.Dispatcher.Invoke(new Action(() => _view.Cursor = Cursors.Arrow));
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Excel.WriteDataGridToExcel(_dataForWrite, _excelFile);
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

        #region Commands 

        public Command AcceptClickCommand { get; set; }
        public Command ChartClickCommand { get; set; }
        public Command ExcelClickCommand { get; set; }
        public Command ClearClickCommand { get; set; }

        private void ClearClickMethod()
        {
            foreach (var type in Types)
            {
                type.IsSelected = false;
            }
            FromYear = _minYear;
            ToYear = _maxYear;
        }

        private void ExcelClickMethod(System.Windows.Window view)
        {
            _view = view;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file | *.xlsx";
            sfd.FileOk += sfd_FileOk;
            sfd.ShowDialog();
            _view.Cursor = Cursors.Wait;
        }

        public void AcceptClickMethod()
        {
            RaisePropertyChanged("FiltredDataList");
        }

        public void ChartClickMethod(DataGrid dataGrid)
        {
            List<ChartData> chartDatas = new List<ChartData>();
            foreach (Study study in _studies)
            {
                if (SelectedGroup.Equals(Headers.ForTypes))
                {
                    foreach (var type in Types)
                    {
                        ChartData tmpData = new ChartData();
                        tmpData.Name = study.Name + " | " + type.Value.Name;
                        tmpData.Points = new List<Point>();
                        FiltredDataList.Where(obj => obj.StudyName == study.Name && obj.PlantType.Equals(type.Value.Name)).ToList().ForEach(x => tmpData.Points.Add(new Point(x.Year, x.Value)));
                        chartDatas.Add(tmpData);
                    }
                }
                if (SelectedGroup.Equals(Headers.ForYears))
                {
                    ChartData tmpData = new ChartData();
                    tmpData.Name = study.Name;
                    tmpData.Points = new List<Point>();
                    FiltredDataList.Where(obj => obj.StudyName == study.Name).ToList().ForEach(x => tmpData.Points.Add(new Point(x.Year, x.Value)));
                    chartDatas.Add(tmpData);
                }

            }
            Views.Chart view = new Views.Chart(chartDatas,SelectedParameter);
            view.ShowDialog();
        }
        #endregion

        #region Properties 

        public string VisibilityTypes
        {
            get
            {
                if (SelectedGroup.Equals(Headers.ForTypes))
                {
                    return "Visible";
                }
                return "Hidden";
            }
        }

        public List<string> GroupsList
        {
            get
            {
                SelectedGroup = Headers.ForYears;
                return new List<string>() { Headers.ForYears, Headers.ForTypes };
            }
        }

        public string SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                RaisePropertyChanged("FiltredDataList");
            }
        }

        public List<string> ParametersList
        {
            get
            {
                SelectedParameter = Headers.CapacityTotal;
                return new List<string>() { Headers.CapacityTotal, Headers.EnergyTotal, Headers.FuelTotal };
            }
        }

        public string SelectedParameter
        {
            get { return _selectedParameter; }
            set
            {
                _selectedParameter = value;
                RaisePropertyChanged("FiltredDataList");
            }
        }

        public List<TypesAdditionalVm> FiltredDataList
        {
            get
            {
                if (SelectedGroup.Equals(Headers.ForTypes))
                {
                    return GetFiltredDataListForTypes();
                }
                if (SelectedGroup.Equals(Headers.ForYears))
                {
                    return GetFiltredDataListForYears();
                }
                return new List<TypesAdditionalVm>();
            }
        }

        public int FromYear
        {
            get { return _fromYear; }
            set
            {
                if (value < _minYear)
                {
                    _fromYear = _minYear;
                }
                else if (value > _maxYear)
                {
                    _fromYear = _maxYear;
                }
                else
                {
                    _fromYear = value;
                }
            }
        }

        public int ToYear
        {
            get { return _toYear; }
            set
            {
                if (value < _minYear)
                {
                    _toYear = _minYear;
                }
                else if (value > _maxYear)
                {
                    _toYear = _maxYear;
                }
                else
                {
                    _toYear = value;
                }
            }
        }

        public ObservableCollection<PlantTypeWithChoiceVm> Types
        {
            get { return _types; }
        }

        #endregion

        #region Methods 
        void sfd_FileOk(object sender, CancelEventArgs e)
        {
            _excelFile = (sender as SaveFileDialog).FileName;
            _dataForWrite = FiltredDataList;
            Thread thread = new Thread(WriteDataToExcel);
            thread.Start();
        }

        private void WriteDataToExcel( )
        {
            _backgroundWorker.RunWorkerAsync();
            
        }

        private List<TypesAdditionalVm> GetFiltredDataListForYears()
        {
            List<TypesAdditionalVm> resList = new List<TypesAdditionalVm>();
            List<TypesAdditionalVm> tmpList = new List<TypesAdditionalVm>();
            Func<HPlant, double> hFunc;
            Func<TPlant, double> tFunc;
            MakeFuncs(out hFunc, out tFunc);

            foreach (Study study in _studies)
            {
                foreach (PlantTypeWithChoiceVm plantType in Types)
                {
                    if (plantType.IsSelected)
                    {
                        var query = from scenario in study.Scenarios
                            where scenario.Year >= FromYear && scenario.Year <= ToYear
                            select new TypesAdditionalVm()
                            {
                                StudyName = study.Name,
                                Year = scenario.Year,
                                Value =
                                    scenario.HPlants.Where(
                                        x => plantType.Value.Plants.Exists(obj => obj.ShortName == x.Name)).Sum(hFunc)
                                    +
                                    scenario.TPlants.Where(
                                        x => plantType.Value.Plants.Exists(obj => obj.ShortName == x.Name)).Sum(tFunc)
                            };
                        tmpList.AddRange(query);
                    }
                }
            }
            var query1 = from tmp1 in tmpList
                        group tmp1.Value by new {tmp1.StudyName, tmp1.Year } into val
                        select new
                        {
                            val.Key,
                            Value = val.Sum()
                        };
            
            foreach (Study study in _studies)
            {
                        var query = from scenario in study.Scenarios
                                    where scenario.Year >= FromYear && scenario.Year <= ToYear
                                    select new TypesAdditionalVm()
                                    {
                                        StudyName = study.Name,
                                        Year = scenario.Year,
                                        Value = query1.Where(x => x.Key.Year == scenario.Year && x.Key.StudyName.Equals(study.Name)).Sum(x => x.Value)
                                    };
                        resList.AddRange(query);
            }
            return resList.OrderBy(x => x.Year).ThenBy(x => x.StudyName).ToList();

        }

        private List<TypesAdditionalVm> GetFiltredDataListForTypes()
        {
            List<TypesAdditionalVm> resList = new List<TypesAdditionalVm>();
            Func<HPlant, double> hFunc;
            Func<TPlant, double> tFunc;
            MakeFuncs(out hFunc, out tFunc);

            foreach (Study study in _studies)
            {
                foreach (PlantTypeWithChoiceVm plantType in Types)
                {
                    if (plantType.IsSelected)
                    {
                        var query = (from scenario in study.Scenarios
                            where scenario.Year >= FromYear && scenario.Year <= ToYear 
                                     select new TypesAdditionalVm
                                     {
                                         StudyName = study.Name,
                                         PlantType = plantType.Value.Name,
                                         Year = scenario.Year,

                                         Value =
                                             scenario.TPlants.Where(
                                                 x => plantType.Value.Plants.Exists(obj => obj.ShortName == x.Name))
                                                 .Sum(tFunc)
                                             +
                                             scenario.HPlants.Where(
                                                 x => plantType.Value.Plants.Exists(obj => obj.ShortName == x.Name))
                                                 .Sum(hFunc)
                                     });
                        resList.AddRange(query);
                    }
                }
            }
            return resList.OrderBy(x => x.PlantType).ThenBy(x => x.Year).ThenBy(x => x.StudyName).ToList();
        }

        //build selection function
        private void MakeFuncs(out Func<HPlant, double> hFunc, out Func<TPlant, double> tFunc)
        {
            hFunc = plant => plant.EnergyTotal;
            tFunc = plant => plant.EnergyTotal;
            if (SelectedParameter.Equals(Headers.EnergyTotal))
            {
                hFunc = plant => plant.EnergyTotal;
                tFunc = plant => plant.EnergyTotal;
            }
            if (SelectedParameter.Equals(Headers.CapacityTotal))
            {
                hFunc = plant => plant.CapacityTotal;
                tFunc = plant => plant.CapacityTotal;
            }
            if (SelectedParameter.Equals(Headers.FuelTotal))
            {
                hFunc = plant => 0;
                tFunc = plant => plant.FuelTotal;
            }
            if (SelectedParameter.Equals(Headers.OaM))
            {
                hFunc = plant => plant.OaM;
                tFunc = plant => plant.OaM;
            }
        }

        private ObservableCollection<PlantTypeWithChoiceVm> GetTypes()
        {
            HashSet<PlantType> result = new HashSet<PlantType>();
            foreach (Study study in _studies)
            {
                foreach (var type in study.PlantTypes)
                {
                    result.Add(type);
                }
            }
            return (new ListChoiceVm(result.ToList())).Values;
        }

        #endregion
    }

}
