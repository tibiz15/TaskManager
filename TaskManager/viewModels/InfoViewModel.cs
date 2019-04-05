using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using TaskManager.models;
using TaskManager.tools;

namespace TaskManager
{
    class InfoViewModel : PropertyChanger
    {
        private bool _threads = true;
        private ProcessModel _process;


        public List<Object> Source { get; set; }

        public String InfoContent { get; private set; }

        public String ProcessInfo { get; private set; }

        private bool _zeroItems;
        public bool ZeroItems
        {
            get => _zeroItems;
            set
            {
                _zeroItems = value;
                OnPropertyChanged("NoItemsVisibility");
                OnPropertyChanged("ItemsVisibility");
            }
        }

        public Visibility NoItemsVisibility
        {
            get
            {
                if (ZeroItems) return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public Visibility ItemsVisibility
        {
            get
            {
                if (ZeroItems) return Visibility.Hidden;
                return Visibility.Visible;
            }
        }


        public InfoViewModel(ProcessModel process, bool threads)
        {
            InfoContent = "No threads in the Process";
            if (!threads)
            {
                InfoContent = "No modules in the process";
            }
            ProcessInfo = process.Name + " ID: " + process.Id;
            _threads = threads;
            _process = process;
            Source = new List<object>();
            UpdateThreads();

            Timer _timer = new Timer(2000);
            _timer.Elapsed += Task;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async void Task(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                UpdateThreads();

            });


        }

        private void UpdateThreads()
        {
            try
            {
                Source.Clear();
                OnPropertyChanged("Source");
                Process p = Process.GetProcessById(_process.Id);

                if (_threads)
                {
                    try
                    {
                        if (p.Threads.Count == 0)
                        {
                            ZeroItems = true;
                        }
                        else
                        {
                            int i = 0;
                            foreach (ProcessThread pThread in p.Threads)
                            {
                                DateTime d = ProcessManager.PcStarted;
                                try
                                {
                                    d = pThread.StartTime;
                                }
                                catch
                                {
                                }

                                Source.Add(new ThreadModel(pThread.Id, pThread.ThreadState, d));
                                i++;
                            }

                            ZeroItems = (i == 0);
                        }
                    }
                    catch
                    {
                        ZeroItems = true;
                    }
                }
                else
                {

                    try
                    {
                        if (p.Modules.Count == 0)
                        {
                            ZeroItems = true;
                        }
                        else
                        {

                            int i = 0;
                            foreach (ProcessModule pModule in p.Modules)
                            {
                                string name = "";
                                try
                                {
                                    name = pModule.FileName;
                                }
                                catch
                                {
                                }

                                string path = "";
                                try
                                {
                                    path = pModule.ModuleName;
                                }
                                catch
                                {
                                }

                                Source.Add(new ModuleModel(path, name));
                                i++;
                            }

                            ZeroItems = (i == 0);
                        }
                    }
                    catch
                    {
                        ZeroItems = true;
                    }
                }

            }
            catch
            {
                //for errors in the datagrid
            }

        }

    }
}
