using System.Diagnostics;

namespace Translumo.MVVM.Common
{
    public class OpenLinkCommand : CommandBase<OpenLinkCommand>
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var url = parameter?.ToString()?.Replace("&", "^&");
            if (!string.IsNullOrWhiteSpace(url))
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
        }
    }
}
