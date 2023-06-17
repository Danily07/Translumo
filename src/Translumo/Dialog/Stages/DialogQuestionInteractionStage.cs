using System.Windows;
using Translumo.MVVM.Common;
using Translumo.MVVM.ViewModels;

namespace Translumo.Dialog.Stages
{
    public class DialogQuestionInteractionStage : ConditionalInteractionStage
    {
        public DialogQuestionInteractionStage(DialogService dialogService, string stageQuestion) : base(dialogService, async () =>
                (await dialogService.ShowDialogAsync(SimpleDialogViewModel.Create(stageQuestion, SimpleDialogTypes.Question))) == MessageBoxResult.OK)
        {
        }
    }
}
