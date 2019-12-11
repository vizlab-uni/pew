using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;

namespace ProjectHeracles.Nodes.EditorViewModels
{
    public class SliderValueEditorViewModel : ValueEditorViewModel<double?>
    {
        static SliderValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new SliderValueEditorView(), typeof(IViewFor<SliderValueEditorViewModel>));
        }

        private int _minValue;
        public int MinValue
        {
            get => _minValue;
            set => this.RaiseAndSetIfChanged(ref _minValue, value);
        }

        private int _maxValue;
        public int MaxValue
        {
            get => _maxValue;
            set => this.RaiseAndSetIfChanged(ref _maxValue, value);
        }

        public SliderValueEditorViewModel()
        {
            MinValue = 0;
            MaxValue = 100;
            Value = 0;
        }

        public SliderValueEditorViewModel(int initValue, int min=0, int max=100)
        {
            Value = initValue;
            MinValue = min;
            MaxValue = max;
        }
    }
}
