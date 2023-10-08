using System.Windows;
using Translumo.Utils;

namespace Translumo.MVVM.Models
{
    public class DisplayAlignment : BindableBase
    {
        public TextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                SetProperty(ref _textAlignment, value);
            }
        }

        public string DisplayText
        {
            get => _displayText;
            set
            {
                SetProperty(ref _displayText, value);
            }
        }


        private TextAlignment _textAlignment;
        private string _displayText;

        public DisplayAlignment(TextAlignment textAlignment, string displayText)
        {
            this.TextAlignment = textAlignment;
            this.DisplayText = displayText;
        }
    }
}
