using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Marketplaes02_for_sotrudnik.Model
{
    public class ActionCommand : ICommand
    {
        Action<object> action;

        public ActionCommand(Action<object> action) 
        {
            this.action = action;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) 
        {
          return  parameter != null;
        } 

        public void Execute(object? parameter) 
        {
            action?.Invoke(parameter);
        }
    }
}
