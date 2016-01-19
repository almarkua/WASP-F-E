using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using WASP_F_E.Models;

namespace WASP_F_E.ViewModels
{
    internal enum AddingErrors
    {
        None,Mersim,Fixsys,Varsys,Database
    }

    class AddStudy:INotifyPropertyChanged
    {
        #region Fields 
        
        private string _pathToMersimFile, _pathToFixsysFile, _pathToVarsysFile, _name;
        private DateTime _date = DateTime.Now;
        private readonly StudyContext _context = new StudyContext();
        private OpenFileDialog _mersimFileDialog, _fixsysFileDialog, _varsysFileDialog;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        private Views.AddStudy _currentView;

        #endregion

        #region Constructors 

        public AddStudy()
        {
            AddStudyClickCommand = new Command(args => AddStudyClickMethod(args));
            MersimClickCommand = new Command(args => MersimClickMethod(args));
            FixsysClickCommand = new Command(args => FixsysClickMethod(args));
            VarsysClickCommand = new Command(args => VarsysClickMethod(args));
            IsPlantsFromFile = true;
        }

        #endregion

        #region INotifyPropertyChanged Implementations

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

        #region Properties

        public string PathToMersimFile
        {
            get { return _pathToMersimFile; }
            set
            {
                if (value != null && File.Exists(value.Trim()))
                {
                    _pathToMersimFile = value;
                    RaisePropertyChanged("PathToMersimFile");
                } 
            }
        }

        public string PathToFixsysFile
        {
            get { return _pathToFixsysFile; }
            set
            {
                if (value != null && File.Exists(value))
                {
                    _pathToFixsysFile = value;
                    RaisePropertyChanged("PathToFixsysFile");
                }
            }
        }

        public string PathToVarsysFile
        {
            get { return _pathToVarsysFile; }
            set
            {
                if (value != null && File.Exists(value))
                {
                    _pathToVarsysFile = value;
                    RaisePropertyChanged("PathToVarsysFile");
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public bool IsPlantsFromFile { get; set; }

        public bool IsPlantsFromStudy { get; set; }

        public DateTime Date {
            get { return _date; }
            set
            {
                _date = value; 
                RaisePropertyChanged("Date");
            }
        }

        public ObservableCollection<Study> StudiesList
        {
            get
            {
                var query = from study in _context.Studies
                            select study;
                return new ObservableCollection<Study>(query.ToList());
            }
        }

        public ICommand AddStudyClickCommand { get; set; }

        public ICommand MersimClickCommand { get; set; }

        public ICommand FixsysClickCommand { get; set; }

        public ICommand VarsysClickCommand { get; set; }

        public Study SelectedStudy { get; set; }

        #endregion

        #region Methods 

        private void AddStudyClickMethod(object view)
        {
            _currentView = view as Views.AddStudy;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            _currentView.Cursor = Cursors.Wait;
        }
        
        //обробка помилок додавання нового дослідження
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _currentView.Cursor = Cursors.Arrow;
            switch ((AddingErrors)e.Result)
            {
                case AddingErrors.None:
                {
                    MessageBox.Show(_currentView,"Дослідження успішно додане", "Вітаємо",
                       MessageBoxButton.OK, MessageBoxImage.Information);
                    _currentView.Close();
                    break;
                }
                case AddingErrors.Fixsys:
                {
                    MessageBox.Show("Зчитування fixsys файлу завершилось невдало. Перевірте будьласка файл.","Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                  
                }
                case AddingErrors.Varsys:
                {
                    MessageBox.Show("Зчитування varsys файлу завершилось невдало. Перевірте будьласка файл.", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                case AddingErrors.Mersim:
                {
                    MessageBox.Show("Зчитування mersim файлу завершилось невдало. Перевірте будьласка файл.", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                case AddingErrors.Database:
                {
                    MessageBox.Show("Помилка при роботі з базою данних.", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = CreateNewStudy();
        }
        
        public AddingErrors CreateNewStudy()
        {

            Study newStudy = new Study();
            newStudy.Name = Name;
            newStudy.Date = Date;
            newStudy.PlantTypes = new List<PlantType>();
            Mersim mersim = new Mersim(PathToMersimFile);
            if (IsPlantsFromFile)
            {
                Varsys varsys = new Varsys(PathToVarsysFile);
                Fixsys fixsys = new Fixsys(PathToFixsysFile);
                PlantType type = new PlantType();
                type.Name = "Undefined type";
                type.Plants = new List<Plant>();
                try
                {
                    type.Plants.AddRange(fixsys.Parse());
                }
                catch (Exception)
                {
                    return AddingErrors.Fixsys;
                }
                try
                {
                    type.Plants.AddRange(varsys.Parse());
                    newStudy.PlantTypes.Add(type);
                }
                catch (Exception)
                {
                    return AddingErrors.Varsys;
                }
            }
            if (IsPlantsFromStudy)
            {
                newStudy.PlantTypes = SelectedStudy.PlantTypes.ToList();
            }
            try
            {
                newStudy.Scenarios = mersim.Parse();
            }
            catch (Exception)
            {
                return AddingErrors.Mersim;
            }
            
            try
            {
                _context.Studies.Add(newStudy);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return AddingErrors.Database;
            }
            return AddingErrors.None;
        }

        public void MersimClickMethod(object arg)
        {
            _mersimFileDialog = new OpenFileDialog();
            _mersimFileDialog.Multiselect = false;
            _mersimFileDialog.Filter = "Файл звіту|*.rep|Текстовий документ|*.txt";
            _mersimFileDialog.FileOk += _mersimFileDialog_FileOk;
            _mersimFileDialog.ShowDialog();

        }

        void _mersimFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            PathToMersimFile = _mersimFileDialog.FileName;
        }

        public void FixsysClickMethod(object arg)
        {
            _fixsysFileDialog = new OpenFileDialog();
            _fixsysFileDialog.Multiselect = false;
            _fixsysFileDialog.Filter = "Файл звіту|*.rep|Текстовий документ|*.txt";
            _fixsysFileDialog.FileOk += _fixsysFileDialog_FileOk;
            _fixsysFileDialog.ShowDialog();
        }

        void _fixsysFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            PathToFixsysFile = _fixsysFileDialog.FileName;
        }


        public void VarsysClickMethod(object arg)
        {
            _varsysFileDialog = new OpenFileDialog();
            _varsysFileDialog.Multiselect = false;
            _varsysFileDialog.Filter = "Файл звіту|*.rep|Текстовий документ|*.txt";
            _varsysFileDialog.FileOk += _varsysFileDialog_FileOk;
            _varsysFileDialog.ShowDialog();
        }

        void _varsysFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            PathToVarsysFile = _varsysFileDialog.FileName;
        }

        #endregion

    }
}
