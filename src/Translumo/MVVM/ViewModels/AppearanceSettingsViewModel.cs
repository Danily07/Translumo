using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.Input;
using Translumo.Configuration;
using Translumo.MVVM.Common;
using Translumo.Services;
using Translumo.Utils;
using RelayCommand = Microsoft.Toolkit.Mvvm.Input.RelayCommand;

namespace Translumo.MVVM.ViewModels
{
    public sealed class AppearanceSettingsViewModel : BindableBase, IAdditionalPanelController
    {
        public event EventHandler<bool> PanelStateIsChanged;

        public ChatWindowConfiguration Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public bool ColorPickerIsOpened
        {
            get => _colorPickerIsOpened;
            set
            {
                SetProperty(ref _colorPickerIsOpened, value);
                PanelStateIsChanged?.Invoke(this, value);
            }
        }

        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }

        public ICommand ChangeBackColorClickedCommand => new RelayCommand<Control>(OnChangeBackColorClicked);
        public ICommand ChangeFontColorClickedCommand => new RelayCommand<Control>(OnChangeFontColorClicked);
        public ICommand SendTestTextCommand => new RelayCommand(OnSendTestTextCommand);
        public ICommand ColorIsPickedCommand => new RelayCommand<bool>(OnColorPickedCommand);

        private ChatWindowConfiguration _model;
       
        private Action<ChatWindowConfiguration, Color> _newColorSetter;
        private string _additionalPanelTriggerName;
        private bool _colorPickerIsOpened;
        private Color _selectedColor;

        private readonly ChatUITextMediator _chatMediator;
        public AppearanceSettingsViewModel(ChatWindowConfiguration model, ChatUITextMediator chatMediator)
        {
            this.Model = model;
            this._chatMediator = chatMediator;
        }

        private void OnChangeBackColorClicked(Control sender)
        {
            OnChangeColorClicked(sender, (model, color) => model.BackgroundColor = color, Model.BackgroundColor);
        }

        private void OnChangeFontColorClicked(Control sender)
        {
            OnChangeColorClicked(sender, (model, color) => model.FontColor = color, Model.FontColor);
        }

        private void OnSendTestTextCommand()
        {
            _chatMediator.SendText($"Translated test text was sent", true);
        }

        private void OnColorPickedCommand(bool applyColor)
        {
            ClosePanel();
            if (applyColor)
            {
                _newColorSetter.Invoke(Model, SelectedColor);
            }

            SelectedColor = default(Color);
        }

        private void OnChangeColorClicked(Control sender, Action<ChatWindowConfiguration, Color> newColorSetter, Color currentColor)
        {
            OpenCloseColorPanel(sender.Name);

            _newColorSetter = newColorSetter;
            SelectedColor = currentColor;
        }

        private void OpenCloseColorPanel(string triggerControlName)
        {
            var toOpen = _additionalPanelTriggerName != triggerControlName;
            _additionalPanelTriggerName = toOpen ? triggerControlName : null;
            ColorPickerIsOpened = toOpen;
        }

        public void ClosePanel()
        {
            _additionalPanelTriggerName = null;
            ColorPickerIsOpened = false;
        }
    }
}
