using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;
namespace ProjectHeracles.Nodes.EditorViewModels
{
    class SliderFloatValueEditorViewModel : ValueEditorViewModel<double?>
    {
        static SliderFloatValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new SliderValueEditorView(), typeof(IViewFor<SliderFloatValueEditorViewModel>));
        }

        private double _minValue;
        public double MinValue
        {
            get => _minValue;
            set => this.RaiseAndSetIfChanged(ref _minValue, value);
        }

        private double _maxValue;
        public double MaxValue
        {
            get => _maxValue;
            set => this.RaiseAndSetIfChanged(ref _maxValue, value);
        }

        public SliderFloatValueEditorViewModel()
        {
            MinValue = 0.0;
            MaxValue = 1.5;
            Value = 0.0;
        }

        public SliderFloatValueEditorViewModel(double initValue, double min = 0.0, double max = 1.5)
        {
            Value = initValue;
            MinValue = min;
            MaxValue = max;
        }
    }
}
