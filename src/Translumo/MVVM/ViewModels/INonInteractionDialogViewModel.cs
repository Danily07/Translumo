using System;

namespace Translumo.MVVM.ViewModels
{
    public interface INonInteractionDialogViewModel
    {
        event EventHandler DialogIsClosed;
    }
}
