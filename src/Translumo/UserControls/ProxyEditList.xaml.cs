using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Translumo.MVVM.Common;

namespace Translumo.Controls
{
    /// <summary>
    /// Interaction logic for ProxyEditList.xaml
    /// </summary>
    public partial class ProxyEditList : UserControl
    {
        public static readonly DependencyProperty SourceListProperty =
            DependencyProperty.Register(
                "SourceList", typeof(ObservableCollection<ProxyCardItem>), typeof(ProxyEditList),
                new FrameworkPropertyMetadata(
                    defaultValue: default(ObservableCollection<ProxyCardItem>), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SourceListCallback));

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(ProxyEditList));
        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(ProxyEditList));
        public static readonly DependencyProperty SubmitCommandProperty = DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(ProxyEditList));


        private static void SourceListCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((ObservableCollection<ProxyCardItem>)e.NewValue == (ObservableCollection<ProxyCardItem>)e.OldValue)
            {
                return;
            }

            var targetControl = (ProxyEditList)d;
            targetControl.IcProxyList.DataContext = (ObservableCollection<ProxyCardItem>)e.NewValue;
        }

        public ObservableCollection<ProxyCardItem> SourceList
        {
            get { return (ObservableCollection<ProxyCardItem>)GetValue(SourceListProperty); }
            set { SetValue(SourceListProperty, value); }
        }

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public ICommand SubmitCommand
        {
            get { return (ICommand)GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }


        public ProxyEditList()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            SubmitCommand.Execute(true);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SubmitCommand.Execute(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SourceList.Add(new ProxyCardItem());
            ScProxyContent.ScrollToEnd();
        }

        private void DpScroll_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!e.HeightChanged)
            {
                return;
            }

            ScProxyContent.MaxHeight = DpScroll.ActualHeight - (BtnAddNew.Height + BtnAddNew.Margin.Top);
        }

        private void ProxySettingCard_RemoveIsClicked(object sender, EventArgs e)
        {
            IEditableCollectionView items = IcProxyList.Items; //Cast to interface
            if (items.CanRemove)
            {
                items.Remove(sender);
            }
        }
    }
}
