using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Translumo.Utils
{
    public static class DispatcherExtensions
    {
        public static void RaiseOnUIThread<TArg>(this EventHandler<TArg> eventHandler, object sender, TArg arg)
            where TArg : EventArgs
        {
            foreach (var @delegate in eventHandler.GetInvocationList())
            {
                Application.Current.Dispatcher.BeginInvoke(@delegate, sender, arg);
            }
        }

        public static void RaiseOnUIThread(this EventHandler eventHandler, object sender)
        {
            foreach (var @delegate in eventHandler.GetInvocationList())
            {
                Application.Current.Dispatcher.BeginInvoke(@delegate, sender, EventArgs.Empty);
            }
        }

        public static void ExecuteOnUIThread(this ICommand command, object parameter)
        {
            Application.Current.Dispatcher.BeginInvoke(() => command.Execute(parameter));
        }
    }
}
