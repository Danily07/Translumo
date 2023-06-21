using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Windows.Input;
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

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string ConfigurationPropertyName { get; }

        private HotKeyInfo _hotKey;
        private string _description;

        public HotKeyModel(HotKeyInfo hotKey, string configurationName, string description)
        {
            _hotKey = hotKey;
            _description = description;
            ConfigurationPropertyName = configurationName;
        }
    }
}
