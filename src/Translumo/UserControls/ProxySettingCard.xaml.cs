using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace Translumo.Controls
{
    /// <summary>
    /// Interaction logic for ProxySettingCard.xaml
    /// </summary>
    public partial class ProxySettingCard : UserControl
    {

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(ProxySettingCard));

        public static readonly DependencyProperty IpAddressProperty =
            DependencyProperty.Register(
                "IpAddress", typeof(string), typeof(ProxySettingCard),
                new FrameworkPropertyMetadata(
                    defaultValue: default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IpAddressCallback));

        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register(
                "Port", typeof(string), typeof(ProxySettingCard),
                new FrameworkPropertyMetadata(
                    defaultValue: default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PortCallback));

        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register(
                "Login", typeof(string), typeof(ProxySettingCard),
                new FrameworkPropertyMetadata(
                    defaultValue: default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, LoginCallback));


        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password", typeof(string), typeof(ProxySettingCard),
                new FrameworkPropertyMetadata(
                    defaultValue: default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PasswordCallback));

        private static void IpAddressCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var targetControl = (ProxySettingCard)d;
            string newValue = Regex.Replace((e.NewValue as string) ?? string.Empty, "\\d+", 
                v => v.Value.PadRight(3, targetControl.TbIpAddress.PromptChar));
            if (newValue == e.OldValue as string)
            {
                return;
            }

            targetControl.TbIpAddress.Value = newValue;
        }

        private static void PortCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue as string == e.OldValue as string)
            {
                return;
            }

            var targetControl = (ProxySettingCard)d;
            targetControl.TbPort.Value = e.NewValue as string;
        }

        private static void LoginCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue as string == e.OldValue as string)
            {
                return;
            }

            var targetControl = (ProxySettingCard)d;
            targetControl.TbLogin.Text = e.NewValue as string;
        }

        private static void PasswordCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue as string == e.OldValue as string)
            {
                return;
            }

            var targetControl = (ProxySettingCard)d;
            targetControl.TbPassword.Text = e.NewValue as string;
        }
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public string IpAddress
        {
            get { return (string)GetValue(IpAddressProperty); }
            set { SetValue(IpAddressProperty, value); }
        }

        public string Port
        {
            get { return (string)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }


        public ProxySettingCard()
        {
            InitializeComponent();
        }

        private string GetNonMaskedValue(MaskedTextBox control)
        {
            return control.Value?.ToString()?.Replace(" ", string.Empty);
        }

        private void TbPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = GetNonMaskedValue(sender as MaskedTextBox);
            if (Port != newValue)
            {
                Port = newValue;
            }
        }

        private void TbIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = GetNonMaskedValue(sender as MaskedTextBox);
            if (IpAddress != newValue)
            {
                IpAddress = newValue;
            }
        }

        private void TbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = (sender as TextBox)?.Text;
            if (Login != newValue)
            {
                Login = newValue;
            }
        }

        private void TbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = (sender as TextBox)?.Text;
            if (Password != newValue)
            {
                Password = newValue;
            }
        }
    }
}
