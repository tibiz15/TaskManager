using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace TaskManager.models
{
    class ProcessManager
    {
        private Dictionary<ProcessModel, bool> ProcessesDictionary { get; }

        public List<ProcessModel> Processes
        {
            get => ProcessesDictionary.Keys.ToList();

        }


        public static long TotalRam;

        public static DateTime PcStarted;

        public ProcessManager()
        {


            ManagementObjectCollection winInfo = new ManagementObjectSearcher("Select * From Win32_OperatingSystem").Get();

            foreach (ManagementObject obj in winInfo)
            {
                TotalRam = long.Parse(obj["TotalVisibleMemorySize"].ToString()) * 1024;
                PcStarted = GetStartTime(obj["LastBootUpTime"].ToString());
            }

            ProcessesDictionary = new Dictionary<ProcessModel, bool>();
            UpdateProcesses();
        }

        //updats EVERYTHING
        //if process is not in list, add it with fresh values
        //if process is in list, update it's values
        public void UpdateProcesses()
        {
            ManagementObjectCollection processList = new ManagementObjectSearcher("Select * From Win32_Process").Get();
            //MessageBox.Show(processList.Count.ToString());
            foreach (ManagementObject obj in processList)
            {
                try
                {
                    int procId = int.Parse(obj["ProcessId"].ToString());
                    ProcessModel oldModel = Processes.Find(prcss => prcss.Id == procId);

                    if (oldModel == null) //process does not exist in the list
                    {

                        try
                        {
                            Process tempProcess = Process.GetProcessById(procId);

                            DateTime d = ProcessManager.PcStarted;
                            //DateTime d = getStartTime(obj["CreationDate"].ToString());
                            String fileName = "";
                            String moduleName = "";
                            String user = "SYSTEM";

                            TimeSpan cpu = TimeSpan.Zero;

                            try
                            {
                                d = tempProcess.StartTime;
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                fileName = obj["Caption"].ToString();
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                moduleName = obj["ExecutablePath"].ToString();
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                cpu = tempProcess.TotalProcessorTime;
                            }
                            catch (Exception)
                            {
                            }

                            try
                            {
                                string[] argList = new string[] { string.Empty, string.Empty };
                                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                                if (returnVal == 0) user = argList[0];

                            }
                            catch (Exception)
                            {
                            }

                            try
                            {

                                ProcessesDictionary.Add(new ProcessModel(tempProcess.ProcessName, tempProcess.Id, cpu,
                                    tempProcess.PrivateMemorySize64,
                                    tempProcess.Responding, tempProcess.Threads.Count,
                                    user, moduleName, fileName,
                                    d), true);
                            }
                            catch (Exception)
                            {
                            }

                        }
                        catch (Exception)
                        {
                        }

                    }
                    else // processs exists, so just update some values
                    {
                        Process tempProcess = Process.GetProcessById(procId);

                        ProcessesDictionary.Remove(oldModel);



                        oldModel.Active = tempProcess.Responding;
                        oldModel.Ram = tempProcess.PrivateMemorySize64;

                        TimeSpan cpu = TimeSpan.Zero;
                        try
                        {
                            cpu = tempProcess.TotalProcessorTime;
                        }
                        catch (Exception)
                        {
                        }

                        oldModel.Cpu = cpu;
                        oldModel.Threads = tempProcess.Threads.Count;


                        ProcessesDictionary.Add(oldModel, true);



                    }
                }
                catch { }
            }

            String s = ProcessesDictionary.Count.ToString() + " ";

            List<ProcessModel> toDelete = new List<ProcessModel>();

            foreach (var key in ProcessesDictionary.Keys.ToList())
            {
                if (ProcessesDictionary[key] == false)
                {
                    toDelete.Add(key);
                }
                else
                {
                    ProcessesDictionary[key] = false;
                }
            }

            foreach (var processModel in toDelete)
            {
                ProcessesDictionary.Remove(processModel);
            }

            s += ProcessesDictionary.Count.ToString();

            //MessageBox.Show(s);
        }


        //==================


        private DateTime GetStartTime(String date)
        {
            DateTime start = DateTime.MinValue;
            start = start.AddYears(int.Parse(date.Substring(0, 4)) - 1);
            start = start.AddMonths(int.Parse(date.Substring(4, 2)) - 1);
            start = start.AddDays(int.Parse(date.Substring(6, 2)) - 1);

            start = start.AddHours(int.Parse(date.Substring(8, 2)) - 1);
            start = start.AddMinutes(int.Parse(date.Substring(10, 2)) - 1);
            start = start.AddSeconds(int.Parse(date.Substring(12, 2)) - 1);

            start = start.AddHours((DateTime.Now.IsDaylightSavingTime()) ? 1 : 0);
            return start;
        }


    }
}
