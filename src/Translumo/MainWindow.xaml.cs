//using ScreenTranslator.Core.HotKeys;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Interop;
//using System.Windows.Media;
//using Windows.Media.Ocr;
//using MaterialDesignThemes.Wpf;
//using Microsoft.Extensions.DependencyInjection;
//using ScreenTranslator.Core.Configuration;
//using ScreenTranslator.Core.MVVM.Views;
//using ScreenTranslator.Core.Utils;

//namespace ScreenTranslator.Core
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class MainWindow : Window
//    {
//        private const int MAX_PHRASES = 30;

//        private readonly HotKey _screenHk;
//        private readonly HotKey _visibility;
//        private readonly HotKey _selectionArea;
//        private readonly HotKey _new;

//        private TranslationProcessorService _service;

//        private int _displayedPhrases = 0;

//        private ScreenCaptureConfiguration _captConfiguration;
//        private readonly IServiceProvider _serviceProvider;
//        private readonly ChatWindowConfiguration _chatConfiguration;

//        public MainWindow(IServiceProvider serviceProvider)
//        {
//            this._serviceProvider = serviceProvider;
//            this._chatConfiguration = serviceProvider.GetService<ChatWindowConfiguration>();
//            this._chatConfiguration.PropertyChanged += ChatConfigurationOnPropertyChanged;
//            var chatMediator = serviceProvider.GetService<ChatTextMediator>();
//            chatMediator.TextArrived += OnTextTranslated;
//            InitializeComponent();

//            _screenHk = new HotKey(Key.OemTilde, KeyModifier.None, OnHotKeyScreenHandler);
//            _visibility = new HotKey(Key.T, KeyModifier.Alt, OnHotKeyVisibilityHandler);
//            _selectionArea = new HotKey(Key.R, KeyModifier.Alt, OnHotKeySelectionAreaHandler);

//            _captConfiguration = new ScreenCaptureConfiguration();
//            //TODO: DI
//            _service = new TranslationProcessorService(_captConfiguration, chatMediator);

//            rtbChat.BorderThickness = new Thickness(0);

//            var view = _serviceProvider.GetService<SettingsView>();
//            view.ShowDialog();
//        }

//        private void ChatConfigurationOnPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == nameof(_chatConfiguration.BackgroundColor) || e.PropertyName == nameof(_chatConfiguration.BackgroundOpacity))
//            {
//                var color = _chatConfiguration.BackgroundColor;
//                color.ScA = _chatConfiguration.BackgroundOpacity;
//                this.Background = new SolidColorBrush(color);
//            }

//            if (e.PropertyName == nameof(_chatConfiguration.FontColor))
//            {
//                rtbChat.Foreground = new SolidColorBrush(_chatConfiguration.FontColor);
//            }
//        }

//        private void OnTextTranslated(object sender, TranslatedEventArgs e)
//        {
//            var color = e.Successful ? _chatConfiguration.FontColor : Color.FromRgb(230, 15, 15);
//            this.Dispatcher.Invoke(() =>
//            {
//                AppendText(e.Text, color);
//            });
//        }

//        private void rtbChat_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (e.ChangedButton == MouseButton.Left)
//                this.DragMove();
//        }

//        private void OnHotKeyScreenHandler(HotKey hotKey)
//        {
//            if (_service.IsStarted)
//            {
//                _service.StopTranslation();
//                AppendText("Stopped", Color.FromRgb(43, 255, 149));
//            }
//            else
//            {
//                _service.StartTranslation();
//                AppendText("Started", Color.FromRgb(43, 255, 149));
//            }
//        }

//        private void AppendText(string text, Color rgbColor)
//        {
//            if (string.IsNullOrEmpty(text))
//                return;

//            if (_displayedPhrases > MAX_PHRASES)
//            {
//                TextRange txt = new TextRange(rtbChat.Document.ContentStart, rtbChat.Document.ContentEnd);
//                txt.Text = string.Empty;
//                _displayedPhrases = 0;
//            }

//            rtbChat.CaretPosition = rtbChat.CaretPosition.DocumentEnd;

//            var color = new SolidColorBrush(rgbColor);
//            var textRange = new TextRange(rtbChat.Document.ContentEnd, rtbChat.Document.ContentEnd);
//            textRange.Text = text;
//            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
//            textRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

//            rtbChat.ScrollToEnd();

//            _displayedPhrases++;

//            rtbChat.AppendText(Environment.NewLine);
//        }

//        private void OnHotKeyVisibilityHandler(HotKey hotKey)
//        {
//            this.Visibility = this.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
//            if (this.Visibility == Visibility.Visible && !_service.IsStarted)
//            {
//                //_service.StartTranslation();
//                //AppendText("Started", Color.FromRgb(43, 255, 149));
//            }
//            else if (this.Visibility == Visibility.Hidden && _service.IsStarted)
//            {
//                _service.StopTranslation();
//                AppendText("Stopped", Color.FromRgb(43, 255, 149));
//            }
//        }

//        private void OnHotKeySelectionAreaHandler(HotKey hotKey)
//        {
//            var window = new SelectionAreaWindow();
//            var result = window.ShowDialog();
//            if (result.HasValue && result.Value)
//            {
//                _captConfiguration.CaptureAreaP1 = window.MouseInitialPos;
//                _captConfiguration.CaptureAreaP2 = window.MouseEndPos;
//            }
//        }

//        private void Window_Activated(object sender, EventArgs e)
//        {
//            this.Topmost = true;
//        }

//        private void Window_Deactivated(object sender, EventArgs e)
//        {
//            this.Topmost = true;
//            this.Activate();
//        }

//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {
//        }
//    }
//}
