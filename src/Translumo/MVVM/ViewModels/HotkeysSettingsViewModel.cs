using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Translumo.HotKeys;
using Translumo.MVVM.Models;
using Translumo.Utils;
using Translumo.Utils.Extensions;

namespace Translumo.MVVM.ViewModels
{
    public class HotkeysSettingsViewModel : IDisposable
    {
        public IList<HotKeyModel> Model { get; set; }

        public ICommand ExitEditModeCommand => new ActionCommand(OnExitEditMode);
        public ICommand EnterEditModeCommand => new ActionCommand(OnEnterEditMode);

        private readonly HotKeysConfiguration _configuration;
        private readonly HotKeysServiceManager _serviceManager;

        public HotkeysSettingsViewModel(HotKeysServiceManager hotKeysServiceManager)
        {
            this._configuration = hotKeysServiceManager.Configuration;
            this._serviceManager = hotKeysServiceManager;

            Model = new List<HotKeyModel>(new[]
            {
                new HotKeyModel(_configuration.ChatVisibilityKey, nameof(_configuration.ChatVisibilityKey), LocalizationManager.GetValue("Str.Hotkeys.ChatVisibility", false, OnLocalizedValueChanged, this)),
                new HotKeyModel(_configuration.SettingVisibilityKey, nameof(_configuration.SettingVisibilityKey), LocalizationManager.GetValue("Str.Hotkeys.SettingsVisibility", false, OnLocalizedValueChanged, this)),
                new HotKeyModel(_configuration.SelectAreaKey, nameof(_configuration.SelectAreaKey), LocalizationManager.GetValue("Str.Hotkeys.SelectArea", false, OnLocalizedValueChanged, this)),
                new HotKeyModel(_configuration.TranslationStateKey, nameof(_configuration.TranslationStateKey), LocalizationManager.GetValue("Str.Hotkeys.OnOffTranslation", false, OnLocalizedValueChanged, this))
            });

            Model.ForEach(m => m.PropertyChanged += OnPropertyChanged);
        }

        private void OnExitEditMode()
        {
            _serviceManager.RegisterAll();
        }

        private void OnEnterEditMode()
        {
            _serviceManager.UnregisterAll();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var model = sender as HotKeyModel;
            if (model == null || e.PropertyName != nameof(HotKeyModel.HotKey))
            {
                return;
            }

            var targetPropInfo = _configuration.GetType().GetProperty(model.ConfigurationPropertyName);
            var sameKeyModel = Model.FirstOrDefault(m => m.HotKey.Equals(model.HotKey) && m != model);
            if (sameKeyModel != null)
            {
                _serviceManager.UnregisterHotKey(model.ConfigurationPropertyName);
                sameKeyModel.HotKey = targetPropInfo.GetValue(_configuration) as HotKeyInfo;
            }

            targetPropInfo.SetValue(_configuration, model.HotKey);
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
