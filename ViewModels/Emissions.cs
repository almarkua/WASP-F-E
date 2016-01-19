using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using WASP_F_E.Models;
using System.Windows.Input;

namespace WASP_F_E.ViewModels
{
    class Emissions: INotifyPropertyChanged
    {
        #region Fields

        private Study _currentStudy;
        private Plant _selectedPlant;
        private PlantType _selectedType;
        private List<PlantType> _types;
        private List<Plant> _plants;
        private ObservableCollection<EmissionValue> _emissionsValues;
        private List<EmissionType> _emissionsTypes;
        private string _excelFile;
        private Window _view;

        #endregion

        #region Constructors

        public Emissions(Study currentStudy)
        {
            _currentStudy = currentStudy;
            Types = _currentStudy.PlantTypes;
            EmissionsTypes = _currentStudy.Emissions;
            CalculateClickCommand = new Command(arg => CalculateClickMethod(arg as Window));
        }

        #endregion

        #region Properties

        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set
            {
                _selectedPlant = value;
                RaisePropertyChanged("SelectedPlant");
            }
        }

        public ObservableCollection<EmissionValue> EmissionsValues { get; set; }

        public EmissionCalculationType CalculationType { get; set; }

        public PlantType SelectedType {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                Plants = _selectedType.Plants;
                RaisePropertyChanged("SelectedType");
            }
        }

        public List<PlantType> Types
        {
            get { return _types; }
            set
            {
                _types = value;
                
                SelectedType = _types[0];
                RaisePropertyChanged("Types");
            }
        }

        public List<Plant> Plants
        {
            get { return _plants; }
            set
            {
                _plants = value;
                SelectedPlant = _plants.FirstOrDefault();
                RaisePropertyChanged("Plants");
            }
        }

        public List<EmissionType> EmissionsTypes {
            get { return _emissionsTypes; }
            set
            {
                _emissionsTypes = value;
                RaisePropertyChanged("EmissionsTypes");
            }
        }

        public List<EmissionCalculationType> EmissionsCalculationTypes{
            get { return new List<EmissionCalculationType>(Enum.GetValues(typeof(EmissionCalculationType)).OfType<EmissionCalculationType>()); }
        }

        #endregion

        #region Commands 

        public Command CalculateClickCommand { get; set; }

        public void CalculateClickMethod(Window view)
        {
            _view = view;
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Excel file | *.xlsx";
            sf.FileOk += sf_FileOk;
            if ((bool)sf.ShowDialog()) {
                _view.Cursor = Cursors.Wait;
            }
        }

        void sf_FileOk(object sender, CancelEventArgs e)
        {
            _excelFile = (sender as SaveFileDialog).FileName;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            bg.RunWorkerAsync();
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _view.Dispatcher.Invoke(() => _view.Cursor = Cursors.Arrow);
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            CalculateEmission();
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

        #region Methods

        //метод обчислення дав Богдан, є на електронці
        private void CalculateEmission()
        {
            var dict = new Dictionary<EmissionType, Dictionary<int, double>>();
            foreach (var scenario in _currentStudy.Scenarios)
            {
                foreach (var emission in _currentStudy.Emissions)
                {
                    if (!dict.ContainsKey(emission)) dict[emission] = new Dictionary<int, double>();
                    foreach (var eValue in emission.EmissionsValues)
                    {
                        foreach (var plant in scenario.TPlants)
                        {
                            if (eValue.Plant.ShortName.Equals(plant.Name))
                            {
                                if (!dict[emission].ContainsKey(scenario.Year)) dict[emission][scenario.Year] = 0;
                                if (eValue.CalculationType == EmissionCalculationType.FromEnergy)
                                {
                                    dict[emission][scenario.Year] += eValue.Value / 100 *plant.EnergyTotal;
                                }
                                if (eValue.CalculationType == EmissionCalculationType.FromFuel)
                                {
                                    dict[emission][scenario.Year] += (eValue.Value / 100 * plant.EnergyTotal / 1000000 * eValue.Plant.AverageEfficiency)/eValue.Plant.HeatValue;
                                }
                            }
                        }
                    }
                }
            }
            Excel.WriteEmissionsToExcel(_currentStudy,dict, _excelFile);
        }

        #endregion
    }
}
