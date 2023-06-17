using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Translumo.Infrastructure;
using Translumo.MVVM.Models;
using Translumo.MVVM.ViewModels;
using Translumo.Utils;

namespace Translumo.MVVM.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatWindowView : Window
    {
        public ChatWindowView()
        {
            InitializeComponent();
        }

        private void ModelOnChatFirstItemsRemoved(object sender, ChatFirstItemsRemovedEventArgs e)
        {
            RemoveFirstTextBlocks(e.NumberRemoved);
        }

        private void ModelOnChatItemAdded(object sender, ChatItemAddedEventArgs e)
        {
            AppendTextBlock(e.Text, e.TextType);
        }

        private void RemoveFirstTextBlocks(int numberBlocks)
        {
            var blocksCollection = rtbChat.Document.Blocks;
            var toDelete = Math.Min(numberBlocks, blocksCollection.Count);
            for (var i = 0; i < toDelete; i++)
            {
                blocksCollection.Remove(blocksCollection.FirstBlock);
            }
        }

        private void AppendTextBlock(string text, TextTypes textType)
        {
             if (string.IsNullOrEmpty(text))
                return;

             rtbChat.CaretPosition = rtbChat.CaretPosition.DocumentEnd;

            var paragraph = new Paragraph { LineHeight = fontTextBlockInstance.LineHeight };
            var run = GetRunInstance(textType).Clone(text);
            paragraph.Inlines.Add(run);
            rtbChat.Document.Blocks.Add(paragraph);
            
            rtbChat.ScrollToEnd();
        }

        private void rtbChat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var viewModel = DataContext as ChatWindowViewModel;

            viewModel.Model.ChatItemAdded += ModelOnChatItemAdded;
            viewModel.Model.ChatFirstItemsRemoved += ModelOnChatFirstItemsRemoved;
        }

        private Run GetRunInstance(TextTypes textType)
        {
            switch (textType)
            {
                case TextTypes.Info:
                    return fontRunInfoInstance;

                case TextTypes.Error:
                    return fontRunErrorInstance;
                default:
                    return fontRunInstance;
            }
        }
    }
}
