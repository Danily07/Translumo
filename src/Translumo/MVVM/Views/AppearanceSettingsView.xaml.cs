using System.Windows.Controls;
using Translumo.Utils;

namespace Translumo.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AppearanceSettingsView.xaml
    /// </summary>
    public partial class AppearanceSettingsView : UserControl
    {
        public AppearanceSettingsView()
        {
            InitializeComponent();
        }

        private void btnWidowColor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            colorSelector.Caption = LocalizationManager.GetValue("Str.AppearanceSettings.FontSizePanelCaption");
        }

        private void btnFontColor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            colorSelector.Caption = LocalizationManager.GetValue("Str.AppearanceSettings.FontColorPanelCaption");
        }
    }
}
