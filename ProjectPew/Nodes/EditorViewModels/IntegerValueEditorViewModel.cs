using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;

namespace ProjectHeracles.Nodes.EditorViewModels
{
    public class IntegerValueEditorViewModel : ValueEditorViewModel<int?>
    {
        static IntegerValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new IntegerValueEditorView(), typeof(IViewFor<IntegerValueEditorViewModel>));
        }

        public IntegerValueEditorViewModel()
        {
            Value = 0;
        }
    }
}
