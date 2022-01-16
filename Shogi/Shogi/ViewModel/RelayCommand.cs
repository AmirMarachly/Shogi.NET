using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shogi.ViewModel
{
    /// <summary>
    /// Simple implementation of the ICommad interface
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> action;

        /// <summary>
        /// Construct the command with the given action
        /// </summary>
        /// <param name="action">The action</param>
        public RelayCommand(Action<object> action)
        {
            this.action = action;
        }

        /// <summary>
        /// The event from the interface, not used
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Whether the command can be runed, always true
        /// </summary>
        /// <param name="parameter">Not used</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Execute the action, with the parameter
        /// </summary>
        /// <param name="parameter">The parameter</param>
        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
