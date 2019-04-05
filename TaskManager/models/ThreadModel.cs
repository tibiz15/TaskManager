using System;
using System.Diagnostics;

namespace TaskManager.models
{
    class ThreadModel
    {
        public int Id { get; set; }
        public ThreadState Active { get; set; }
        public DateTime ExecStart { get; set; }

        public ThreadModel(int id, ThreadState active, DateTime execStart)
        {
            Id = id;
            Active = active;
            ExecStart = execStart;
        }
    }
}
