using Translumo.HotKeys;
using Translumo.Utils;

namespace Translumo.MVVM.Models
{
    public class HotKeyModel : BindableBase
    {
        public HotKeyInfo HotKey
        {
            get => _hotKey;
            set => SetProperty(ref _hotKey, value);
        }

        public GamepadHotKeyInfo GamepadHotKey
        {
            get => _gamepadHotKey;
            set => SetProperty(ref _gamepadHotKey, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string ConfigurationPropertyName { get; }
        public string GamepadConfigurationPropertyName { get; }

        private HotKeyInfo _hotKey;
        private GamepadHotKeyInfo _gamepadHotKey;
        private string _description;

        public HotKeyModel(HotKeyInfo hotKey, GamepadHotKeyInfo gamepadHotKey, string configurationName, string gamepadConfigurationPropertyName, string description)
        {
            _hotKey = hotKey;
            _description = description;
            _gamepadHotKey = gamepadHotKey;
            GamepadConfigurationPropertyName = gamepadConfigurationPropertyName;
            ConfigurationPropertyName = configurationName;
        }
    }
}
