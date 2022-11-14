using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace C968_Final.Commands
{
    public class RelayCommand<T> : ICommand
    {
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute));

            m_execute = execute;
            m_canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.m_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (this.m_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter) => m_canExecute is null ? true : m_canExecute((T)parameter);

        public void Execute(object parameter) => m_execute?.Invoke((T)parameter);

        readonly Action<T> m_execute;
        readonly Predicate<T> m_canExecute;
    }
}
