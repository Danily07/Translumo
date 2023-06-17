namespace Translumo.MVVM.Common
{
    public class ProxyCardItem
    {
        public string IpAddress { get; set; }

        public string Port { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(IpAddress) && !string.IsNullOrEmpty(Port) && !string.IsNullOrEmpty(Login);
        }
    }
}
