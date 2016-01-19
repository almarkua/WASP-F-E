using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WASP_F_E.Models;

namespace WASP_F_E.ViewModels
{
    class Plants:INotifyPropertyChanged
    {
        #region Fields

        private Study _currentStudy;
        private ObservableCollection<Plant> _plants;
        private Plant _selectedPlant;

        #endregion

        #region Constructors

        public Plants(Study currentStudy)
        {
            _currentStudy = currentStudy;
            PlantsList = GetPlants();
            if (PlantsList.Count != 0) SelectedPlant = PlantsList[0];
        }

        #endregion

        #region Properties

        public ObservableCollection<Plant> PlantsList
        {
            get { return _plants; }
            set
            {
                _plants = value;
                RaisePropertyChanged("Plants");
            }
        }

        public List<PlantType> Types {
            get { return _currentStudy.PlantTypes; }
        }

        
        public Plant SelectedPlant {
            get { return _selectedPlant; }
            set { _selectedPlant = value; }
        }

        #endregion

        #region Methods 

        public ObservableCollection<Plant> GetPlants()
        {
            ObservableCollection<Plant> plants = new ObservableCollection<Plant>();
            foreach (var type in _currentStudy.PlantTypes)
            {
                if (type.Plants != null)
                {
                    type.Plants.ForEach(x => plants.Add(x));
                }
            }
            return plants;
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
