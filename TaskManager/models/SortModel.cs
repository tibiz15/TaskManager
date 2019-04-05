using System.Collections.Generic;
using System.Linq;

namespace TaskManager.models
{
    public enum SortType { name = 1, id, cpu, ram, active, threads, user, path, fileName, execStart };


    class SortModel
    {

        static public List<ProcessModel> Sort(List<ProcessModel> processes, SortType type, bool ascending)
        {

            switch (type)
            {
                case SortType.name:

                    return @ascending
                        ? processes.OrderBy(person => person.Name).ToList()
                        : processes.OrderByDescending(person => person.Name).ToList();

                case SortType.id:
                    return @ascending
                        ? processes.OrderBy(person => person.Id).ToList()
                        : processes.OrderByDescending(person => person.Id).ToList();

                case SortType.cpu:
                    return @ascending
                        ? processes.OrderBy(person => person.CpuPercent).ToList()
                        : processes.OrderByDescending(person => person.CpuPercent).ToList();

                case SortType.ram:
                    return @ascending
                        ? processes.OrderBy(person => person.RamPercent).ToList()
                        : processes.OrderByDescending(person => person.RamPercent).ToList();

                case SortType.active:
                    return @ascending
                        ? processes.OrderBy(person => person.Active).ToList()
                        : processes.OrderByDescending(person => person.Active).ToList();

                case SortType.threads:
                    return @ascending
                        ? processes.OrderBy(person => person.Threads).ToList()
                        : processes.OrderByDescending(person => person.Threads).ToList();

                case SortType.user:
                    return @ascending
                        ? processes.OrderBy(person => person.User).ToList()
                        : processes.OrderByDescending(person => person.User).ToList();


                case SortType.fileName:
                    return @ascending
                        ? processes.OrderBy(person => person.FileName).ToList()
                        : processes.OrderByDescending(person => person.FileName).ToList();

                case SortType.path:
                    return @ascending
                        ? processes.OrderBy(person => person.Path).ToList()
                        : processes.OrderByDescending(person => person.Path).ToList();

                case SortType.execStart:
                    return @ascending
                        ? processes.OrderBy(person => person.ExecStart).ToList()
                        : processes.OrderByDescending(person => person.ExecStart).ToList();

                default:
                    return processes;
            }

        }


    }
}
