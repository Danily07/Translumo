using System;
using System.Collections.Generic;
using System.ComponentModel;
using Translumo.Utils.Extensions;

namespace Translumo.HotKeys
{
    public class HotKeysServiceManager
    {
        public event EventHandler SelectAreaKeyPressed;
        public event EventHandler ChatVisibilityKeyPressed;
        public event EventHandler TranslationStateKeyPressed;
        public event EventHandler SettingVisibilityKeyPressed;
        public HotKeysConfiguration Configuration { get; }

        private readonly IDictionary<string, HotKey> _registeredHotKeys;

        public HotKeysServiceManager(HotKeysConfiguration configuration)
        {
            this._registeredHotKeys = InitializeHotKeys(configuration);
            this.Configuration = configuration;

            this.Configuration.PropertyChanged += ConfigurationOnPropertyChanged;
        }

        public void RegisterAll()
        {
            _registeredHotKeys.ForEach(key => RegisterHotKey(key.Key));
        }

        public void UnregisterAll()
        {
            _registeredHotKeys.ForEach(key => UnregisterHotKey(key.Key));
        }

        public void UnregisterHotKey(string keyActionName)
        {
            if (_registeredHotKeys.ContainsKey(keyActionName))
            {
                _registeredHotKeys[keyActionName].Unregister();
            }
        }

        public void RegisterHotKey(string keyActionName)
        {
            if (_registeredHotKeys.ContainsKey(keyActionName))
            {
                _registeredHotKeys[keyActionName].Register();
            }
        }

        private void ConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName) && _registeredHotKeys.ContainsKey(e.PropertyName))
            {
                var newValueProperty = typeof(HotKeysConfiguration).GetProperty(e.PropertyName);
                var newValue = newValueProperty.GetValue(Configuration) as HotKeyInfo;
                _registeredHotKeys[e.PropertyName].Reassign(newValue.Key, newValue.KeyModifier);
            }
        }

        private void OnTranslationStatePressed(HotKey obj)
        {
            TranslationStateKeyPressed?.Invoke(obj, EventArgs.Empty);
        }

        private void OnSelectAreaPressed(HotKey obj)
        {
            SelectAreaKeyPressed?.Invoke(obj, EventArgs.Empty);
        }

        private void OnChatVisibilityPressed(HotKey obj)
        {
            ChatVisibilityKeyPressed?.Invoke(obj, EventArgs.Empty);
        }

        private void OnSettingVisibilityPressed(HotKey obj)
        {
            SettingVisibilityKeyPressed?.Invoke(obj, EventArgs.Empty);
        }

        private IDictionary<string, HotKey> InitializeHotKeys(HotKeysConfiguration configuration)
        {
            return new Dictionary<string, HotKey>()
            {
                {
                    nameof(configuration.ChatVisibilityKey), new HotKey(configuration.ChatVisibilityKey.Key, 
                        configuration.ChatVisibilityKey.KeyModifier, OnChatVisibilityPressed)
                },
                {
                    nameof(configuration.SelectAreaKey), new HotKey(configuration.SelectAreaKey.Key,
                        configuration.SelectAreaKey.KeyModifier, OnSelectAreaPressed)
                },
                {
                    nameof(configuration.TranslationStateKey), new HotKey(configuration.TranslationStateKey.Key,
                        configuration.TranslationStateKey.KeyModifier, OnTranslationStatePressed)
                },
                {
                    nameof(configuration.SettingVisibilityKey), new HotKey(configuration.SettingVisibilityKey.Key,
                        configuration.SettingVisibilityKey.KeyModifier, OnSettingVisibilityPressed)
                }
            };
        }
    }
}
