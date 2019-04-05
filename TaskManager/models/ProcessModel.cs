using System;
using System.Collections.Generic;

namespace TaskManager.models
{
    public class ProcessModel
    {

        public string Name { get; private set; }

        public int Id { get; private set; }

        private double _cpuPercent;

        public double CpuPercent
        {
            get => _cpuPercent;
            set => _cpuPercent = value;
        }

        public string CpuPstg
        {
            get => CpuPercent + " %";
        }

        public string RamVal
        {
            get => ConvertProcessRam(Ram);
        }

        public double RamPercent
        {
            get
            {
                return Math.Round((Ram * 100.0 / ProcessManager.TotalRam), 4);
            }
        }

        public string RamPstg
        {
            get
            {
                return "" + Math.Round((Ram * 100.0 / ProcessManager.TotalRam), 4) + " %";
            }
        }

        public long Ram { get; set; }

        public bool Active { get; set; }

        public int Threads { get; set; }

        public string User { get; private set; }

        public string Path { get; private set; }

        public string FileName { get; private set; }

        public DateTime ExecStart { get; private set; }

        private DateTime _lastTime;
        private TimeSpan _lastTotalProcessorTime;

        public TimeSpan Cpu
        {
            get { return _lastTotalProcessorTime; }
            set
            {
                DateTime curTime = DateTime.Now;
                TimeSpan curTotalProcessorTime = value;

                CpuPercent = 100.0 * (curTotalProcessorTime.TotalMilliseconds - _lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(_lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);
                CpuPercent = Math.Round(CpuPercent, 3);
                _lastTime = curTime;
                _lastTotalProcessorTime = curTotalProcessorTime;
            }
        }

        public ProcessModel(string name, int id, TimeSpan cpu, long ram, bool active, int threads, string user, string path, string fileName, DateTime execStart)
        {
            Name = name;
            Id = id;
            Cpu = cpu;
            Ram = ram;
            Active = active;
            Threads = threads;
            User = user;
            Path = path;
            FileName = fileName;
            ExecStart = execStart;
            CpuPercent = 0.0;


            _lastTime = DateTime.Now;
            _lastTotalProcessorTime = cpu;
        }

        public override string ToString()
        {
            //return "";
            return "" + Name + " " + Cpu.ToString() + " " + Ram.ToString() + " " + Id.ToString();
        }

        private string ConvertProcessRam(long number)
        {
            List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

            for (int i = 0; i < suffixes.Count; i++)
            {
                long temp = number / (int)Math.Pow(1024, i + 1);

                if (temp == 0)
                {
                    return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                }
            }

            return number.ToString();
        }


    }
}
