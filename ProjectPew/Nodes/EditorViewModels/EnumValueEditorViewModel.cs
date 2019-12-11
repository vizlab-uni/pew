using NodeNetwork.Toolkit.ValueNode;
using ProjectHeracles.Nodes.EditorViews;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ProjectHeracles.Nodes.EditorViewModels
{
    public class EnumValueEditorViewModel : ValueEditorViewModel<object>
    {
        static EnumValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new EnumValueEditorView(), typeof(IViewFor<EnumValueEditorViewModel>));
        }

        public object[] Options { get; }
        public string[] OptionLabels { get; }

        #region SelectedOptionIndex
        private int _selectedOptionIndex;
        public int SelectedOptionIndex
        {
            get => _selectedOptionIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedOptionIndex, value);
        }
        #endregion

        public EnumValueEditorViewModel(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(enumType.Name + " is not an enum type");
            }
            Options = Enum.GetValues(enumType).Cast<object>().ToArray();
            OptionLabels = Options.Select(c => Enum.GetName(enumType, c)).ToArray();

            this.WhenAnyValue(vm => vm.SelectedOptionIndex)
                .Select(i => i == -1 ? null : Options[i])
                .BindTo(this, vm => vm.Value);
        }
    }
}
