using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;
using System;

namespace ProjectHeracles.Nodes.EditorViewModels
{
    //TODO: FINISH LATER
    public class ButtonValueEditorViewModel : ValueEditorViewModel<EventHandler>
    {
        static ButtonValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new ButtonValueEditorView(), typeof(IViewFor<ButtonValueEditorViewModel>));
        }

        public ButtonValueEditorViewModel()
        {
            Value = null;
        }
    }
}
