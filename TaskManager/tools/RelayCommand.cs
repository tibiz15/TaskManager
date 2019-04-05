using System;
using System.Windows.Input;

namespace Lab4.tools
{
    class RelayCommand : ICommand
    {
        private Action<object> _action;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                _action(parameter);
            }
            else
            {
                _action("default_action");
            }
        }


    }
}
