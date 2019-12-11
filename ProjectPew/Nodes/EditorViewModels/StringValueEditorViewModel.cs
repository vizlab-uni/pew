using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;

namespace ProjectHeracles.Nodes.EditorViewModels
{
    public class StringValueEditorViewModel : ValueEditorViewModel<string>
    {
        static StringValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new StringValueEditorView(), typeof(IViewFor<StringValueEditorViewModel>));
        }

        public StringValueEditorViewModel()
        {
            Value = "";
        }
    }
}
