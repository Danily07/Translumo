using System;
using System.Collections.Generic;
using System.Linq;
using Translumo.HotKeys;
using Translumo.MVVM.Models;
using Translumo.Utils;

namespace Translumo.MVVM.ViewModels
{
    public class HotkeysSettingsViewModel : IDisposable
    {
        public IList<DisplayHotKey> Model { get; set; }

        public HotkeysSettingsViewModel(HotKeysConfiguration hotKeysConfiguration)
        {
            Model = new List<DisplayHotKey>(new[]
            {
                new DisplayHotKey(hotKeysConfiguration.ChatVisibilityKey, LocalizationManager.GetValue("Str.Hotkeys.ChatVisibility", false, OnLocalizedValueChanged, this)),
                new DisplayHotKey(hotKeysConfiguration.SettingVisibilityKey, LocalizationManager.GetValue("Str.Hotkeys.SettingsVisibility", false, OnLocalizedValueChanged, this)),
                new DisplayHotKey(hotKeysConfiguration.SelectAreaKey, LocalizationManager.GetValue("Str.Hotkeys.SelectArea", false, OnLocalizedValueChanged, this)),
                new DisplayHotKey(hotKeysConfiguration.TranslationStateKey, LocalizationManager.GetValue("Str.Hotkeys.OnOffTranslation", false, OnLocalizedValueChanged, this))
            });
        }


        private void OnLocalizedValueChanged(string key, string oldValue)
        {
            var displayHotKey = Model.First(hk => hk.Description == oldValue);
            displayHotKey.Description = LocalizationManager.GetValue(key, false, OnLocalizedValueChanged, this);
        }

        public void Dispose()
        {
            LocalizationManager.ReleaseChangedValuesCallbacks(this);
        }
    }
}
