using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ProjectHeracles.Nodes.EditorViewModels;

namespace ProjectHeracles.Nodes.EditorViews
{
    public partial class EnumValueEditorView : UserControl, IViewFor<EnumValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(EnumValueEditorViewModel), typeof(EnumValueEditorView), new PropertyMetadata(null));

        public EnumValueEditorViewModel ViewModel
        {
            get => (EnumValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (EnumValueEditorViewModel)value;
        }
        #endregion

        public EnumValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.OptionLabels, v => v.valueComboBox.ItemsSource).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.SelectedOptionIndex, v => v.valueComboBox.SelectedIndex).DisposeWith(d);
            });
        }
    }
}
