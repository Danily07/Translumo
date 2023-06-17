using System.Windows;
using Translumo.Utils;

namespace Translumo.MVVM.Common
{
    public class CloseAppCommand : CommandBase<CloseAppCommand>
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var useConfirmation = parameter != null && (bool)parameter;
            if (useConfirmation)
            {
                if (MessageBox.Show(LocalizationManager.GetValue("Str.ExitConfirmation", true),
                        LocalizationManager.GetValue("Str.ExitConfirmationCaption"), MessageBoxButton.YesNo) !=
                    MessageBoxResult.Yes)
                {
                    return;
                }
            }

            Application.Current.Shutdown();
        }
    }
}
