using System;
using System.Collections.Generic;

namespace Translumo.HotKeys
{
    public class HotKeysServiceManager
    {
        public event EventHandler SelectAreaKeyPressed;
        public event EventHandler ChatVisibilityKeyPressed;
        public event EventHandler TranslationStateKeyPressed;
        public event EventHandler SettingVisibilityKeyPressed;
        public HotKeysConfiguration Configuration { get; }

        private IList<HotKey> _registeredHotKeys;

        public HotKeysServiceManager(HotKeysConfiguration configuration)
        {
            this._registeredHotKeys = InitializeHotKeys(configuration);
            this.Configuration = configuration;
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

        private IList<HotKey> InitializeHotKeys(HotKeysConfiguration configuration)
        {
            return new List<HotKey>(new[]
            {
                new HotKey(configuration.ChatVisibilityKey.Key, configuration.ChatVisibilityKey.KeyModifier,
                    OnChatVisibilityPressed),
                new HotKey(configuration.SelectAreaKey.Key, configuration.SelectAreaKey.KeyModifier,
                    OnSelectAreaPressed),
                new HotKey(configuration.TranslationStateKey.Key, configuration.TranslationStateKey.KeyModifier,
                    OnTranslationStatePressed),
                new HotKey(configuration.SettingVisibilityKey.Key, configuration.SettingVisibilityKey.KeyModifier,
                    OnSettingVisibilityPressed)
            });
        }
    }
}
