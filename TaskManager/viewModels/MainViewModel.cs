using Lab4.tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using TaskManager.models;
using TaskManager.tools;
using Timer = System.Timers.Timer;

namespace TaskManager
{
    class MainViewModel : PropertyChanger
    {
        private ProcessManager _manager;
        private Timer _timer;

        private List<ProcessModel> _rawProcess = new List<ProcessModel>();

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        private int _selectedSort = 0;
        public int SelectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                OnPropertyChanged("SortVisibility");

                UpdateSort();

            }
        }

        private bool _descending;
        public bool Descending
        {
            get => _descending;
            set
            {
                _descending = value;

                UpdateSort();
            }
        }

        public Visibility SortVisibility
        {
            get
            {
                if (SelectedSort == 0) return Visibility.Hidden;
                return Visibility.Visible;
            }
        }

        private object _selected;

        public object Selected
        {
            get => _selected;
            set => _selected = value;
        }

        public List<ProcessModel> Processes
        {
            get { return _rawProcess; }
            set
            {
                _rawProcess = value;
                OnPropertyChanged("Processes");
            }
        }


        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;

            }
        }


        private ICommand _showThreadsCommand;
        public ICommand ShowThreadsCommand
        {
            private get => _showThreadsCommand;
            set
            {
                _showThreadsCommand = value;
            }
        }

        private ICommand _showModulesCommand;
        public ICommand ShowModulesCommand
        {
            private get => _showModulesCommand;
            set
            {
                _showModulesCommand = value;
            }
        }

        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            private get => _openFolderCommand;
            set
            {
                _openFolderCommand = value;
            }
        }

        private ICommand _stopProcessCommand;
        public ICommand StopProcessCommand
        {
            private get => _stopProcessCommand;
            set
            {
                _stopProcessCommand = value;
            }
        }

        public MainViewModel()
        {
            _manager = new ProcessManager();

            Update();

            //update EVERYTHING each 2 seconds(metadata + process list)
            _timer = new Timer(2000);
            _timer.Elapsed += Task;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            ShowThreadsCommand = new RelayCommand(new Action<object>(ShowThreads));
            ShowModulesCommand = new RelayCommand(new Action<object>(ShowModules));
            OpenFolderCommand = new RelayCommand(new Action<object>(OpenFolder));
            StopProcessCommand = new RelayCommand(new Action<object>(StopProcess));
        }

        private async void ShowThreads(object obj)
        {
            await System.Threading.Tasks.Task.Run(() => ShowThreads());
        }

        private void ShowThreads()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                InfoWindow window = new InfoWindow(Selected as ProcessModel, true);
                window.ShowDialog();
            });
        }

        private async void ShowModules(object obj)
        {
            await System.Threading.Tasks.Task.Run(() => ShowModules());
        }

        private void ShowModules()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                InfoWindow window = new InfoWindow(Selected as ProcessModel, false);
                window.ShowDialog();
            });
        }

        private async void OpenFolder(object obj)
        {
            await System.Threading.Tasks.Task.Run(() => OpenFolder());
        }

        private void OpenFolder()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                string str = (Selected as ProcessModel).Path;
                if (String.IsNullOrEmpty(str))
                {
                    str = "C:\\Windows\\System32";
                }
                else
                {
                    str = str.Substring(0, str.LastIndexOf('\\'));
                }

                Process.Start(str);
            });
        }

        private async void StopProcess(object obj)
        {
            await System.Threading.Tasks.Task.Run(() => StopProcess());
        }

        private void StopProcess()
        {
            Process sel = Process.GetProcessById((Selected as ProcessModel).Id);
            sel.Kill();
        }

        //when you click sort
        private async void UpdateSort()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                int index = SelectedSort;
                Processes = SortModel.Sort(_manager.Processes, (SortType)index, !Descending);
                OnPropertyChanged("Processes");

            });
        }

        private async void Task(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {

                Update();

            });
        }

        //scheduled update
        private void Update()
        {

            _manager.UpdateProcesses();

            int index = SelectedSort;
            Processes = SortModel.Sort(_manager.Processes, (SortType)index, !Descending);


        }

    }
}
