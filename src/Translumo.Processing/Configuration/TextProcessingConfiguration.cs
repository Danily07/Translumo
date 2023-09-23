using Translumo.Utils;

namespace Translumo.Processing.Configuration
{
    public class TextProcessingConfiguration : BindableBase
    {
        public static TextProcessingConfiguration Default => new TextProcessingConfiguration()
        {
            KeepFormatting = true
        };

        public bool KeepFormatting
        {
            get => _keepFormatting;
            set
            {
                SetProperty(ref _keepFormatting, value);
            }
        }

        private bool _keepFormatting;

    }
}
