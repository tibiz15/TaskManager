using System.Windows;
using TaskManager.models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow(ProcessModel process, bool threads)
        {
            InitializeComponent();
            DataContext = new InfoViewModel(process, threads);
        }
    }
}
